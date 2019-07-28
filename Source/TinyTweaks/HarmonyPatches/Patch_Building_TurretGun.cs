using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using Harmony;

namespace TinyTweaks
{

    public static class Patch_Building_TurretGun
    {

        [HarmonyPatch(typeof(Building_TurretGun))]
        [HarmonyPatch(nameof(Building_TurretGun.GetGizmos))]
        public static class Patch_GetGizmos
        {

            public static void Postfix(Building_TurretGun __instance, ref IEnumerable<Gizmo> __result)
            {
                // If the turret's gun has CompChangeableProjectile and is controlled by the player, add copy/paste storage setting gizmos for the comp
                if (__instance.gun?.TryGetComp<CompChangeableProjectile>() is CompChangeableProjectile changeableProjectileComp && ReflectedProperties.Building_TurretGun_get_PlayerControlled(__instance))
                    __result = __result.Concat(StorageSettingsClipboard.CopyPasteGizmosFor(changeableProjectileComp.GetStoreSettings()));
            }

        }

        [HarmonyPatch(typeof(Building_TurretGun))]
        [HarmonyPatch("TryStartShootSomething")]
        public static class Patch_TryStartShootSomething
        {

            public static void Postfix(Building_TurretGun __instance, ref int ___burstWarmupTicksLeft)
            {
                // If the turret has CompMannable, scale its warmup ticks with the manning pawn's aiming time
                if ((!ModCompatibilityCheck.TurretExtensions || TinyTweaksSettings.overrideMannedTurretFunctionality) && __instance.GetComp<CompMannable>() is CompMannable mannableComp && mannableComp.MannedNow)
                    ___burstWarmupTicksLeft = Mathf.RoundToInt(___burstWarmupTicksLeft * mannableComp.ManningPawn.GetStatValue(StatDefOf.AimingDelayFactor));
            }

        }

    }

}

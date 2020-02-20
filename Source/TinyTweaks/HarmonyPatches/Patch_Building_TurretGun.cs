using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace TinyTweaks
{

    public static class Patch_Building_TurretGun
    {

        [HarmonyPatch(typeof(Building_TurretGun))]
        [HarmonyPatch(nameof(Building_TurretGun.GetGizmos))]
        public static class GetGizmos
        {

            public static void Postfix(Building_TurretGun __instance, ref IEnumerable<Gizmo> __result)
            {
                // If the turret's gun has CompChangeableProjectile and is controlled by the player, add copy/paste storage setting gizmos for the comp
                if (__instance.gun?.TryGetComp<CompChangeableProjectile>() is CompChangeableProjectile changeableProjectileComp && NonPublicProperties.Building_TurretGun_get_PlayerControlled(__instance))
                    __result = __result.Concat(StorageSettingsClipboard.CopyPasteGizmosFor(changeableProjectileComp.GetStoreSettings()));
            }

        }

        [HarmonyPatch(typeof(Building_TurretGun), nameof(Building_TurretGun.SpawnSetup))]
        public static class SpawnSetup
        {

            public static void Postfix(Building_TurretGun __instance, TurretTop ___top)
            {
                // Set the turret gun's initial rotation to be identical to the turret's rotation
                if (TinyTweaksSettings.turretRotationFix)
                    NonPublicProperties.TurretTop_set_CurRotation(___top, __instance.Rotation.AsAngle);
            }

        }

        [HarmonyPatch(typeof(Building_TurretGun), nameof(Building_TurretGun.Tick))]
        public static class Patch_Tick
        {

            public static void Postfix(Building_TurretGun __instance, LocalTargetInfo ___forcedTarget)
            {
                // If the turret has CompSmartForcedTarget and is attacking a pawn that just got downed, automatically make it target something else
                if (TinyTweaksSettings.smarterTurretTargeting && (!ModCompatibilityCheck.TurretExtensions || TinyTweaksSettings.overrideSmarterForcedTargeting))
                {
                    var smartTargetComp = __instance.TryGetComp<CompSmarterTurretTargeting>();
                    if (smartTargetComp != null && ___forcedTarget.Thing is Pawn pawn)
                    {
                        if (!pawn.Downed && !smartTargetComp.attackingNonDownedPawn)
                            smartTargetComp.attackingNonDownedPawn = true;

                        else if (pawn.Downed && smartTargetComp.attackingNonDownedPawn)
                        {
                            smartTargetComp.attackingNonDownedPawn = false;
                            NonPublicMethods.Building_TurretGun_ResetForcedTarget(__instance);
                        }
                    }
                }
            }

        }

        [HarmonyPatch(typeof(Building_TurretGun))]
        [HarmonyPatch("TryStartShootSomething")]
        public static class TryStartShootSomething
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

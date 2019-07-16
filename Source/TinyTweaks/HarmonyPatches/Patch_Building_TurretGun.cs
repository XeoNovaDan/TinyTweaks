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
        public static class Patch_SpecialDisplayStats
        {

            public static void Postfix(Building_TurretGun __instance, ref IEnumerable<Gizmo> __result)
            {
                // If the turret's gun has CompChangeableProjectile and is controlled by the player, add copy/paste storage setting gizmos for the comp
                if (TinyTweaksSettings.artilleryCopyPasteGizmos && __instance.gun?.TryGetComp<CompChangeableProjectile>() is CompChangeableProjectile changeableProjectileComp)
                {
                    bool playerControlled = (bool)typeof(Building_TurretGun).GetProperty("PlayerControlled", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true).Invoke(__instance, null);
                    if (playerControlled)
                        foreach (var storageGizmo in StorageSettingsClipboard.CopyPasteGizmosFor(changeableProjectileComp.GetStoreSettings()))
                            __result = __result.Add(storageGizmo);
                }
            }

        }

    }

}

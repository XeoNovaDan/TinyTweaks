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

    public static class Patch_ThingDef
    {

        [HarmonyPatch(typeof(ThingDef))]
        [HarmonyPatch(nameof(ThingDef.AllRecipes), MethodType.Getter)]
        public static class Patch_AllRecipes_Getter
        {

            public static void Postfix(ref List<RecipeDef> __result)
            {
                // Sort the resulting list alphabetically
                if (TinyTweaksSettings.alphabeticalBillList)
                    __result = __result.OrderBy(r => r.label).ToList();
            }

        }

        [HarmonyPatch(typeof(ThingDef))]
        [HarmonyPatch(nameof(ThingDef.SpecialDisplayStats))]
        public static class Patch_SpecialDisplayStats
        {

            public static void Postfix(ThingDef __instance, StatRequest req, ref IEnumerable<StatDrawEntry> __result)
            {
                // Add the turret's weapons stats to the list of SpecialDisplayStats
                if (!ModCompatibilityCheck.TurretExtensions || TinyTweaksSettings.overrideTurretStatsFunctionality)
                {
                    var buildingProps = __instance.building;
                    if (buildingProps != null && buildingProps.IsTurret)
                        __result = __result.Concat(buildingProps.turretGunDef.SpecialDisplayStats(req).Where(s => s.category == StatCategoryDefOf.Weapon));
                }
            }

        }

    }

}

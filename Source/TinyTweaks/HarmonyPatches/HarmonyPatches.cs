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

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {

        static HarmonyPatches()
        {
            //HarmonyInstance.DEBUG = true;
            TinyTweaks.HarmonyInstance.PatchAll();

            // Alert_ColonistNeedsTend.NeedingColonists iterator
            TinyTweaks.HarmonyInstance.Patch(typeof(Alert_ColonistNeedsTend).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First().GetMethod("MoveNext", BindingFlags.Public | BindingFlags.Instance),
                transpiler: new HarmonyMethod(typeof(Patch_Alert_ColonistNeedsTend.manual_get_NeedingColonists), "Transpiler"));

            // Pawn_PlayerSettings ctor
            TinyTweaks.HarmonyInstance.Patch(typeof(Pawn_PlayerSettings).GetConstructor(new Type[] { typeof(Pawn) }), postfix: new HarmonyMethod(typeof(Patch_Pawn_PlayerSettings.manual_Ctor), "Postfix"));

            // Turret Extensions
            if (ModCompatibilityCheck.TurretExtensions)
            {
                var patchStatsReportUtility = GenTypes.GetTypeInAnyAssemblyNew("TurretExtensions.Patch_StatsReportUtility", "TurretExtensions");
                if (patchStatsReportUtility != null)
                {
                    var patchStatsToDrawDefThingDef = patchStatsReportUtility.GetNestedType("Patch_StatsToDraw_Def_ThingDef", BindingFlags.Public | BindingFlags.Static);
                    if (patchStatsToDrawDefThingDef != null)
                        TinyTweaks.HarmonyInstance.Patch(AccessTools.Method(patchStatsToDrawDefThingDef, "Postfix"), new HarmonyMethod(typeof(Patch_TurretExtensions_Patch_StatsReportUtility.manual_Patch_StatsToDraw_Def_ThingDef_Postfix), "Prefix"));
                    else
                        Log.Error("Could not find type TurretExtensions.Patch_StatsReportUtility.Patch_StatsToDraw_Def_ThingDef in Turret Extensions");

                    var patchStatsToDrawThing = patchStatsReportUtility.GetNestedType("Patch_StatsToDraw_Thing", BindingFlags.Public | BindingFlags.Static);
                    if (patchStatsToDrawThing != null)
                        TinyTweaks.HarmonyInstance.Patch(AccessTools.Method(patchStatsToDrawThing, "Postfix"), new HarmonyMethod(typeof(Patch_TurretExtensions_Patch_StatsReportUtility.manual_Patch_StatsToDraw_Thing_Postfix), "Prefix"));
                    else
                        Log.Error("Could not find type TurretExtensions.Patch_StatsReportUtility.Patch_StatsToDraw_Thing in Turret Extensions");
                }
                else
                    Log.Error("Could not find type TurretExtensions.Patch_StatsReportUtility in Turret Extensions");
                
            }
        }

    }

}

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

    public static class Patch_RaceProperties
    {

        [HarmonyPatch(typeof(RaceProperties))]
        [HarmonyPatch(nameof(RaceProperties.SpecialDisplayStats))]
        public static class Patch_SpecialDisplayStats
        {

            public static void Postfix(ThingDef parentDef, ref IEnumerable<StatDrawEntry> __result)
            {
                // If leather type is in the list of stats to display, add report text showing the leather's description and stats
                if (__result.Any(sde => sde.LabelCap == "LeatherType".Translate().CapitalizeFirst()))
                {
                    var resultList = __result.ToList();
                    var leatherEntry = resultList.First(sde => sde.LabelCap == "LeatherType".Translate().CapitalizeFirst());
                    var newLeatherEntry = new StatDrawEntry(leatherEntry.category, "LeatherType".Translate(), leatherEntry.ValueString, leatherEntry.DisplayPriorityWithinCategory, LeatherStatDrawEntryReportText(parentDef.race.leatherDef));
                    resultList.Remove(leatherEntry);
                    resultList.Add(newLeatherEntry);
                    __result = resultList;
                }
            }

            public static string LeatherStatDrawEntryReportText(ThingDef leatherDef)
            {
                // Leather's description
                var reportBuilder = new StringBuilder();
                reportBuilder.AppendLine(leatherDef.description);
                reportBuilder.AppendLine();

                // Leather's stats
                reportBuilder.AppendLine($"{"TabStats".Translate()}:");
                foreach (var statBase in leatherDef.statBases)
                    reportBuilder.AppendLine($"    {statBase.stat.LabelCap}: {statBase.value.ToStringByStyle(statBase.stat.toStringStyle)}");

                return reportBuilder.ToString();
            }

        }

    }

}

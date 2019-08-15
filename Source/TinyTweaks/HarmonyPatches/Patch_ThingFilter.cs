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

    public static class Patch_ThingFilter
    {

        [HarmonyPatch(typeof(ThingFilter))]
        [HarmonyPatch(nameof(ThingFilter.SetFromPreset))]
        public static class SetFromPreset
        {

            public static void Postfix(ThingFilter __instance, StorageSettingsPreset preset)
            {
                if (preset == StorageSettingsPreset.DefaultStockpile)
                {

                }

                else
                {
                    // Automatically accept 'Waste' from Dubs Bad Hygiene
                    if (TinyTweaksSettings.dumpingStockpilesAcceptWaste && ModCompatibilityCheck.DubsBadHygiene)
                        __instance.SetAllow(ThingCategoryDef.Named("Waste"), true);
                }
                
            }

        }

        [HarmonyPatch(typeof(ThingFilter), nameof(ThingFilter.Allows), new Type[] { typeof(Thing) })]
        public static class Allows_Thing
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();

                var disallowedSpecialFiltersInfo = AccessTools.Field(typeof(ThingFilter), "disallowedSpecialFilters");
                var getItemInfo = AccessTools.Method(typeof(List<SpecialThingFilterDef>), "get_Item");
                var matchesInfo = AccessTools.Method(typeof(SpecialThingFilterWorker), nameof(SpecialThingFilterWorker.Matches));
                var matchesDisallowedFilterInfo = AccessTools.Method(typeof(Allows_Thing), nameof(MatchesDisallowedFilter));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    if (instruction.opcode == OpCodes.Callvirt && instruction.operand == matchesInfo)
                    {
                        yield return instruction;
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        yield return new CodeInstruction(OpCodes.Ldfld, disallowedSpecialFiltersInfo); // this.disallowedSpecialFilters
                        yield return new CodeInstruction(OpCodes.Ldloc_2); // i
                        yield return new CodeInstruction(OpCodes.Callvirt, getItemInfo); // this.disallowedSpecialFilters[i]
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // t
                        instruction = new CodeInstruction(OpCodes.Call, matchesDisallowedFilterInfo); // MatchesDisallowedFilter(this.disallowedSpecialFilters[i].Worker.Matches(t), this.disallowedSpecialFilters[i], t)
                    }

                    yield return instruction;
                }
            }

            private static bool MatchesDisallowedFilter(bool result, SpecialThingFilterDef disallowedFilter, Thing t)
            {
                if (result && TinyTweaksSettings.specialThingFilterMatchFix)
                    return t.def.IsWithinCategory(disallowedFilter.parentCategory);
                return result;
            }

        }

    }

}

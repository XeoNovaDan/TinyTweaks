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

    public static class Patch_Dialog_AdvancedGameConfig
    {

        [HarmonyPatch(typeof(Dialog_AdvancedGameConfig), nameof(Dialog_AdvancedGameConfig.DoWindowContents))]
        [HarmonyPatch()]
        public static class DoWindowContents
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                #if DEBUG
                    Log.Message("Transpiler start: Dialog_AdvancedGameConfig.DoWindowContents (1 match)");
                #endif

                var instructionList = instructions.ToList();

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Look for the 'NewColumn' call for the season radio button list column; put our DoRandomStartingSeasonButton call just before
                    if (instruction.opcode == OpCodes.Ldstr && (string)instruction.operand == "MapStartSeasonDefault")
                    {
                        #if DEBUG
                            Log.Message("Dialog_AdvancedGameConfig.DoWindowContents match 1 of 1");
                        #endif

                        var clone = instruction.Clone();
                        instruction.opcode = OpCodes.Call; 
                        instruction.operand = AccessTools.Method(typeof(DoWindowContents), nameof(DoRandomStartingSeasonButton));
                        yield return instruction; // DoRandomStartingSeasonButton(listing_Standard)
                        yield return new CodeInstruction(OpCodes.Ldloc_0); // listing_Standard
                        instruction = clone; // "MapStartSeasonDefault"
                    }

                    yield return instruction;
                }
            }

            public static void DoRandomStartingSeasonButton(Listing_Standard listing)
            {
                // Pick a season, any season
                if (TinyTweaksSettings.randomStartingSeason)
                {
                    if (listing.ButtonText("Randomize".Translate()))
                        Find.GameInitData.startingSeason = (Season)Rand.RangeInclusive((int)Season.Spring, (int)Season.Winter);
                    listing.Gap(6);
                }
            }

        }

    }

}

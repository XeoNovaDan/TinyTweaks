﻿using System;
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

    public static class Patch_Dialog_AdvancedGameConfig
    {

        [HarmonyPatch(typeof(Dialog_AdvancedGameConfig))]
        [HarmonyPatch(nameof(Dialog_AdvancedGameConfig.DoWindowContents))]
        public static class DoWindowContents
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Look for the 'NewColumn' call for the season radio button list column; put our DoRandomStartingSeasonButton call just before
                    if (instruction.opcode == OpCodes.Ldstr && (string)instruction.operand == "MapStartSeasonDefault")
                    {
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

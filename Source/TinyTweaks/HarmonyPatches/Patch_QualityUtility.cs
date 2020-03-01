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

    public static class Patch_QualityUtility
    {

        [HarmonyPatch(typeof(QualityUtility), nameof(QualityUtility.GenerateQualityCreatedByPawn), new Type[] { typeof(int), typeof(bool) })]
        public static class GenerateQualityCreatedByPawn
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();
                bool done = false;

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Look three instructions ahead
                    if (!done && i < instructionList.Count - 3)
                    {
                        var thirdInstructionAhead = instructionList[i + 3];

                        // Looking for 'int num2 = (int)Rand.GaussianAsymmetric(num, 0.6f, 0.8f);'
                        if (thirdInstructionAhead.opcode == OpCodes.Call && thirdInstructionAhead.OperandIs(AccessTools.Method(typeof(Rand), nameof(Rand.GaussianAsymmetric))))
                        {
                            // When found, add some IL before that which modifies 'num' (the central quality level in numeric form)
                            yield return instruction; // num
                            yield return new CodeInstruction(OpCodes.Ldarg_0); // relevantSkillLevel
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GenerateQualityCreatedByPawn), nameof(ChangeBaseQuality))); // ChangeBaseQuality(num, relevantSkillLevel)
                            yield return new CodeInstruction(OpCodes.Stloc_0); // num = ChangeBaseQuality(num, relevantSkillLevel)
                            instruction = new CodeInstruction(instruction.opcode, instruction.operand);
                            done = true;
                        }
                    }

                    yield return instruction;
                }
            }

            public static float ChangeBaseQuality(float originalQuality, int relevantSkillLevel)
            {
                if (TinyTweaksSettings.changeQualityDistribution)
                {
                    // It's messy but designed to be as non-destructive as possible
                    switch (relevantSkillLevel)
                    {
                        case 0:
                            return 0;
                        case 1:
                            return 0.33f;
                        case 2:
                            return 0.67f;
                        case 3:
                            return 1;
                        case 4:
                            return 1.33f;
                        case 5:
                            return 1.67f;
                        case 6:
                            return 2;
                        case 7:
                            return 2.25f;
                        case 8:
                            return 2.5f;
                        case 9:
                            return 2.75f;
                        case 10:
                            return 3;
                        case 11:
                            return 3.25f;
                        case 12:
                            return 3.5f;
                        case 13:
                            return 3.75f;
                        case 14:
                            return 4;
                        case 15:
                            return 4.15f;
                        case 16:
                            return 4.3f; 
                        case 17:
                            return 4.45f;
                        case 18:
                            return 4.6f;
                        case 19:
                            return 4.75f;
                        case 20:
                            return 5f;
                    }
                }
                return originalQuality;
            }

        }

    }

}

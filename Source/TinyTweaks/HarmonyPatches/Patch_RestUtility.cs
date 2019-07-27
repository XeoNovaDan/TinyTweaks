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

    public static class Patch_RestUtility
    {

        public static class ManualPatch_FindPatientBedFor_medBedValidator
        {

            static ManualPatch_FindPatientBedFor_medBedValidator()
            {
                MedBedValidator = typeof(RestUtility).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).
                    First(t => t.Name.Contains("FindPatientBedFor") && t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Any(f => f.Name == "medBedValidator"));
            }

            public static Type MedBedValidator;

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();
                var pawnInfo = AccessTools.Field(MedBedValidator, "pawn");
                var modifyResultInfo = AccessTools.Method(typeof(ManualPatch_FindPatientBedFor_medBedValidator), nameof(ModifyResult));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Add a call to our ModifyResult method before each return operation
                    if (instruction.opcode == OpCodes.Ret)
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        yield return new CodeInstruction(OpCodes.Ldfld, pawnInfo); // this.pawn
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // t
                        yield return new CodeInstruction(OpCodes.Call, modifyResultInfo); // ModifyResult(result, this.pawn, t)
                    }

                    yield return instruction;
                }
            }

            public static bool ModifyResult(bool originalResult, Pawn pawn, Thing t)
            {
                if (TinyTweaksSettings.smarterMedicalBedSelection && originalResult)
                {
                    // If the player has any pawns queued up for surgery who aren't resting
                    int pawnsDueForSurgeryCount = pawn.Map.mapPawns.AllPawnsSpawned.Count(p => p.BillStack.AnyShouldDoNow);
                    if (pawnsDueForSurgeryCount > 0)
                    {
                        var freeBeds = pawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>();
                        int bedCount = freeBeds.Count();
                        if (pawnsDueForSurgeryCount < bedCount)
                        {
                            freeBeds = freeBeds.OrderByDescending(b => b.GetStatValue(StatDefOf.SurgerySuccessChanceFactor));
                            return (pawn.BillStack.AnyShouldDoNow ? freeBeds.Skip(bedCount - pawnsDueForSurgeryCount) : freeBeds.Take(bedCount)).Any(b => b == t);
                        }
                    }
                }
                return originalResult;
            }

        }

    }

}

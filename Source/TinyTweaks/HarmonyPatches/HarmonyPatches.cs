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
            TinyTweaks.HarmonyInstance.PatchAll();

            // Pawn_PlayerSettings ctor
            TinyTweaks.HarmonyInstance.Patch(typeof(Pawn_PlayerSettings).GetConstructor(new Type[] { typeof(Pawn) }), postfix: new HarmonyMethod(typeof(Patch_Pawn_PlayerSettings.ManualPatch_Ctor), "Postfix"));

            // RestUtility.FindPatientBedFor.medBedValidator
            TinyTweaks.HarmonyInstance.Patch(Patch_RestUtility.ManualPatch_FindPatientBedFor_medBedValidator.MedBedValidator.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m => m.ReturnType == typeof(bool)),
                transpiler: new HarmonyMethod(typeof(Patch_RestUtility.ManualPatch_FindPatientBedFor_medBedValidator), "Transpiler"));
        }

    }

}

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
        }

    }

}

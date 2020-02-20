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

    public static class Patch_OverlayDrawHandler
    {

        [HarmonyPatch(typeof(OverlayDrawHandler))]
        [HarmonyPatch(nameof(OverlayDrawHandler.ShouldDrawPowerGrid), MethodType.Getter)]
        public static class ShouldDrawPowerGrid_Getter
        {

            public static void Postfix(ref bool __result)
            {
                if (ExtraPlaySettings.drawPowerGrid)
                {
                    __result = true;
                }
            }

        }

    }

}

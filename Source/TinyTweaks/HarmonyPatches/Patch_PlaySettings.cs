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

    public static class Patch_PlaySettings
    {

        [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
        public static class AllRecipes_Getter
        {

            public static void Postfix(WidgetRow row, bool worldView)
            {
                if (!worldView)
                {
                    // Draw power grid
                    row.ToggleableIcon(ref ExtraPlaySettings.drawPowerGrid, ThingDefOf.PowerConduit.uiIcon, "TinyTweaks.PowerGridButton".Translate());
                }
            }

        }

        [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.ExposeData))]
        public static class ExposeData
        {

            public static void Postfix()
            {
                Scribe_Values.Look(ref ExtraPlaySettings.drawPowerGrid, "TT_drawPowerGrid");
            }

        }

    }

}

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
        public static class Patch_SetFromPreset
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

    }

}

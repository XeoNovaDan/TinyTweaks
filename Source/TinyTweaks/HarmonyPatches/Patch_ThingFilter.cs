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

    public static class Patch_ThingFilter
    {

        //[HarmonyPatch(typeof(ThingFilter), nameof(ThingFilter.SetFromPreset))]
        public static class SetFromPreset
        {

            public static void Postfix(ThingFilter __instance, StorageSettingsPreset preset)
            {
                if (preset == StorageSettingsPreset.DefaultStockpile)
                {

                }

                else
                {

                }
                
            }

        }

    }

}

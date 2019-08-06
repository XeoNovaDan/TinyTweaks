using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorld.Planet;
using Harmony;

namespace TinyTweaks
{

    public static class Patch_WITab_Caravan_Items
    {

        [HarmonyPatch(typeof(WITab_Caravan_Items))]
        [HarmonyPatch("FillTab")]
        public static class FillTab
        {

            public static void Postfix(WITab_Caravan_Items __instance, Vector2 ___size)
            {
                if (TinyTweaksSettings.caravanFoodRestrictions)
                {
                    var tabRect = new Rect(Vector2.zero, ___size);

                    // Add a button to assign food restrictions
                    if (Widgets.ButtonText(new Rect(tabRect.x + 220, tabRect.y + 10, 200, 27), "TinyTweaks.AssignFoodRestrictions".Translate()))
                    {
                        Find.WindowStack.Add(new Dialog_AssignCaravanFoodRestrictions(NonPublicProperties.WITab_get_SelCaravan(__instance)));
                    }
                }
            }

        }

    }

}

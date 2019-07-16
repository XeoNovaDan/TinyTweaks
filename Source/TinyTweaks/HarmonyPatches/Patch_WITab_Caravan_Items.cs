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
        public static class Patch_FillTab
        {

            public static void Postfix(WITab_Caravan_Items __instance, Vector2 ___size)
            {
                var tabRect = new Rect(Vector2.zero, ___size);

                // Add a button to assign food policies
                if (Widgets.ButtonText(new Rect(tabRect.x + 220, tabRect.y + 10, 200, 27), "TinyTweaks.AssignFoodRestrictions".Translate()))
                {
                    var selCaravan = (Caravan)__instance.GetType().GetProperty("SelCaravan", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(true).Invoke(__instance, null);
                    Find.WindowStack.Add(new Dialog_AssignCaravanFoodRestrictions(selCaravan));
                }
            }

        }

    }

}

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

    public static class Patch_Dialog_DebugActionsMenu
    {

        [HarmonyPatch(typeof(Dialog_DebugActionsMenu), "DoListingItems_MapTools")]
        public static class DoListingItems_MapTools
        {

            public static void Postfix(Dialog_DebugActionsMenu __instance)
            {
                NonPublicMethods.Dialog_DebugOptionLister_DoGap(__instance);
                NonPublicMethods.Dialog_DebugOptionLister_DoLabel(__instance, "Tiny Tweaks debug options");

                #region 1 vs. 1 testing
                NonPublicMethods.Dialog_DebugOptionLister_DebugAction(__instance, "1 vs. 1 testing", () =>
                {
                    Log.Message("Hello world");
                });
                #endregion
            }

        }

    }

}

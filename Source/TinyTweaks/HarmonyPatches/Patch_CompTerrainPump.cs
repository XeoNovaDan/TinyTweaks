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

    public static class Patch_CompTerrainPump
    {

        [HarmonyPatch(typeof(CompTerrainPump))]
        [HarmonyPatch(nameof(CompTerrainPump.CompTickRare))]
        public static class Patch_CompTickRare
        {

            public static void Prefix(CompTerrainPump __instance, int ___progressTicks, ref bool __state)
            {
                // Pass __state as true if the terrain pump is a moisture pump and isn't yet finished
                if (TinyTweaksSettings.autoRemoveMoisturePumps && __instance is CompTerrainPumpDry)
                {
                    float progressDays = ___progressTicks / GenDate.TicksPerDay;
                    float progressPercentage = progressDays / ((CompProperties_TerrainPump)__instance.props).daysToRadius;
                    __state = progressPercentage < 1;
                }
                else
                    __state = false;
            }

            public static void Postfix(CompTerrainPump __instance, int ___progressTicks, ref bool __state)
            {
                // If __state is true and the moisture pump has since finished, auto-designate an appropriate removal on it and send a message
                if (__state)
                {
                    float progressDays = ___progressTicks / GenDate.TicksPerDay;
                    float progressPercentage = progressDays / ((CompProperties_TerrainPump)__instance.props).daysToRadius;
                    if (progressPercentage >= 1)
                    {
                        var parent = __instance.parent;
                        var parentMap = __instance.parent.Map;
                        string messageText = "TinyTweaks.TerrainPumpDryFinished".Translate(parent.Label);
                        if (TinyTweaksSettings.autoRemoveMoisturePumps)
                        {
                            if (parent.def.Minifiable)
                            {
                                parentMap.designationManager.AddDesignation(new Designation(parent, DesignationDefOf.Uninstall));
                                messageText += $" {"TinyTweaks.TerrainPumpDryFinished_Uninstall".Translate()}";
                            }
                            else
                            {
                                parentMap.designationManager.AddDesignation(new Designation(parent, DesignationDefOf.Deconstruct));
                                messageText += $" {"TinyTweaks.TerrainPumpDryFinished_Deconstruct".Translate()}";
                            }
                        }
                        Messages.Message(messageText, parent, MessageTypeDefOf.TaskCompletion, false);
                    }
                }
            }

        }

    }

}

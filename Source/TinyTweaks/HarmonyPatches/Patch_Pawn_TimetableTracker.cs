using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace TinyTweaks
{

    public static class Patch_Pawn_TimetableTracker
    {

        [HarmonyPatch(typeof(Pawn_TimetableTracker), MethodType.Constructor, new Type[] { typeof(Pawn) })]
        public static class AddAndRemoveDynamicComponents
        {

            public static void Postfix(Pawn_TimetableTracker __instance, Pawn pawn)
            {
                // AutoOwl functionality
                if (TinyTweaksSettings.autoOwl && pawn.story?.traits?.HasTrait(TraitDefOf.NightOwl) == true)
                {
                    for (int i = 0; i < GenDate.HoursPerDay; i++)
                    {
                        if (i >= 11 && i <= 18)
                            __instance.times[i] = TimeAssignmentDefOf.Sleep;
                        else
                            __instance.times[i] = TimeAssignmentDefOf.Anything;
                    }
                }
            }

        }

    }

}

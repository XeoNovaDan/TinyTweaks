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

    public static class Patch_PawnGenerator
    {

        [HarmonyPatch(typeof(PawnGenerator), "GenerateNewPawnInternal")]
        public static class GenerateNewPawnInternal
        {

            public static void Postfix(Pawn __result)
            {
                // AutoOwl functionality
                if (TinyTweaksSettings.autoOwl && __result != null && __result.timetable is Pawn_TimetableTracker timetable && __result.story?.traits?.HasTrait(TraitDefOf.NightOwl) == true)
                {
                    for (int i = 0; i < GenDate.HoursPerDay; i++)
                    {
                        if (i >= 11 && i <= 18)
                            timetable.times[i] = TimeAssignmentDefOf.Sleep;
                        else
                            timetable.times[i] = TimeAssignmentDefOf.Anything;
                    }
                }
            }

        }

    }

}

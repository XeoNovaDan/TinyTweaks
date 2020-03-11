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

    public static class Patch_SkillRecord
    {

        [HarmonyPatch(typeof(SkillRecord), nameof(SkillRecord.Interval))]
        public static class Interval
        {

            public static bool Prefix(SkillRecord __instance, Pawn ___pawn)
            {
                // Delay skill decay
                if (TinyTweaksSettings.delayedSkillDecay && !___pawn.GetComp<CompSkillRecordCache>().CanDecaySkill(__instance.def))
                    return false;
                return true;
            }

        }

        [HarmonyPatch(typeof(SkillRecord), nameof(SkillRecord.Learn))]
        public static class Learn
        {

            public static void Postfix(SkillRecord __instance, Pawn ___pawn, float xp)
            {
                // Update the pawn's CompSkillTrackerCache
                if (xp >= CompSkillRecordCache.MinExpToDelaySkillDecay || (xp > 0 && __instance.xpSinceMidnight >= CompSkillRecordCache.MinExpToDelaySkillDecay))
                {
                    var skillRecordCache = ___pawn.GetComp<CompSkillRecordCache>();
                    if (skillRecordCache != null)
                        skillRecordCache.NotifySubstantialExperienceGainedFor(__instance.def);
                    else
                        Log.Warning($"{___pawn} has null CompSkillRecordCache (def={___pawn.def.defName})");
                }
            }

        }

    }

}

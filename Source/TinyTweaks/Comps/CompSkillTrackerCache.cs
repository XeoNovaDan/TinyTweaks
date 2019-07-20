using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{

    public class CompSkillRecordCache : ThingComp
    {
        public const int MinExpToDelaySkillDecay = SkillRecord.MaxFullRateXpPerDay / 2;

        private const int MinTicksSinceSkillGainForSkillDecay = GenDate.TicksPerHour * 8;

        public Dictionary<SkillDef, int> lastExperienceGainTickForSkillsCache = new Dictionary<SkillDef, int>();

        public override void CompTick()
        {
            // Because for some reason, doing this either in Initialize or in the field directly still leaves it as null :V
            if (lastExperienceGainTickForSkillsCache == null)
                lastExperienceGainTickForSkillsCache = new Dictionary<SkillDef, int>();
        }

        public void NotifySubstantialExperienceGainedFor(SkillDef skillDef)
        {
            if (lastExperienceGainTickForSkillsCache.ContainsKey(skillDef))
                lastExperienceGainTickForSkillsCache[skillDef] = Find.TickManager.TicksGame;

            else
                lastExperienceGainTickForSkillsCache.Add(skillDef, Find.TickManager.TicksGame);
        }

        public bool CanDecaySkill(SkillDef skillDef) => !lastExperienceGainTickForSkillsCache.ContainsKey(skillDef) || Find.TickManager.TicksGame > lastExperienceGainTickForSkillsCache[skillDef] + MinTicksSinceSkillGainForSkillDecay;


        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look(ref lastExperienceGainTickForSkillsCache, "lastExperienceGainTickForSkillsCache", LookMode.Def, LookMode.Value);
        }

    }

}

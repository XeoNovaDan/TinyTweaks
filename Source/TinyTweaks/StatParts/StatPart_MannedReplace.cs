using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{

    public class StatPart_MannedReplace : StatPart
    {

        private static bool ActiveNow => !ModCompatibilityCheck.TurretExtensions || TinyTweaksSettings.overrideMannedTurretFunctionality;

        public override string ExplanationPart(StatRequest req)
        {
            if (ActiveNow && req.HasThing && req.Thing.TryGetComp<CompMannable>() is CompMannable mannableComp)
            {
                if (mannableComp.MannedNow)
                    return $"{mannableComp.ManningPawn.LabelShort}: {mannableComp.ManningPawn.GetStatValue(manningPawnStat).ToStringByStyle(manningPawnStat.toStringStyle)}";
                else if (zeroIfUnmanned)
                    return $"{"TinyTweaks.Unmanned".Translate()}: {0f.ToStringByStyle(parentStat.toStringStyle)}";
            }
            return null;
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            // Use the manning pawn's stat if manned, otherwise 
            if (ActiveNow && req.HasThing && req.Thing.TryGetComp<CompMannable>() is CompMannable mannableComp)
            {
                val = mannableComp.MannedNow ? mannableComp.ManningPawn.GetStatValue(manningPawnStat) : (zeroIfUnmanned ? 0 : val);
            }
        }

        private StatDef manningPawnStat;
        private bool zeroIfUnmanned;

    }

}

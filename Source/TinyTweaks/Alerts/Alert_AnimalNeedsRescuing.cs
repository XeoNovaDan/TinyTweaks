using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{
    public class Alert_AnimalNeedsRescuing : Alert_Critical
    {

        private List<Pawn> AnimalsNeedingRescue
        {
            get
            {
                var pawns = PawnsFinder.AllMaps_Spawned;
                var result = new List<Pawn>();
                for (int i = 0; i < pawns.Count; i++)
                {
                    var p = pawns[i];
                    if (p.PlayerColonyAnimal() && Alert_ColonistNeedsRescuing.NeedsRescue(p))
                        result.Add(p);
                }
                return result;
            }
        }

        public override string GetLabel()
        {
            if (AnimalsNeedingRescue.Count < 1)
                return "TinyTweaks.AnimalNeedsRescue".Translate();
            return "TinyTweaks.AnimalsNeedRescue".Translate();
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var sortedAnimals = TinyTweaksUtility.SortedAnimalList(AnimalsNeedingRescue);
            for (int i = 0; i < sortedAnimals.Count; i++)
            {
                var pawn = sortedAnimals[i];
                var listEntry = pawn.NameShortColored.CapitalizeFirst();
                if (pawn.HasBondRelation())
                    listEntry += $" {"BondBrackets".Translate()}".Colorize(ColoredText.NameColor);
                stringBuilder.AppendLine("  - " + listEntry.Resolve());
            }
            return string.Format("TinyTweaks.AnimalsNeedRescue_Desc".Translate(), stringBuilder.ToString());
        }

        public override AlertReport GetReport()
        {
            if (!TinyTweaksSettings.animalMedicalAlerts)
                return false;
            return AlertReport.CulpritsAre(AnimalsNeedingRescue);
        }

    }
}

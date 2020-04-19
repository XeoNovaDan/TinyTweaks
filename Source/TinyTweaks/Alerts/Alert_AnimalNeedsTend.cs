using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{
    public class Alert_AnimalNeedsTend : Alert
    {

        public Alert_AnimalNeedsTend()
        {
            defaultLabel = "TinyTweaks.AnimalNeedsTreatment".Translate();
            defaultPriority = AlertPriority.High;
        }

        private List<Pawn> NeedingAnimals
        {
            get
            {
                var pawns = PawnsFinder.AllMaps_Spawned;
                var result = new List<Pawn>();
                for (int i = 0; i < pawns.Count; i++)
                {
                    var p = pawns[i];
                    if (p.PlayerColonyAnimal() && p.health.HasHediffsNeedingTendByPlayer(true))
                    {
                        Building_Bed curBed = p.CurrentBed();
                        if (curBed == null || TinyTweaksSettings.medBedMedicalAlert || !curBed.Medical)
                            if (!Alert_ColonistNeedsRescuing.NeedsRescue(p))
                                result.Add(p);
                    }
                }
                return result;
            }
        }

        public override string GetLabel()
        {
            if (NeedingAnimals.Count <= 1)
                return "TinyTweaks.AnimalNeedsTreatment".Translate();
            return "TinyTweaks.AnimalsNeedTreatment".Translate();
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var sortedAnimals = TinyTweaksUtility.SortedAnimalList(NeedingAnimals);
            for (int i = 0; i < sortedAnimals.Count; i++)
            {
                var pawn = sortedAnimals[i];
                var listEntry = pawn.NameShortColored.CapitalizeFirst();
                if (pawn.HasBondRelation())
                    listEntry += $" {"BondBrackets".Translate()}".Colorize(ColoredText.NameColor);
                stringBuilder.AppendLine("  - " + listEntry.Resolve());
            }
            return string.Format("TinyTweaks.AnimalNeedsTreatment_Desc".Translate(), stringBuilder.ToString());
        }

        public override AlertReport GetReport()
        {
            if (!TinyTweaksSettings.animalMedicalAlerts)
                return false;
            return AlertReport.CulpritsAre(NeedingAnimals);
        }

    }
}

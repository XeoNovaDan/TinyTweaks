using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{
    public class Alert_LifeThreateningHediffAnimal : Alert_Critical
    {

        private List<Pawn> SickAnimals
        {
            get
            {
                var pawns = PawnsFinder.AllMaps_Spawned;
                var result = new List<Pawn>();
                for (int i = 0; i < pawns.Count; i++)
                {
                    var p = pawns[i];
                    if (p.PlayerColonyAnimal_Alive_NoCryptosleep())
                    {
                        for (int j = 0; j < p.health.hediffSet.hediffs.Count; j++)
                        {
                            Hediff diff = p.health.hediffSet.hediffs[j];
                            if (diff.CurStage != null && diff.CurStage.lifeThreatening && !diff.FullyImmune())
                            {
                                result.Add(p);
                                break;
                            }
                        }

                    }
                }
                return result;
            }
        }

        public override string GetLabel()
        {
            return "TinyTweaks.AnimalsWithLifeThreateningDisease".Translate();
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool amputatable = false;
            var sortedAnimals = TinyTweaksUtility.SortedAnimalList(SickAnimals);
            for (int i = 0; i < sortedAnimals.Count; i++)
            {
                var pawn = sortedAnimals[i];
                var listEntry = pawn.NameShortColored.CapitalizeFirst();
                if (pawn.HasBondRelation())
                    listEntry += $" {"BondBrackets".Translate()}".Colorize(ColoredText.NameColor);
                stringBuilder.AppendLine("  - " + listEntry.Resolve());
                var hediffs = pawn.health.hediffSet.hediffs;
                for (int j = 0; j < hediffs.Count; j++)
                {
                    var hediff = hediffs[j];
                    if (hediff.CurStage != null && hediff.CurStage.lifeThreatening && hediff.Part != null && hediff.Part != pawn.RaceProps.body.corePart)
                    {
                        amputatable = true;
                        break;
                    }
                }
            }
            if (amputatable)
                return string.Format("TinyTweaks.AnimalsWithLifeThreateningDiseaseAmputation_Desc".Translate(), stringBuilder.ToString());
            return string.Format("TinyTweaks.AnimalsWithLifeThreateningDisease_Desc".Translate(), stringBuilder.ToString());
        }

        public override AlertReport GetReport()
        {
            if (!TinyTweaksSettings.animalMedicalAlerts)
                return false;
            return AlertReport.CulpritsAre(SickAnimals);
        }

    }
}

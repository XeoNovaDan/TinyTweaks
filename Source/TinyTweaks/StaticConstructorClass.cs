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

    [StaticConstructorOnStartup]
    public static class StaticConstructorClass
    {

        static StaticConstructorClass()
        {
            if (TinyTweaksSettings.changeDefLabels)
                ChangeDefLabels();

            if (TinyTweaksSettings.changeBuildableDefDesignationCategories)
                UpdateDesignationCategories();

            // Patch defs
            PatchThingDefs();
        }

        private static void PatchThingDefs()
        {
            var allThingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
            for (int i = 0; i < allThingDefs.Count; i++)
            {
                var tDef = allThingDefs[i];

                // If the def has CompLaunchable, add CompLaunchableAutoRebuild to it
                if (tDef.HasComp(typeof(CompLaunchable)))
                    tDef.AddComp(typeof(CompLaunchableAutoRebuild));

                // If the def has RaceProps and RaceProps are humanlike, add CompSkillTrackerCache to it
                if (tDef.race != null && tDef.race.Humanlike)
                    tDef.AddComp(typeof(CompSkillRecordCache));

                // If the def is a turret but not a mortar, add CompSmarterTurretTargeting to it
                if (tDef.IsBuildingArtificial && tDef.building.IsTurret && !tDef.building.IsMortar)
                    tDef.AddComp(typeof(CompSmarterTurretTargeting));
            }
        }

        private static void UpdateDesignationCategories()
        {
            // Change the DesignationCategoryDefs of appropriate defs
            ChangeDesignationCategories();

            // Update all appropriate categories
            if (CategoriesToRemove.Any())
            {
                var defDatabaseRemove = typeof(DefDatabase<DesignationCategoryDef>).GetMethod("Remove", BindingFlags.NonPublic | BindingFlags.Static);
                foreach (var dcDef in CategoriesToRemove)
                    defDatabaseRemove.Invoke(null, new object[] { dcDef });
            }

            foreach (var dcDef in DefDatabase<DesignationCategoryDef>.AllDefs)
                dcDef.ResolveReferences();
            
        }

        private static void ChangeDesignationCategories()
        {
            // This method only exists in the case that other modders want their BuildableDefs to be changed and they decide to do so via harmony
            foreach (var thDef in DefDatabase<ThingDef>.AllDefs)
                ChangeDesignationCategory(thDef);
            foreach (var trDef in DefDatabase<TerrainDef>.AllDefs)
                ChangeDesignationCategory(trDef);
        }

        private static void ChangeDesignationCategory(BuildableDef bDef)
        {
            if (bDef.designationCategory == null)
                return;

            var mod = bDef.modContentPack;

            // Furniture+ => Furniture
            if (DesignationCategoryDefOf.ANON2MF != null && bDef.designationCategory == DesignationCategoryDefOf.ANON2MF)
                bDef.designationCategory = DesignationCategoryDefOf.Furniture;

            // More Floors => Floors
            else if (DesignationCategoryDefOf.MoreFloors != null && bDef.designationCategory == DesignationCategoryDefOf.MoreFloors)
                bDef.designationCategory = DesignationCategoryDefOf.Floors;

            // Dubs Bad Hygiene
            else if (mod.Name == "Dubs Bad Hygiene")
            {
                // Temperature stuff gets moved to Temperature category
                if (bDef.researchPrerequisites?.Any(r => r.defName == "CentralHeating" || r.defName == "PoweredHeating" || r.defName == "MultiSplitAirCon") ?? false)
                    bDef.designationCategory = DesignationCategoryDefOf.Temperature;

                // Rest gets moved from Hygiene/Misc => Hygiene
                else if (bDef.designationCategory == DesignationCategoryDefOf.HygieneMisc)
                        bDef.designationCategory = DesignationCategoryDefOf.Hygiene;
            }  

            // Furniture => Storage (Deep Storage)
            else if (mod.Name == "LWM's Deep Storage")
                bDef.designationCategory = DesignationCategoryDefOf.Storage;

            // Defenses => Security
            else if (DesignationCategoryDefOf.DefensesExpanded_CustomCategory != null && bDef.designationCategory == DesignationCategoryDefOf.DefensesExpanded_CustomCategory)
                bDef.designationCategory = RimWorld.DesignationCategoryDefOf.Security;
        }

        private static IEnumerable<DesignationCategoryDef> CategoriesToRemove
        {
            get
            {
                if (DesignationCategoryDefOf.ANON2MF != null)
                    yield return DesignationCategoryDefOf.ANON2MF;
                if (DesignationCategoryDefOf.MoreFloors != null)
                    yield return DesignationCategoryDefOf.MoreFloors;
                if (DesignationCategoryDefOf.HygieneMisc != null)
                    yield return DesignationCategoryDefOf.HygieneMisc;
                if (DesignationCategoryDefOf.DefensesExpanded_CustomCategory != null)
                    yield return DesignationCategoryDefOf.DefensesExpanded_CustomCategory;
            }
        }

        private static void ChangeDefLabels()
        {
            // Go through every appropriate def that has a label
            var changeableDefTypes = GenDefDatabase.AllDefTypesWithDatabases().Where(t => ShouldChangeDefTypeLabel(t)).ToList();
            for (int i = 0; i < changeableDefTypes.Count; i++)
            {
                var curDefs = GenDefDatabase.GetAllDefsInDatabaseForDef(changeableDefTypes[i]).ToList();
                for (int j = 0; j < curDefs.Count; j++)
                {
                    var curDef = curDefs[j];
                    if (!curDef.label.NullOrEmpty())
                    {
                        // Update the def's label
                        AdjustLabel(ref curDef.label);

                        // If the def is a ThingDef...
                        if (curDef is ThingDef tDef)
                        {
                            // If the ThingDef is a stuff item
                            if (tDef.stuffProps is StuffProperties stuffProps)
                            {
                                // Update the stuff adjective if there is one
                                if (!stuffProps.stuffAdjective.NullOrEmpty())
                                    AdjustLabel(ref stuffProps.stuffAdjective);
                            }
                        }
                    }
                }
            }
        }

        public static bool ShouldChangeDefTypeLabel(Type defType)
        {
            return defType != typeof(StorytellerDef) && defType != typeof(ResearchProjectDef) && defType != typeof(ResearchTabDef) && defType != typeof(ExpansionDef);
        }

        private static void AdjustLabel(ref string label)
        {
            // Split the label up by spaces
            string[] splitLabel = label.Split(' ');

            // Process each word within the label
            for (int i = 0; i < splitLabel.Count(); i++)
            {
                // If the word contains hyphens, split at the hyphens and process each word
                if (splitLabel[i].Contains('-'))
                {
                    string[] labelPartSplit = splitLabel[i].Split('-');
                    for (int j = 0; j < labelPartSplit.Count(); j++)
                        AdjustLabelPart(ref labelPartSplit[j], true);
                    splitLabel[i] = String.Join("-", labelPartSplit);
                }

                // Otherwise adjust as a whole
                else
                    AdjustLabelPart(ref splitLabel[i], false);
            }

            // Update the label
            label = String.Join(" ", splitLabel);
        }

        private static void AdjustLabelPart(ref string labelPart, bool uncapitaliseSingleCharacters)
        {
            // If labelPart is only a single character, do nothing unless uncapitaliseSingleCharacters is true
            if (labelPart.Length == 1)
            {
                if (uncapitaliseSingleCharacters)
                    labelPart = labelPart.ToLower();
                return;
            }   

            // Split labelPart into its characters
            char[] labelPartChars = labelPart.ToCharArray();

            // Go through each character and if there are no more characters that aren't lower-cased letters, uncapitalise labelPart
            bool uncapitalise = true;
            for (int j = 1; j < labelPartChars.Count(); j++)
            {
                if (!Char.IsLower(labelPartChars[j]))
                {
                    uncapitalise = false;
                    break;
                }
            }

            if (uncapitalise)
                labelPart = labelPart.ToLower();
        }

    }

}

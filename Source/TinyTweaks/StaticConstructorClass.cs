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

            if (TinyTweaksSettings.condenseThingDefDesignationCategories)
                UpdateDesignationCategories();
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

            // Furniture+ => Furniture
            if (TT_DesignationCategoryDefOf.ANON2MF != null && bDef.designationCategory == TT_DesignationCategoryDefOf.ANON2MF)
                bDef.designationCategory = TT_DesignationCategoryDefOf.Furniture;

            // More Floors => Floors
            if (TT_DesignationCategoryDefOf.MoreFloors != null && bDef.designationCategory == TT_DesignationCategoryDefOf.MoreFloors)
                bDef.designationCategory = TT_DesignationCategoryDefOf.Floors;

            // Hygiene/Misc => Hygiene
            if (TT_DesignationCategoryDefOf.HygieneMisc != null && bDef.designationCategory == TT_DesignationCategoryDefOf.HygieneMisc)
                bDef.designationCategory = TT_DesignationCategoryDefOf.Hygiene;
        }

        private static IEnumerable<DesignationCategoryDef> CategoriesToRemove
        {
            get
            {
                if (TT_DesignationCategoryDefOf.ANON2MF != null)
                    yield return TT_DesignationCategoryDefOf.ANON2MF;
                if (TT_DesignationCategoryDefOf.MoreFloors != null)
                    yield return TT_DesignationCategoryDefOf.MoreFloors;
                if (TT_DesignationCategoryDefOf.HygieneMisc != null)
                    yield return TT_DesignationCategoryDefOf.HygieneMisc;
            }
        }

        private static void ChangeDefLabels()
        {
            // Go through every appropriate def that has a label
            foreach (var defType in GenDefDatabase.AllDefTypesWithDatabases().Where(t => ShouldChangeDefTypeLabel(t)))
            {
                foreach (var def in GenDefDatabase.GetAllDefsInDatabaseForDef(defType).Where(d => !d.label.NullOrEmpty()))
                {
                    // Update the def's label
                    AdjustLabel(ref def.label);

                    // If the def is a ThingDef...
                    if (def is ThingDef tDef)
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

        public static bool ShouldChangeDefTypeLabel(Type defType)
        {
            return defType != typeof(StorytellerDef) && defType != typeof(ResearchProjectDef) && defType != typeof(ResearchTabDef);
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

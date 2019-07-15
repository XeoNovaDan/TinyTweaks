using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{

    public class TinyTweaksSettings : ModSettings
    {

        public static bool changeDefLabels = true;
        public static bool condenseThingDefDesignationCategories = true;

        public static bool renameLampTest = true;

        public void DoWindowContents(Rect wrect)
        {
            var options = new Listing_Standard();
            var defaultColor = GUI.color;
            options.Begin(wrect);
            GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            // Change def labels
            options.Gap();
            options.CheckboxLabeled("TinyTweaks.ChangeDefLabels".Translate(), ref changeDefLabels, "TinyTweaks.ChangeDefLabels_ToolTip".Translate());

            // Condense ThingDef designation categories
            options.Gap();
            options.CheckboxLabeled("TinyTweaks.CondenseThingDefDesignationCategories".Translate(), ref condenseThingDefDesignationCategories, "TinyTweaks.CondenseThingDefDesignationCategories_ToolTip".Translate());

            // Finish
            options.End();
            Mod.GetSettings<TinyTweaksSettings>().Write();

        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref changeDefLabels, "changeDefLabels", true);
            Scribe_Values.Look(ref condenseThingDefDesignationCategories, "condenseThingDefDesignationCategories", true);
        }

    }

}

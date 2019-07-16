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

        private const float PageButtonSpacing = 250;
        private const float PageButtonWidth = 150;
        private const float PageButtonHeight = 35;

        //[TweakValue("aPageButtonXPosOffset", -100, 100)]
        private static float PageButtonXPosOffset = -75;

        private const int MaxPageIndex = 2;
        private static int _pageIndex = 1;
        private static int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = Mathf.Clamp(value, 1, MaxPageIndex);
        }

        #region Startup Settings
        public static bool changeDefLabels = true;
        public static bool changeBuildableDefDesignationCategories = true;

        public static bool tweakVanilla = false;
        public static bool tweakDubsBadHygiene = false;
        #endregion

        #region General Settings
        public static bool artilleryCopyPasteGizmos = true;
        public static bool alphabeticalBillList = true;
        public static bool autoRemoveMoisturePumps = true;

        public static bool changeQualityDistribution = false;
        #endregion

        public void DoWindowContents(Rect wrect)
        {
            var options = new Listing_Standard();
            var defaultColor = GUI.color;
            options.Begin(wrect);
            GUI.color = defaultColor;
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.UpperLeft;

            options.Gap();

            #region Startup Settings
            if (PageIndex == 1)
            {
                // Heading
                options.Label("TinyTweaks.StartupSettingsHeading".Translate());
                Text.Font = GameFont.Small;
                options.Label("TinyTweaks.StartupSettingsNote".Translate());
                options.GapLine(24);

                // Change architect menu tabs
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.ChangeBuildableDefDesignationCategories".Translate(), ref changeBuildableDefDesignationCategories, "TinyTweaks.ChangeBuildableDefDesignationCategories_ToolTip".Translate());

                // Consistent label casing
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.ChangeDefLabels".Translate(), ref changeDefLabels, "TinyTweaks.ChangeDefLabels_ToolTip".Translate());

                // Balance changes
                options.GapLine(24);
                options.Gap();
                options.Label("TinyTweaks.BalanceChanges".Translate());

                // Vanilla
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.TweakVanilla".Translate(), ref tweakVanilla, "TinyTweaks.TweakVanilla_ToolTip".Translate());

                // Dubs Bad Hygiene
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.TweakDubsBadHygiene".Translate(), ref tweakDubsBadHygiene, "TinyTweaks.TweakDubsBadHygiene_ToolTip".Translate());
            }
            #endregion

            #region General Settings
            if (PageIndex == 2)
            {
                // Heading
                options.Label("TinyTweaks.GeneralSettingsHeading".Translate());
                Text.Font = GameFont.Small;
                options.Label("TinyTweaks.GeneralSettingsNote".Translate());
                options.GapLine(24);

                // Automatically remove finished moisture pumps
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.AutoRemoveTerrainPumpDry".Translate(), ref autoRemoveMoisturePumps, "TinyTweaks.AutoRemoveTerrainPumpDry_ToolTip".Translate());

                // Copy/Paste buttons on artillery
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.ArtilleryCopyPasteGizmos".Translate(), ref artilleryCopyPasteGizmos, "TinyTweaks.ArtilleryCopyPasteGizmos_ToolTip".Translate());

                // Sort workbench bill list alphabetically
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.AlphabeticalBillList".Translate(), ref alphabeticalBillList, "TinyTweaks.AlphabeticalBillList_ToolTip".Translate());

                // Balance changes
                options.GapLine(24);
                options.Gap();
                options.Label("TinyTweaks.BalanceChanges".Translate());

                // Change quality distribution
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.ChangeQualityDistribution".Translate(), ref changeQualityDistribution, "TinyTweaks.ChangeQualityDistribution_ToolTip".Translate());
            }
            #endregion

            #region Page Buttons
            // This involved a crap ton of trial and error
            var leftButtonRect = new Rect((wrect.width / 2) - ((PageButtonSpacing / 2) - PageButtonXPosOffset), wrect.height - (PageButtonHeight + 20), PageButtonWidth, PageButtonHeight);
            if (Widgets.ButtonText(leftButtonRect, "TinyTweaks.PreviousPage".Translate()))
                PageIndex--;

            var rightButtonRect = new Rect((wrect.width / 2) + ((PageButtonSpacing / 2) + PageButtonXPosOffset), wrect.height - (PageButtonHeight + 20), PageButtonWidth, PageButtonHeight);
            if (Widgets.ButtonText(rightButtonRect, "TinyTweaks.NextPage".Translate()))
                PageIndex++;

            var pageNumberRect = new Rect((wrect.width / 2) - 15, wrect.height - (PageButtonHeight + 15), 120, PageButtonHeight);
            Widgets.Label(pageNumberRect, $"{PageIndex} / {MaxPageIndex}");
            #endregion

            // Finish
            options.End();
            Mod.GetSettings<TinyTweaksSettings>().Write();

        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref changeDefLabels, "changeDefLabels", true);
            Scribe_Values.Look(ref changeBuildableDefDesignationCategories, "changeBuildableDefDesignationCategories", true);
            Scribe_Values.Look(ref tweakVanilla, "tweakVanilla", false);
            Scribe_Values.Look(ref tweakDubsBadHygiene, "tweakDubsBadHygiene", false);

            Scribe_Values.Look(ref artilleryCopyPasteGizmos, "artilleryCopyPasteGizmos", true);
            Scribe_Values.Look(ref alphabeticalBillList, "alphabeticalBillList", true);
            Scribe_Values.Look(ref autoRemoveMoisturePumps, "autoRemoveMoisturePumps", true);
            Scribe_Values.Look(ref changeQualityDistribution, "changeQualityDistribution", false);
        }

    }

}

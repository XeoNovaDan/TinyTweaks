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

        private const float PageButtonWidth = 150;
        private const float PageButtonHeight = 35;
        private const float PageButtonPosOffsetFromCentre = 60;

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

        public static bool tweakVanilla = true;
        public static bool tweakDubsBadHygiene = true;
        #endregion

        #region General Settings
        public static bool alphabeticalBillList = true;
        public static bool autoRemoveMoisturePumps = true;

        public static bool changeQualityDistribution = true;
        public static bool bloodPumpingAffectsBleeding = true;
        public static bool delayedSkillDecay = true;
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

                // Sort workbench bill list alphabetically
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.AlphabeticalBillList".Translate(), ref alphabeticalBillList, "TinyTweaks.AlphabeticalBillList_ToolTip".Translate());

                // Balance changes
                options.GapLine(24);
                options.Gap();
                options.Label("TinyTweaks.BalanceChanges".Translate());

                // Blood pumping affects bleeding
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.BloodPumpingAffectsBleeding".Translate(), ref bloodPumpingAffectsBleeding, "TinyTweaks.BloodPumpingAffectsBleeding_ToolTip".Translate());

                // Change quality distribution
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.ChangeQualityDistribution".Translate(), ref changeQualityDistribution, "TinyTweaks.ChangeQualityDistribution_ToolTip".Translate());

                // Delayed skill decay
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.DelayedSkillDecay".Translate(), ref delayedSkillDecay, "TinyTweaks.DelayedSkillDecay_ToolTip".Translate());
            }
            #endregion

            #region Page Buttons
            float halfRectWidth = wrect.width / 2;
            float xOffset = (halfRectWidth - PageButtonWidth) / 2;
            var leftButtonRect = new Rect(xOffset + PageButtonPosOffsetFromCentre, wrect.height - PageButtonHeight, PageButtonWidth, PageButtonHeight);
            if (Widgets.ButtonText(leftButtonRect, "TinyTweaks.PreviousPage".Translate()))
                PageIndex--;

            var rightButtonRect = new Rect(halfRectWidth + xOffset - PageButtonPosOffsetFromCentre, wrect.height - PageButtonHeight, PageButtonWidth, PageButtonHeight); ;
            if (Widgets.ButtonText(rightButtonRect, "TinyTweaks.NextPage".Translate()))
                PageIndex++;

            Text.Anchor = TextAnchor.MiddleCenter;
            var pageNumberRect = new Rect(0, wrect.height - PageButtonHeight, wrect.width, PageButtonHeight);
            Widgets.Label(pageNumberRect, $"{PageIndex} / {MaxPageIndex}");
            Text.Anchor = TextAnchor.UpperLeft;
            #endregion

            // Finish
            options.End();
            Mod.GetSettings<TinyTweaksSettings>().Write();

        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref changeDefLabels, "changeDefLabels", true);
            Scribe_Values.Look(ref changeBuildableDefDesignationCategories, "changeBuildableDefDesignationCategories", true);
            Scribe_Values.Look(ref tweakVanilla, "tweakVanilla", true);
            Scribe_Values.Look(ref tweakDubsBadHygiene, "tweakDubsBadHygiene", true);

            Scribe_Values.Look(ref alphabeticalBillList, "alphabeticalBillList", true);
            Scribe_Values.Look(ref autoRemoveMoisturePumps, "autoRemoveMoisturePumps", true);
            Scribe_Values.Look(ref changeQualityDistribution, "changeQualityDistribution", true);
            Scribe_Values.Look(ref bloodPumpingAffectsBleeding, "bloodPumpingAffectsBleeding", true);
            Scribe_Values.Look(ref delayedSkillDecay, "delayedSkillDecay", true);
        }

    }

}

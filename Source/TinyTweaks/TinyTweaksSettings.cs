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

        private const int QoLPageIndex = 1;
        private const int BugFixPageIndex = 2;
        private const int BalancePageIndex = 3;

        private const int MaxPageIndex = 3;
        private static int _pageIndex = 1;
        private static int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = Mathf.Clamp(value, 1, MaxPageIndex);
        }

        #region QoL Changes
        public static bool autoAssignAnimalFollowSettings = true;
        public static bool autoRemoveMoisturePumps = true;
        public static bool changeDefLabels = true;

        // Restart
        public static bool alphabeticalBillList = true;
        public static bool changeBuildableDefDesignationCategories = true;
        #endregion

        #region Bug Fixes
        public static bool meleeArmourPenetrationFix = true;
        #endregion

        #region Balance Changes
        public static bool changeQualityDistribution = true;
        public static bool bloodPumpingAffectsBleeding = true;
        public static bool delayedSkillDecay = true;

        // Restart
        public static bool tweakVanilla = true;
        public static bool tweakDubsBadHygiene = true;
        #endregion

        private void DoHeading(Listing_Standard listing, GameFont font)
        {
            listing.Gap();
            string headingTranslationKey = "TinyTweaks.";
            switch(PageIndex)
            {
                case QoLPageIndex:
                    headingTranslationKey += "QualityOfLifeChangesHeading";
                    goto WriteHeader;
                case BugFixPageIndex:
                    headingTranslationKey += "BugFixesHeading";
                    goto WriteHeader;
                case BalancePageIndex:
                    headingTranslationKey += "BalanceChangesHeading";
                    goto WriteHeader;
            }
            WriteHeader:
            Text.Font = font + 1;
            listing.Label(headingTranslationKey.Translate());
            Text.Font = font;
            listing.GapLine(24);
        }

        private void GameRestartNotRequired(Listing_Standard listing)
        {
            listing.Gap();
            listing.Label("TinyTweaks.GameRestartNotRequired".Translate());
        }

        private void GameRestartRequired(Listing_Standard listing)
        {
            listing.Gap();
            listing.Label("TinyTweaks.GameRestartRequired".Translate());
        }

        public void DoWindowContents(Rect wrect)
        {
            var options = new Listing_Standard();
            var defaultColor = GUI.color;
            options.Begin(wrect);
            GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            DoHeading(options, Text.Font);

            #region QoL Changes
            if (PageIndex == QoLPageIndex)
            {
                // 'Game restart not required' note
                GameRestartNotRequired(options);

                // Automatically assign animals to follow their master
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.QoLChanges.AutoAssignAnimalFollowSettings".Translate(), ref autoAssignAnimalFollowSettings, "TinyTweaks.QoLChanges.AutoAssignAnimalFollowSettings_ToolTip".Translate());

                // Automatically remove finished moisture pumps
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.QoLChanges.AutoRemoveTerrainPumpDry".Translate(), ref autoRemoveMoisturePumps, "TinyTweaks.QoLChanges.AutoRemoveTerrainPumpDry_ToolTip".Translate());

                // Sort workbench bill list alphabetically
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.QoLChanges.AlphabeticalBillList".Translate(), ref alphabeticalBillList, "TinyTweaks.QoLChanges.AlphabeticalBillList_ToolTip".Translate());

                // 'Game restart required' note
                options.GapLine(24);
                GameRestartRequired(options);

                // Change architect menu tabs
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.QoLChanges.ChangeBuildableDefDesignationCategories".Translate(), ref changeBuildableDefDesignationCategories, "TinyTweaks.QoLChanges.ChangeBuildableDefDesignationCategories_ToolTip".Translate());

                // Consistent label casing
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.QoLChanges.ChangeDefLabels".Translate(), ref changeDefLabels, "TinyTweaks.QoLChanges.ChangeDefLabels_ToolTip".Translate());


            }
            #endregion

            #region Bug Fixes
            else if (PageIndex == BugFixPageIndex)
            {
                // 'Game restart not required' note
                GameRestartNotRequired(options);

                // Melee weapon AP fix
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.BugFixes.MeleeArmourPenetration".Translate(), ref meleeArmourPenetrationFix, "TinyTweaks.BugFixes.MeleeArmourPenetration_ToolTip".Translate());

            }
            #endregion

            #region Balance Changes
            else if (PageIndex == BalancePageIndex)
            {
                // 'Game restart not required' note
                GameRestartNotRequired(options);

                // Blood pumping affects bleeding
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.BalanceChanges.BloodPumpingAffectsBleeding".Translate(), ref bloodPumpingAffectsBleeding, "TinyTweaks.BalanceChanges.BloodPumpingAffectsBleeding_ToolTip".Translate());

                // Change quality distribution
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.BalanceChanges.ChangeQualityDistribution".Translate(), ref changeQualityDistribution, "TinyTweaks.BalanceChanges.ChangeQualityDistribution_ToolTip".Translate());

                // Delayed skill decay
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.BalanceChanges.DelayedSkillDecay".Translate(), ref delayedSkillDecay, "TinyTweaks.BalanceChanges.DelayedSkillDecay_ToolTip".Translate());

                // 'Game restart required' note
                options.GapLine(24);
                GameRestartRequired(options);

                // Vanilla
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.BalanceChanges.Vanilla".Translate(), ref tweakVanilla, "TinyTweaks.BalanceChanges.Vanilla_ToolTip".Translate());

                // Dubs Bad Hygiene
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.BalanceChanges.DubsBadHygiene".Translate(), ref tweakDubsBadHygiene, "TinyTweaks.BalanceChanges.DubsBadHygiene_ToolTip".Translate());
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
            #region QoL Changes
            Scribe_Values.Look(ref autoRemoveMoisturePumps, "autoRemoveMoisturePumps", true);
            Scribe_Values.Look(ref changeDefLabels, "changeDefLabels", true);

            // Restart
            Scribe_Values.Look(ref alphabeticalBillList, "alphabeticalBillList", true);
            Scribe_Values.Look(ref changeBuildableDefDesignationCategories, "changeBuildableDefDesignationCategories", true);
            #endregion

            #region Bug Fixes
            #endregion

            #region Balance Changes
            Scribe_Values.Look(ref changeQualityDistribution, "changeQualityDistribution", true);
            Scribe_Values.Look(ref bloodPumpingAffectsBleeding, "bloodPumpingAffectsBleeding", true);
            Scribe_Values.Look(ref delayedSkillDecay, "delayedSkillDecay", true);

            // Restart
            Scribe_Values.Look(ref tweakVanilla, "tweakVanilla", true);
            Scribe_Values.Look(ref tweakDubsBadHygiene, "tweakDubsBadHygiene", true);
            #endregion
        }

    }

}

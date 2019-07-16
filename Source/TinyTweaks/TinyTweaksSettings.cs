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
        public static bool tweakDubsBadHygiene = true;
        #endregion

        #region General Settings
        public static bool artilleryCopyPasteGizmos = true;
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

                // Change def labels
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.ChangeDefLabels".Translate(), ref changeDefLabels, "TinyTweaks.ChangeDefLabels_ToolTip".Translate());

                // Change DesignationCategoryDefs of some BuildableDefs
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.ChangeBuildableDefDesignationCategories".Translate(), ref changeBuildableDefDesignationCategories, "TinyTweaks.ChangeBuildableDefDesignationCategories_ToolTip".Translate());

                options.GapLine(24);
                // Tweak Dubs Bad Hygiene
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.TweakDubsBadHygiene".Translate(), ref tweakDubsBadHygiene);
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

                // Copy/Paste gizmos on artillery (e.g. mortars)
                options.Gap();
                options.CheckboxLabeled("TinyTweaks.ArtilleryCopyPasteGizmos".Translate(), ref artilleryCopyPasteGizmos, "TinyTweaks.ArtilleryCopyPasteGizmos_ToolTip".Translate());
            }
            #endregion

            #region Page Buttons
            // This involved a crap ton of trial and error
            var leftButtonRect = new Rect((wrect.width / 2) - ((PageButtonSpacing / 2) - PageButtonXPosOffset), wrect.height - (PageButtonHeight + 40), PageButtonWidth, PageButtonHeight);
            if (Widgets.ButtonText(leftButtonRect, "TinyTweaks.PreviousPage".Translate()))
                PageIndex--;

            var rightButtonRect = new Rect((wrect.width / 2) + ((PageButtonSpacing / 2) + PageButtonXPosOffset), wrect.height - (PageButtonHeight + 40), PageButtonWidth, PageButtonHeight);
            if (Widgets.ButtonText(rightButtonRect, "TinyTweaks.NextPage".Translate()))
                PageIndex++;

            var pageNumberRect = new Rect((wrect.width / 2) - 12.5f, wrect.height - (PageButtonHeight + 40), 120, PageButtonHeight);
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
            Scribe_Values.Look(ref tweakDubsBadHygiene, "tweakDubsBadHygiene", true);

            Scribe_Values.Look(ref artilleryCopyPasteGizmos, "artilleryCopyPasteGizmos", true);
        }

    }

}

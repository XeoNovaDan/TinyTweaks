using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace TinyTweaks
{

    public static class TinyTweaksUtility
    {

        public static void AddComp(this ThingDef def, Type compType)
        {
            if (def.comps.NullOrEmpty())
                def.comps = new List<CompProperties>();
            def.comps.Add(new CompProperties(compType));
        }

        public static void CollapsibleSubheading(this Listing_Standard listing, string label, ref bool checkOn)
        {
            float lineHeight = Text.LineHeight;
            var rowRect = listing.GetRect(lineHeight);
            var anchor = Text.Anchor;

            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rowRect, label);

            var buttonRect = new Rect(rowRect.x + rowRect.width - 150, rowRect.y, 150, rowRect.height);
            Text.Anchor = TextAnchor.MiddleCenter;
            if (Widgets.ButtonText(buttonRect, (checkOn ? "TinyTweaks.Collapse" : "TinyTweaks.Expand").Translate()))
            {
                SoundDefOf.Click.PlayOneShot(null);
                checkOn = !checkOn;
            }

            Text.Anchor = anchor;   
            listing.Gap(listing.verticalSpacing);
        }

    }

}

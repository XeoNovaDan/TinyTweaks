using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{

    public class TinyTweaks : Mod
    {

        public TinyTweaksSettings settings;

        public TinyTweaks(ModContentPack content) : base(content)
        {
            GetSettings<TinyTweaksSettings>();
        }

        public override string SettingsCategory() => "TinyTweaks.SettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<TinyTweaksSettings>().DoWindowContents(inRect);
        }

    }

}

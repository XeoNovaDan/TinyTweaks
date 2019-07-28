using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Harmony;
using System.Reflection;

namespace TinyTweaks
{

    public class TinyTweaks : Mod
    {

        public TinyTweaksSettings settings;

        public TinyTweaks(ModContentPack content) : base(content)
        {
            GetSettings<TinyTweaksSettings>();
            HarmonyInstance = HarmonyInstance.Create("XeoNovaDan.TinyTweaks");
        }

        public static HarmonyInstance HarmonyInstance;

        public override string SettingsCategory() => "TinyTweaks.SettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<TinyTweaksSettings>().DoWindowContents(inRect);
        }

    }

}

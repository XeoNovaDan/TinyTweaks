using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;

namespace TinyTweaks
{

    public class TinyTweaks : Mod
    {

        public TinyTweaksSettings settings;

        public TinyTweaks(ModContentPack content) : base(content)
        {
            settings = GetSettings<TinyTweaksSettings>();
            HarmonyInstance = new Harmony("XeoNovaDan.TinyTweaks");
        }

        public static Harmony HarmonyInstance;

        public override string SettingsCategory() => "TinyTweaks.SettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }

    }

}

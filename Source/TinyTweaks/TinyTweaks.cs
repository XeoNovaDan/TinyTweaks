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
            #if DEBUG
                Log.Error("XeoNovaDan left debugging enabled in Tiny Tweaks - please let him know!");
            #endif

            settings = GetSettings<TinyTweaksSettings>();
            harmonyInstance = new Harmony("XeoNovaDan.TinyTweaks");
        }

        public static Harmony harmonyInstance;

        public override string SettingsCategory() => "TinyTweaks.SettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }

    }

}

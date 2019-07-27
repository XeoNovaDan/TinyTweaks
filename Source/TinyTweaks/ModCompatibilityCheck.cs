using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace TinyTweaks
{

    public static class ModCompatibilityCheck
    {

        public static bool DubsBadHygiene => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Dubs Bad Hygiene");

    }

}

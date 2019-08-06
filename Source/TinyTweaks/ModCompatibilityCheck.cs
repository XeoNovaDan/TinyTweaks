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
    
    [StaticConstructorOnStartup]
    public static class ModCompatibilityCheck
    {

        private static bool ModLoaded(string name) => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == name);

        public static bool DubsBadHygiene = ModLoaded("Dubs Bad Hygiene");
        public static bool TurretExtensions = ModLoaded("[XND] Turret Extensions");

    }

}

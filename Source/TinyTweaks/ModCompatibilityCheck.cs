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

        static ModCompatibilityCheck()
        {
            var loadedMods = ModsConfig.ActiveModsInLoadOrder.ToList();

            for (int i = 0; i < loadedMods.Count; i++)
            {
                var curMod = loadedMods[i];

                if (curMod.PackageId.Equals("Dubwise.DubsBadHygiene", StringComparison.CurrentCultureIgnoreCase))
                    DubsBadHygiene = true;
            }
        }

        public static bool DubsBadHygiene;

    }

}

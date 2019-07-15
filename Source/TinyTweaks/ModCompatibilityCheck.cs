using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{

    public static class ModCompatibilityCheck
    {

        public const string MoreFurnitureName = "More Furniture [1.0]";
        public const string MoreFloorsName = "[T] MoreFloors";
        public const string DubsBadHygieneName = "Dubs Bad Hygiene";

        public static bool MoreFurniture => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == MoreFurnitureName);

        public static bool MoreFloors => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == MoreFloorsName);

        public static bool DubsBadHygiene => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == DubsBadHygieneName);

    }

}

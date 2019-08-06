using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using Harmony;

namespace TinyTweaks
{

    public static class Patch_TurretExtensions_Patch_StatsReportUtility
    {

        public static class manual_Patch_StatsToDraw_Def_ThingDef_Postfix
        {

            public static bool Prefix()
            {
                // If overrideTurretStatsFunctionality is true, return false on Turret Extensions' turret stat methods
                return !TinyTweaksSettings.overrideTurretStatsFunctionality;
            }

        }

        public static class manual_Patch_StatsToDraw_Thing_Postfix
        {

            public static bool Prefix()
            {
                return !TinyTweaksSettings.overrideTurretStatsFunctionality;
            }

        }

    }

}

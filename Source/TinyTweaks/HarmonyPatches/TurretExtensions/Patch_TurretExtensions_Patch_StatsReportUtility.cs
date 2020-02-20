using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace TinyTweaks
{

    public static class Patch_TurretExtensions_Patch_Building_TurretGun
    {

        public static class manual_Patch_Tick
        {

            public static bool Prefix()
            {
                return !TinyTweaksSettings.overrideSmarterForcedTargeting;
            }

        }

    }

}

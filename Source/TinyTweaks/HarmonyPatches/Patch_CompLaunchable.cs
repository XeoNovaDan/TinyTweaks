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

    public static class Patch_CompLaunchable
    {

        [HarmonyPatch(typeof(CompLaunchable), nameof(CompLaunchable.TryLaunch))]
        public static class TryLaunch
        {

            public static void Postfix(CompLaunchable __instance)
            {
                __instance.parent.BroadcastCompSignal(CompLaunchableAutoRebuild.AutoRebuildSignal);
            }

        }

    }

}

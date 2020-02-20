using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;
using RimWorld.Planet;
using HarmonyLib;

namespace TinyTweaks
{

    [StaticConstructorOnStartup]
    public static class NonPublicMethods
    {

        public static Func<Building_TurretGun, float> Building_TurretGun_BurstCooldownTime = (Func<Building_TurretGun, float>)
            Delegate.CreateDelegate(typeof(Func<Building_TurretGun, float>), null, AccessTools.Method(typeof(Building_TurretGun), "BurstCooldownTime"));
        public static Action<Building_TurretGun> Building_TurretGun_ResetForcedTarget = (Action<Building_TurretGun>)
            Delegate.CreateDelegate(typeof(Action<Building_TurretGun>), null, AccessTools.Method(typeof(Building_TurretGun), "ResetForcedTarget"));

        public static Func<Dialog_DebugOptionLister, string, Action, bool> Dialog_DebugOptionLister_DebugAction = (Func<Dialog_DebugOptionLister, string, Action, bool>)
            Delegate.CreateDelegate(typeof(Func<Dialog_DebugOptionLister, string, Action, bool>), null, AccessTools.Method(typeof(Dialog_DebugOptionLister), "DebugAction"));
        public static Action<Dialog_DebugOptionLister> Dialog_DebugOptionLister_DoGap = (Action<Dialog_DebugOptionLister>)
            Delegate.CreateDelegate(typeof(Action<Dialog_DebugOptionLister>), null, AccessTools.Method(typeof(Dialog_DebugOptionLister), "DoGap"));
        public static Action<Dialog_DebugOptionLister, string> Dialog_DebugOptionLister_DoLabel = (Action<Dialog_DebugOptionLister, string>)
            Delegate.CreateDelegate(typeof(Action<Dialog_DebugOptionLister, string>), null, AccessTools.Method(typeof(Dialog_DebugOptionLister), "DoLabel"));

        public static Func<Def, ThingDef, IEnumerable<StatDrawEntry>> StatsReportUtility_StatsToDraw_def_stuff = (Func<Def, ThingDef, IEnumerable<StatDrawEntry>>)
            Delegate.CreateDelegate(typeof(Func<Def, ThingDef, IEnumerable<StatDrawEntry>>), null, AccessTools.Method(typeof(StatsReportUtility), "StatsToDraw", new[] { typeof(Def), typeof(ThingDef) }));

        public static Func<Thing, IEnumerable<StatDrawEntry>> StatsReportUtility_StatsToDraw_thing = (Func<Thing, IEnumerable<StatDrawEntry>>)
            Delegate.CreateDelegate(typeof(Func<Thing, IEnumerable<StatDrawEntry>>), null, AccessTools.Method(typeof(StatsReportUtility), "StatsToDraw", new[] { typeof(Thing) }));

    }

}

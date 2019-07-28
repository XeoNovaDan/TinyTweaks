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
using Harmony;

namespace TinyTweaks
{

    [StaticConstructorOnStartup]
    public static class ReflectedMethods
    {

        static ReflectedMethods()
        {
            Building_TurretGun_BurstCooldownTime = (Func<Building_TurretGun, float>)Delegate.CreateDelegate(typeof(Func<Building_TurretGun, float>), null,
                AccessTools.Method(typeof(Building_TurretGun), "BurstCooldownTime"));

            StatsReportUtility_StatsToDraw_def_stuff = (Func<Def, ThingDef, IEnumerable<StatDrawEntry>>)Delegate.CreateDelegate(typeof(Func<Def, ThingDef, IEnumerable<StatDrawEntry>>), null,
                AccessTools.Method(typeof(StatsReportUtility), "StatsToDraw", new[] { typeof(Def), typeof(ThingDef) }));

            StatsReportUtility_StatsToDraw_thing = (Func<Thing, IEnumerable<StatDrawEntry>>)Delegate.CreateDelegate(typeof(Func<Thing, IEnumerable<StatDrawEntry>>), null,
                AccessTools.Method(typeof(StatsReportUtility), "StatsToDraw", new[] { typeof(Thing) }));
        }

        public static Func<Building_TurretGun, float> Building_TurretGun_BurstCooldownTime;

        public static Func<Def, ThingDef, IEnumerable<StatDrawEntry>> StatsReportUtility_StatsToDraw_def_stuff;

        public static Func<Thing, IEnumerable<StatDrawEntry>> StatsReportUtility_StatsToDraw_thing;

    }

}

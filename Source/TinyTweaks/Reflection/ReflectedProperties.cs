using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;
using RimWorld.Planet;

namespace TinyTweaks
{

    public static class ReflectedProperties
    {

        static ReflectedProperties()
        {
            Building_TurretGun_get_PlayerControlled = (Func<Building_TurretGun, bool>)Delegate.CreateDelegate(typeof(Func<Building_TurretGun, bool>), null,
                typeof(Building_TurretGun).GetProperty("PlayerControlled", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(true));

            WITab_get_SelCaravan = (Func<WITab, Caravan>)Delegate.CreateDelegate(typeof(Func<WITab, Caravan>), null,
                typeof(WITab).GetProperty("SelCaravan", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(true));
        }

        public static Func<Building_TurretGun, bool> Building_TurretGun_get_PlayerControlled;

        public static Func<WITab, Caravan> WITab_get_SelCaravan;

    }

}

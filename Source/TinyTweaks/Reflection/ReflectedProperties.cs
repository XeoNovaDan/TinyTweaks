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
            WITab_SelCaravan_Getter = (Func<WITab, Caravan>)Delegate.CreateDelegate(typeof(Func<WITab, Caravan>), null,
                typeof(WITab).GetProperty("SelCaravan", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(true));
        }

        public static Func<WITab, Caravan> WITab_SelCaravan_Getter;

    }

}

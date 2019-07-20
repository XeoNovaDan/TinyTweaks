using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{

    public static class TinyTweaksUtility
    {

        public static void AddComp(this ThingDef def, Type compType)
        {
            if (def.comps.NullOrEmpty())
                def.comps = new List<CompProperties>();
            def.comps.Add(new CompProperties(compType));
        }

    }

}

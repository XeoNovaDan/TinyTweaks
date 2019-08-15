using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{

    public class CompSmarterTurretTargeting : ThingComp
    {

        public bool attackingNonDownedPawn;

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref attackingNonDownedPawn, "attackingNonDownedPawn");
        }

    }

}

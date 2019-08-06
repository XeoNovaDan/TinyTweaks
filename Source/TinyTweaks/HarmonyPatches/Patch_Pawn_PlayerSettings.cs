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

    public static class Patch_Pawn_PlayerSettings
    {

        public static class manual_Ctor
        {

            public static void Postfix(Pawn_PlayerSettings __instance, Pawn pawn)
            {
                // If the pawn is an animal, automatically assign them to follow when drafted/doing fieldwork
                if (TinyTweaksSettings.autoAssignAnimalFollowSettings && pawn.RaceProps.Animal)
                {
                    __instance.followDrafted = true;
                    __instance.followFieldwork = true;
                }
            }

        }

    }

}

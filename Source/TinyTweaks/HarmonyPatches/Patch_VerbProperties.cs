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

    public static class Patch_VerbProperties
    {

        [HarmonyPatch(typeof(VerbProperties))]
        [HarmonyPatch(nameof(VerbProperties.AdjustedArmorPenetration))]
        [HarmonyPatch(new Type[] { typeof(Tool), typeof(Pawn), typeof(Thing), typeof(HediffComp_VerbGiver) })]
        public static class Patch_AdjustedArmorPenetration
        {

            public static void Postfix(VerbProperties __instance, Tool tool, Thing equipment, ref float __result)
            {
                // Not gated behind an option since it is a bug fix :3
                // Scale forced AP with quality and stuff like everything else
                if (tool != null && __result == tool.armorPenetration && equipment != null)
                {
                    __result *= equipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier);
                    if (equipment.Stuff != null && __instance.meleeDamageDef != null && __instance.meleeDamageDef.armorCategory.multStat != null)
                        __result *= equipment.Stuff.GetStatValueAbstract(__instance.meleeDamageDef.armorCategory.multStat);
                }
            }

        }

    }

}

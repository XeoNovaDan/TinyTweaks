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
        public static class AdjustedArmorPenetration
        {

            public static void Postfix(VerbProperties __instance, Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource, ref float __result)
            {
                // Scale defined AP with quality and stuff like everything else
                if (TinyTweaksSettings.meleeArmourPenetrationFix)
                {
                    if (tool != null && __result == tool.armorPenetration && equipment != null)
                    {
                        __result *= equipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier);
                        if (equipment.Stuff != null && __instance.meleeDamageDef != null && __instance.meleeDamageDef.armorCategory.multStat != null)
                            __result *= equipment.Stuff.GetStatValueAbstract(__instance.meleeDamageDef.armorCategory.multStat);
                    }
                    if (attacker != null)
                        __result *= __instance.GetDamageFactorFor(tool, attacker, hediffCompSource);
                }
            }

        }

    }

}

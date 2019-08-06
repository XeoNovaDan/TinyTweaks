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

    public static class Patch_ThingDef
    {

        [HarmonyPatch(typeof(ThingDef))]
        [HarmonyPatch(nameof(ThingDef.AllRecipes), MethodType.Getter)]
        public static class AllRecipes_Getter
        {

            public static void Postfix(ref List<RecipeDef> __result)
            {
                // Sort the resulting list alphabetically
                if (TinyTweaksSettings.alphabeticalBillList)
                    __result = __result.OrderBy(r => r.label).ToList();
            }

        }

        [HarmonyPatch(typeof(ThingDef))]
        [HarmonyPatch(nameof(ThingDef.SpecialDisplayStats))]
        public static class SpecialDisplayStats
        {

            public static void Postfix(ThingDef __instance, StatRequest req, ref IEnumerable<StatDrawEntry> __result)
            {

                // Minimum range
                if (__instance.Verbs.FirstOrDefault(v => v.isPrimary) is VerbProperties verb)
                {
                    var verbStatCategory = (__instance.category != ThingCategory.Pawn) ? StatCategoryDefOf.Weapon : StatCategoryDefOf.PawnCombat;
                    if (verb.LaunchesProjectile)
                    {
                        if (verb.minRange > default(float))
                            __result = __result.Add(new StatDrawEntry(verbStatCategory, "MinimumRange".Translate(), verb.minRange.ToString("F0"), 10, String.Empty));
                    }
                }
                
                if (!ModCompatibilityCheck.TurretExtensions || TinyTweaksSettings.overrideTurretStatsFunctionality)
                {
                    // Add turret weapons stats to the list of SpecialDisplayStats
                    var buildingProps = __instance.building;
                    if (buildingProps != null && buildingProps.IsTurret)
                    {
                        var gunStatList = new List<StatDrawEntry>();
                        if (req.HasThing)
                        {
                            var gun = ((Building_TurretGun)req.Thing).gun;
                            gunStatList.AddRange(gun.def.SpecialDisplayStats(StatRequest.For(gun)));
                            gunStatList.AddRange(NonPublicMethods.StatsReportUtility_StatsToDraw_thing(gun));
                        }
                        else
                        {
                            var defaultStuff = GenStuff.DefaultStuffFor(buildingProps.turretGunDef);
                            gunStatList.AddRange(buildingProps.turretGunDef.SpecialDisplayStats(StatRequest.For(buildingProps.turretGunDef, defaultStuff)));
                            gunStatList.AddRange(NonPublicMethods.StatsReportUtility_StatsToDraw_def_stuff(buildingProps.turretGunDef, defaultStuff));
                        }

                        // Replace gun warmup and cooldown with turret warmup and cooldown
                        var cooldownEntry = gunStatList.FirstOrDefault(s => s.stat == StatDefOf.RangedWeapon_Cooldown);
                        if (cooldownEntry != null)
                            cooldownEntry = new StatDrawEntry(cooldownEntry.category, cooldownEntry.LabelCap, TurretCooldown(req, buildingProps).ToStringByStyle(cooldownEntry.stat.toStringStyle),
                                cooldownEntry.DisplayPriorityWithinCategory, cooldownEntry.overrideReportText);
                        else
                        {
                            var cooldownStat = StatDefOf.RangedWeapon_Cooldown;
                            gunStatList.Add(new StatDrawEntry(cooldownStat.category, cooldownStat, buildingProps.turretBurstCooldownTime, StatRequest.ForEmpty(), cooldownStat.toStringNumberSense));
                        }

                        var warmupEntry = gunStatList.FirstOrDefault(s => s.LabelCap == "WarmupTime".Translate().CapitalizeFirst());
                        if (warmupEntry != null)
                            warmupEntry = new StatDrawEntry(warmupEntry.category, warmupEntry.LabelCap, $"{buildingProps.turretBurstWarmupTime.ToString("0.##")} s",
                                warmupEntry.DisplayPriorityWithinCategory, warmupEntry.overrideReportText);
                            

                        gunStatList.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
                        __result = __result.Concat(gunStatList.Where(s => s.category == StatCategoryDefOf.Weapon));
                    }
                        
                }
            }

            private static float TurretCooldown(StatRequest req, BuildingProperties buildingProps)
            {
                if (req.HasThing)
                    return NonPublicMethods.Building_TurretGun_BurstCooldownTime((Building_TurretGun)req.Thing);
                else if (buildingProps.turretBurstCooldownTime >= 0)
                    return buildingProps.turretBurstCooldownTime;
                else
                    return buildingProps.turretGunDef.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown);
            }

        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TinyTweaks
{

    public class CompLaunchableAutoRebuild : ThingComp
    {

        public const string AutoRebuildSignal = "TT_ParentLaunched";

        private bool autoRebuild;
        private Map previousMap;

        private bool CanToggleAutoRebuild => parent.Faction == Faction.OfPlayer && parent.def.blueprintDef != null && parent.def.IsResearchFinished;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            autoRebuild = CanToggleAutoRebuild && parent.Map.areaManager.Home[parent.Position];
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
                yield return gizmo;

            // Automatic rebuild
            if (CanToggleAutoRebuild)
            {
                yield return new Command_Toggle
                {
                    defaultLabel = "TinyTweaks.CommandAutoRebuild".Translate(),
                    defaultDesc = "TinyTweaks.CommandAutoRebuild_Description".Translate(parent.def.label),
                    icon = parent.def.uiIcon,
                    isActive = () => autoRebuild,
                    toggleAction = () => autoRebuild = !autoRebuild
                };
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            this.previousMap = previousMap;
            base.PostDestroy(mode, previousMap);
        }

        public override void ReceiveCompSignal(string signal)
        {
            if (signal == AutoRebuildSignal && autoRebuild)
            {
                GenConstruct.PlaceBlueprintForBuild(parent.def, parent.Position, previousMap, parent.Rotation, Faction.OfPlayer, parent.Stuff);
            }

            base.ReceiveCompSignal(signal);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref autoRebuild, "autoRebuild");
        }

    }

}

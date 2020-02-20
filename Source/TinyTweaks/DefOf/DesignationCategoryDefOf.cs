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

    [DefOf]
    public static class DesignationCategoryDefOf
    {

        // Vanilla defs
        public static DesignationCategoryDef Furniture;
        public static DesignationCategoryDef Floors;
        public static DesignationCategoryDef Temperature;

        public static DesignationCategoryDef ANON2MF => DefDatabase<DesignationCategoryDef>.GetNamedSilentFail("ANON2MF");

        public static DesignationCategoryDef MoreFloors => DefDatabase<DesignationCategoryDef>.GetNamedSilentFail("MoreFloors");

        public static DesignationCategoryDef HygieneMisc => DefDatabase<DesignationCategoryDef>.GetNamedSilentFail("HygieneMisc");

        public static DesignationCategoryDef Hygiene => DefDatabase<DesignationCategoryDef>.GetNamedSilentFail("Hygiene");

        public static DesignationCategoryDef Storage => DefDatabase<DesignationCategoryDef>.GetNamedSilentFail("Storage");

        public static DesignationCategoryDef DefensesExpanded_CustomCategory => DefDatabase<DesignationCategoryDef>.GetNamedSilentFail("DefensesExpanded_CustomCategory");

    }

}

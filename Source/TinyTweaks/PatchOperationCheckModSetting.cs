using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Verse;
using RimWorld;
using System.Xml;

namespace TinyTweaks
{

    public class PatchOperationCheckModSetting : PatchOperation
    {

        private string settingName;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            var settingInfo = typeof(TinyTweaksSettings).GetField(settingName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (settingInfo == null)
            {
                Log.Error($"Error with PatchOperationCheckModSetting in {sourceFile}: {settingName} could not be found");
                return false;
            }
            return (bool)settingInfo.GetValue(null);
        }

    }

}

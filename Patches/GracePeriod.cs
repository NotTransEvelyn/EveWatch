using GorillaNetworking;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveWatch.Patches
{
    [HarmonyPatch(typeof(GorillaNetworkPublicTestsJoin), "GracePeriod")]
    public class GracePeriod
    {
        static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaNetworkPublicTestJoin2), "GracePeriod")]
    public class GracePeriod2
    {
        static bool Prefix()
        {
            return false;
        }
    }
}

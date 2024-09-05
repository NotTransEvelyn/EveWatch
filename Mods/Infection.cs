using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EveWatch.Mods
{
    public class Infection
    {
        #region TagGun

        #endregion

        #region TagAll

        #endregion

        #region TagAura
        static Dictionary<string, float> tagAuraAndDist = new Dictionary<string, float>()
        {
            { "Short", 0.5f },
            { "Comp", 1.2f },
            { "Extreme", 5 },
        };

        static int currentTagAuraIndex;

        public static string CurrentTagAuraName;
        static float dist;

        public static void SwitchTagType(bool foo = false)
        {
            if (!foo) currentTagAuraIndex++;
            if (currentTagAuraIndex == tagAuraAndDist.Count) currentTagAuraIndex = 0;
            CurrentTagAuraName = tagAuraAndDist.ElementAt(currentTagAuraIndex).Key;
            dist = tagAuraAndDist.ElementAt(currentTagAuraIndex).Value;
            if (!foo)
            {
                Main.GetMod("Tag Aura").Desc = $"Type: {CurrentTagAuraName}\nLets you tag\npeople easier!";
                Main.GetMod("Change Distance").Desc = $"Distance: {CurrentTagAuraName}";
            }
        }

        public static void TagAura()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                float distance = Vector3.Distance(vrrig.headMesh.transform.position, GorillaTagger.Instance.offlineVRRig.head.rigTarget.position);

                if (GorillaTagger.Instance.offlineVRRig.setMatIndex != 0 && vrrig.setMatIndex == 0 && GorillaLocomotion.Player.Instance.disableMovement == false && distance < dist)
                {
                    GorillaLocomotion.Player.Instance.rightControllerTransform.position = vrrig.headMesh.transform.position;
                }
            }
        }

        #endregion
    }
}

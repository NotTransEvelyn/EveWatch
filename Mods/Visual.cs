using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EveWatch.Mods
{
    public class Visual : MonoBehaviour
    {
        #region Tracers
        static bool TracersEnabled;
        public static void Tracers() => TracersEnabled = true;
        public static void DisableTracers() => TracersEnabled = false;
        #endregion

        #region BoxESP
        static bool BoxEspEnabled;
        public static void BoxESP() => BoxEspEnabled = true;
        public static void DisableBoxESP() => BoxEspEnabled = false;
        #endregion

        void Update()
        {
            #region Tracers
            if (TracersEnabled)
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (var person in PhotonNetwork.PlayerListOthers)
                    {
                        VRRig vRRig = GorillaTagManager.instance.FindPlayerVRRig(person);
                        if (vRRig.headConstraint.GetComponent<LineRenderer>() == null)
                        {
                            LineRenderer line = vRRig.headConstraint.AddComponent<LineRenderer>();
                            line.material.shader = Shader.Find("GUI/Text Shader");
                            line.startWidth = 0.001f;
                            line.endWidth = 0.001f;
                        }

                        if (vRRig.setMatIndex != 2 && vRRig.setMatIndex != 1)
                        {
                            vRRig.headConstraint.GetComponent<LineRenderer>().startColor = Color.green;
                            vRRig.headConstraint.GetComponent<LineRenderer>().endColor = Color.green;
                        }
                        else
                        {
                            vRRig.headConstraint.GetComponent<LineRenderer>().startColor = Color.red;
                            vRRig.headConstraint.GetComponent<LineRenderer>().endColor = Color.red;
                        }
                        vRRig.headConstraint.GetComponent<LineRenderer>().SetPosition(1, GorillaTagger.Instance.offlineVRRig.rightHandTransform.position);
                        vRRig.headConstraint.GetComponent<LineRenderer>().SetPosition(0, vRRig.headConstraint.position);
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (var person in PhotonNetwork.PlayerListOthers)
                    {
                        if (GorillaTagManager.instance.FindPlayerVRRig(person).headConstraint.GetComponent<LineRenderer>() != null)
                        {
                            Destroy(GorillaTagManager.instance.FindPlayerVRRig(person).headConstraint.GetComponent<LineRenderer>());
                        }
                    }
                }
            }
            #endregion

            #region BoxESP
            if (BoxEspEnabled)
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (var wawa in PhotonNetwork.PlayerListOthers)
                    {
                        VRRig vRRig = GorillaTagManager.instance.FindPlayerVRRig(wawa);
                        TextMesh text = null;

                        if (vRRig.transform.Find("ESP") != null) Destroy(vRRig.transform.Find("ESP").gameObject);

                        GameObject wa = new GameObject("ESP");
                        wa.transform.SetParent(vRRig.transform);
                        wa.transform.localPosition = Vector3.zero;
                        wa.transform.localScale = new Vector3(0.025f, 0.025f, 0);
                        wa.transform.LookAt(Camera.main.transform);

                        text = wa.AddComponent<TextMesh>();
                        text.text = "☐";
                        text.alignment = TextAlignment.Center;
                        text.anchor = TextAnchor.MiddleCenter;
                        text.fontSize = 1000;

                        if (vRRig.setMatIndex != 2 && vRRig.setMatIndex != 1) text.color = Color.green;
                        else text.color = Color.red;
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom) foreach (var wawa in PhotonNetwork.PlayerListOthers) if (GorillaTagManager.instance.FindPlayerVRRig(wawa).transform.Find("ESP") != null) Destroy(GorillaTagManager.instance.FindPlayerVRRig(wawa).transform.Find("ESP").gameObject);
            }
            #endregion
        }
    }
}

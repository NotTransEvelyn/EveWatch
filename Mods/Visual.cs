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

        #region WatchESP
        static bool WatchEspEnabled;
        public static void WatchESP() => WatchEspEnabled = true;
        public static void DisableWatchESP() => WatchEspEnabled = false;
        #endregion

        #region SkellESP
        static bool SkellEspEnabled;
        public static void SkellESP() => SkellEspEnabled = true;
        public static void DisableSkellESP()
        {
            SkellEspEnabled = false;
            foreach(VRRig rig in Resources.FindObjectsOfTypeAll<VRRig>())
            {
                rig.ShowSkeleton(false);
            }
        }
        #endregion

        void Update()
        {
            #region Tracers
            if (TracersEnabled)
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (var person in NetworkSystem.Instance.AllNetPlayers)
                    {
                        VRRig vRRig = GorillaTagManager.instance.FindPlayerVRRig(person);
                        if (vRRig != null && !vRRig.isLocal)
                        {
                            if (vRRig.headConstraint.GetComponent<LineRenderer>() == null)
                            {
                                LineRenderer line = vRRig.headConstraint.AddComponent<LineRenderer>();
                                line.material.shader = Shader.Find("GUI/Text Shader");
                                line.startWidth = 0.001f;
                                line.endWidth = 0.001f;
                            }
                            if (SkellEspEnabled)
                            {
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
                            }
                            else
                            {
                                if (vRRig.setMatIndex != 2 && vRRig.setMatIndex != 1 && !vRRig.skeleton.renderer.enabled)
                                {
                                    vRRig.headConstraint.GetComponent<LineRenderer>().startColor = Color.green;
                                    vRRig.headConstraint.GetComponent<LineRenderer>().endColor = Color.green;
                                }
                                else
                                {
                                    vRRig.headConstraint.GetComponent<LineRenderer>().startColor = Color.red;
                                    vRRig.headConstraint.GetComponent<LineRenderer>().endColor = Color.red;
                                }
                            }
                            vRRig.headConstraint.GetComponent<LineRenderer>().SetPosition(1, GorillaTagger.Instance.offlineVRRig.rightHandTransform.position);
                            vRRig.headConstraint.GetComponent<LineRenderer>().SetPosition(0, vRRig.headConstraint.position);
                        }
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (var person in NetworkSystem.Instance.AllNetPlayers)
                    {
                        if (!GorillaTagManager.instance.FindPlayerVRRig(person).isLocal)
                        {
                            if (GorillaTagManager.instance.FindPlayerVRRig(person).headConstraint.GetComponent<LineRenderer>() != null)
                            {
                                Destroy(GorillaTagManager.instance.FindPlayerVRRig(person).headConstraint.GetComponent<LineRenderer>());
                            }
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
                    foreach (var wawa in NetworkSystem.Instance.AllNetPlayers)
                    {
                        VRRig vRRig = GorillaTagManager.instance.FindPlayerVRRig(wawa);
                        if (!vRRig.isLocal)
                        {
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
                            if (SkellEspEnabled)
                            {
                                if (vRRig.setMatIndex != 2 && vRRig.setMatIndex != 1) text.color = Color.green;
                                else text.color = Color.red;
                            }
                            else
                            {
                                if (vRRig.setMatIndex != 2 && vRRig.setMatIndex != 1 && !vRRig.skeleton.renderer.enabled) text.color = Color.green;
                                else text.color = Color.red;
                            }
                        }
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom) foreach (var wawa in NetworkSystem.Instance.AllNetPlayers) if (!GorillaTagManager.instance.FindPlayerVRRig(wawa).isLocal) if (GorillaTagManager.instance.FindPlayerVRRig(wawa).transform.Find("ESP") != null) Destroy(GorillaTagManager.instance.FindPlayerVRRig(wawa).transform.Find("ESP").gameObject);
            }
            #endregion

            #region WatchESP
            if (WatchEspEnabled)
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (var wawa in NetworkSystem.Instance.AllNetPlayers)
                    {
                        if (wawa.GetPlayerRef().CustomProperties.ContainsKey("EveWatch"))
                        {
                            VRRig vRRig = GorillaTagManager.instance.FindPlayerVRRig(wawa);
                            if (!vRRig.isLocal)
                            {
                                TextMesh text = null;

                                if (vRRig.transform.Find("WATCHESP") != null) Destroy(vRRig.transform.Find("WATCHESP").gameObject);

                                GameObject wa = new GameObject("WATCHESP");
                                wa.transform.SetParent(vRRig.transform);
                                wa.transform.localPosition = Vector3.zero;
                                wa.transform.localScale = new Vector3(0.025f, 0.025f, 0);
                                wa.transform.LookAt(Camera.main.transform);

                                text = wa.AddComponent<TextMesh>();
                                text.text = "☐";
                                text.alignment = TextAlignment.Center;
                                text.anchor = TextAnchor.MiddleCenter;
                                text.fontSize = 1000;

                                text.color = Color.yellow;
                            }
                        }
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom) foreach (var wawa in NetworkSystem.Instance.AllNetPlayers) if (!GorillaTagManager.instance.FindPlayerVRRig(wawa).isLocal && wawa.GetPlayerRef().CustomProperties.ContainsKey("EveWatch")) if (GorillaTagManager.instance.FindPlayerVRRig(wawa).transform.Find("WATCHESP") != null) Destroy(GorillaTagManager.instance.FindPlayerVRRig(wawa).transform.Find("WATCHESP").gameObject);
            }
            #endregion

            #region SkellEsp
            if (SkellEspEnabled)
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (var wawa in NetworkSystem.Instance.AllNetPlayers)
                    {
                        VRRig vRRig = GorillaTagManager.instance.FindPlayerVRRig(wawa);
                        if (!vRRig.isLocal)
                        {
                            vRRig.ShowSkeleton(true);
                            vRRig.skeleton.renderer.material.shader = Shader.Find("GUI/Text Shader");

                            if (vRRig.setMatIndex != 2 && vRRig.setMatIndex != 1) vRRig.skeleton.renderer.material.color = Color.green;
                            else vRRig.skeleton.renderer.material.color = Color.red;
                        }
                    }
                }
            }
            #endregion
        }
    }
}

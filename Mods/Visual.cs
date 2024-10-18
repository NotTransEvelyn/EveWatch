using MonkeNotificationLib;
using Photon.Pun;
using PlayFab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
                ShowSkeleton(rig, false);
            }
        }
        #endregion

        #region Tag Aura Radius
        static bool tagAuraRadiusEnabled;
        static GameObject radius;
        public static void TagAuraRad()
        {
            tagAuraRadiusEnabled = true;
            if (PhotonNetwork.InRoom && (GorillaTagger.Instance.offlineVRRig.setMatIndex == 2 || GorillaTagger.Instance.offlineVRRig.setMatIndex == 1))
            {
                CreateThing();
            }
        }

        static void CreateThing()
        {
            if (radius == null)
            {
                radius = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                radius.transform.localScale = new Vector3(1, 0.01f, 1);
                radius.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                radius.GetComponent<Renderer>().material.color = new Color(0.6f, 0, 1, 0.1f);

                Destroy(radius.GetComponent<Collider>());
                radius.transform.SetParent(GorillaTagger.Instance.offlineVRRig.transform, false);
                radius.transform.localPosition = new Vector3(0, -0.5f, 0);
            }
        }

        public static void TagAuraRadDisable()
        {
            tagAuraRadiusEnabled = false;
            GameObject.Destroy(radius);
        }
        #endregion

        #region ModList
        public static bool modListEnabled = true;
        static string modListText;
        static TextMeshPro modListTMPro;

        public static void RestartText()
        {
            if (modListEnabled)
            {
                if (modListTMPro != null)
                {
                    modListText = "";
                    foreach (var item in Main.Mods)
                    {
                        if (item.Value && item.Key.Name != "Mod List")
                        {
                            if (item.Key.Name != "Tag Aura" && item.Key.Name != "Speed Boost")
                            {
                                modListText += $"{item.Key.Name}\n";
                            }else
                            {
                                if (item.Key.Name == "Tag Aura")
                                {
                                    modListText += $"{item.Key.Name}, " + Infection.CurrentTagAuraName.WrapColor("green") + "\n";
                                }else if (item.Key.Name == "Speed Boost")
                                {
                                    modListText += $"{item.Key.Name}, " + Movement.CurrentSpeedName.WrapColor("green") + "\n";
                                }
                            }
                        }
                    }
                    modListTMPro.text = modListText.ToUpper();
                }
                else
                {
                    BuildText();
                }
            }
            else
            {
                if (modListTMPro.transform != null)
                {
                    GameObject.Destroy(modListTMPro.gameObject);
                }
            }

        }

        static void BuildText()
        {
            modListTMPro = new GameObject("ModList").AddComponent<TextMeshPro>();
            modListTMPro.alignment = TextAlignmentOptions.TopRight;
            modListTMPro.richText = true;
            modListTMPro.material = Instantiate(GorillaTagger.Instance.offlineVRRig.playerText1.material);
            modListTMPro.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            modListTMPro.fontSize = 0.15f;
            modListTMPro.transform.localPosition = new Vector3(-9.7f, -2.4f, 0.6f);
            modListTMPro.text = "";
            modListTMPro.transform.SetParent(Camera.main.transform, false);
            RestartText();
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
                        }else
                        {
                            VRRig vRRig = GorillaTagManager.instance.FindPlayerVRRig(wawa);
                            if (vRRig.transform.Find("WATCHESP") != null) Destroy(vRRig.transform.Find("WATCHESP").gameObject);
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
                            ShowSkeleton(vRRig, true);
                            vRRig.skeleton.renderer.material.shader = Shader.Find("GUI/Text Shader");

                            if (vRRig.setMatIndex != 2 && vRRig.setMatIndex != 1) vRRig.skeleton.renderer.material.color = Color.green;
                            else vRRig.skeleton.renderer.material.color = Color.red;
                        }
                    }
                }
            }
            #endregion

            #region Tag Aura Radius
            if (tagAuraRadiusEnabled && PhotonNetwork.InRoom)
            {
                if (GorillaTagger.Instance.offlineVRRig.setMatIndex == 2 || GorillaTagger.Instance.offlineVRRig.setMatIndex == 1)
                {
                    CreateThing();
                    if (radius != null)
                    {
                        float distance = 0;
                        distance = Infection.dist;
                        distance = distance * 2;
                        radius.transform.localScale = new Vector3(distance, 0.0001f, distance);
                    }
                }
                else
                {
                    GameObject.Destroy(radius);
                }
            }
            #endregion
        }

        static void ShowSkeleton(VRRig rig, bool show)
        {
            rig.skeleton.renderer.enabled = show;
            rig.mainSkin.enabled = !show;
            rig.faceSkin.enabled = !show;
        }
    }
}

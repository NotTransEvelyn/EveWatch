using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EveWatch.Librarys
{
    internal class GunLib
    {
        public class GunLibData
        {
            public VRRig lockedPlayer { get; set; }

            public bool isShooting { get; set; }

            public bool isLocked { get; set; }

            public Vector3 hitPosition { get; set; }

            public RaycastHit RaycastHit { get; set; }

            public bool isTriggered { get; set; }

            public GunLibData(bool stateTriggered, bool triggy, bool foundPlayer, VRRig player = null, Vector3 hitpos = new Vector3(), RaycastHit raycastHit = default)
            {
                lockedPlayer = player;
                isShooting = stateTriggered;
                isLocked = foundPlayer;
                hitPosition = hitpos;
                isTriggered = triggy;
                RaycastHit = raycastHit;
            }
        }

        static GameObject pointer;
        static LineRenderer lr;
        static GunLibData data = new GunLibData(false, false, false);

        public static void GunCleanUp()
        {
            if (pointer == null || lr == null) { return; }
            GameObject.Destroy(pointer);
            pointer = null;
            GameObject.Destroy(lr.gameObject);
            lr = null;
            data = new GunLibData(false, false, false);
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static GunLibData ShootLock()
        {
            try
            {
                bool rightHand3 = false;

                Transform controller;
                if (!rightHand3)
                {
                    controller = GorillaLocomotion.Player.Instance.rightControllerTransform;
                    data.isShooting = ControllerInputPoller.instance.rightGrab;
                    data.isTriggered = ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f;
                }
                else
                {
                    controller = GorillaLocomotion.Player.Instance.leftControllerTransform;
                    data.isShooting = ControllerInputPoller.instance.leftGrab;
                    data.isTriggered = ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f; ;
                }

                if (data.isShooting)
                {
                    Renderer pr = pointer?.GetComponent<Renderer>();
                    if (data.lockedPlayer == null && !data.isLocked)
                    {
                        if (Physics.Raycast(controller.position - controller.up, -controller.up, out RaycastHit hit, Mathf.Infinity) && pointer == null)
                        {
                            pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            GameObject.Destroy(pointer.GetComponent<Rigidbody>());
                            GameObject.Destroy(pointer.GetComponent<SphereCollider>());
                            pointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                            pr = pointer?.GetComponent<Renderer>();
                            pr.material.color = Color.red;
                            pr.material.shader = Shader.Find("GUI/Text Shader");
                        }
                        if (lr == null)
                        {
                            var lrob = new GameObject("line");
                            lr = lrob.AddComponent<LineRenderer>();
                            lr.endWidth = 0.01f;
                            lr.startWidth = 0.01f;
                            lr.material.shader = Shader.Find("GUI/Text Shader");
                        }
                        lr.SetPosition(0, controller.position);
                        lr.SetPosition(1, hit.point);
                        data.hitPosition = hit.point;
                        pointer.transform.position = hit.point;
                        VRRig vrrig = hit.collider.GetComponentInParent<VRRig>();
                        if (vrrig != null)
                        {
                            if (data.isTriggered)
                            {
                                data.lockedPlayer = vrrig;
                                data.isLocked = true;
                                lr.SetColour(Color.blue, 0.5f);
                                pr.material.color = Color.blue;
                            }
                            else
                            {
                                data.isLocked = false;
                                lr.SetColour(Color.green, 0.5f);
                                pr.material.color = Color.green;
                                GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                            }
                        }
                        else
                        {
                            data.isLocked = false;
                            lr.SetColour(Color.red, 0.5f);
                            pr.material.color = Color.red;
                        }
                    }

                    if (data.isTriggered && data.lockedPlayer != null)
                    {
                        data.isLocked = true;
                        lr.SetPosition(0, controller.position);
                        lr.SetPosition(1, data.lockedPlayer.transform.position);
                        data.hitPosition = data.lockedPlayer.transform.position;
                        pointer.transform.position = data.lockedPlayer.transform.position;
                        lr.SetColour(Color.blue, 0.5f);
                        pr.material.color = Color.blue;
                    }
                    else if (data.lockedPlayer != null)
                    {
                        data.isLocked = false;
                        data.lockedPlayer = null;
                        lr.SetColour(Color.red, 0.5f);
                        pr.material.color = Color.red;
                    }
                }
                else
                {
                    GunCleanUp();
                }
                return data;
            }
            catch (Exception ex) { Debug.Log(ex.ToString()); return null; }
        }

        public static GunLibData Shoot()
        {
            try
            {
                bool rightHand3 = false;
                Transform controller;
                if (!rightHand3)
                {
                    controller = GorillaLocomotion.Player.Instance.rightControllerTransform;
                    data.isShooting = ControllerInputPoller.instance.rightGrab;
                    data.isTriggered = ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f;

                }
                else
                {
                    controller = GorillaLocomotion.Player.Instance.leftControllerTransform;
                    data.isShooting = ControllerInputPoller.instance.leftGrab;
                    data.isTriggered = ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f;
                }
                if (data.isShooting)
                {
                    Renderer pr = pointer?.GetComponent<Renderer>();
                    if (Physics.Raycast(controller.position - controller.up, -controller.up, out RaycastHit hit, Mathf.Infinity, GorillaLocomotion.Player.Instance.locomotionEnabledLayers) && pointer == null)
                    {
                        pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        GameObject.Destroy(pointer.GetComponent<SphereCollider>());
                        pointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        pr = pointer?.GetComponent<Renderer>();
                        pr.material.color = Color.red;
                        pr.material.shader = Shader.Find("GUI/Text Shader");
                    }
                    if (lr == null)
                    {
                        var lrob = new GameObject("line");
                        lr = lrob.AddComponent<LineRenderer>();
                        lr.endWidth = 0.01f;
                        lr.startWidth = 0.01f;
                        lr.material.shader = Shader.Find("GUI/Text Shader");
                    }
                    lr.SetPosition(0, controller.position);
                    lr.SetPosition(1, hit.point);
                    data.hitPosition = hit.point;
                    data.RaycastHit = hit;
                    pointer.transform.position = hit.point;
                    VRRig rig = hit.collider.GetComponentInParent<VRRig>();
                    if (rig != null)
                    {
                        if (data.isTriggered)
                        {
                            data.lockedPlayer = rig;
                            data.isLocked = true;
                            pr.material.color = Color.blue;
                            lr.SetColour(Color.blue, 0.5f);
                        }
                        else
                        {
                            lr.SetColour(Color.red, 0.5f);
                            pr.material.color = Color.green;
                            data.isLocked = false;
                            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tagHapticStrength / 3, GorillaTagger.Instance.tagHapticDuration / 2);
                        }
                    }
                    else
                    {
                        lr.SetColour(Color.red, 0.5f);
                        pr.material.color = Color.red;
                        data.isLocked = false;
                    }

                }
                else
                {
                    GunCleanUp();
                }
                return data;
            }
            catch (Exception ex) { Debug.Log(ex.ToString()); return null; }
        }
    }

    static class LineRendererEx
    {
        public static void SetColour(this LineRenderer lineRenderer, Color colour, float transparency)
        {
            Color color = colour;
            color.a = transparency;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }
}
using BepInEx;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TheGorillaWatch
{
    [BepInPlugin("com.ArtificialGorillas.gorillatag.GorillaWatch", "GorillaWatch", "1.3.1")]
    public class Mod : BaseUnityPlugin
    {
        public static int counter;

        public static float PageCoolDown;

        public static int layer = 29, layerMask = 1 << layer;

        private LayerMask baseMask;

        float bounce;

        private bool toggleWatch = true;

        PhysicMaterialCombine PMCombine;

        int modCount;

        public static Dictionary<string, bool> Mods;

        public static GameObject leftplat = null;

        public static GameObject rightplat = null;

        public static GameObject Frozone = null;

        public static GameObject FrozoneR = null;


        public static GameObject DrawR = null;

        public static GameObject DrawL = null;

        public static GameObject Swim = null;

        public static Vector3[] lastLeft = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

        public static Vector3[] lastRight = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

        Vector3 AddForceStuff = new Vector3(0f, 40f, 0f);
        bool doneDeletion;

        void Start()
        {
            Mods = new Dictionary<string, bool>()
            {
                { "GorillaWatch!\nMade by:\nARTIFICIALGORILLAS", false },
                { "PlatformGuy", false },
                { "FrozoneGuy", false },
                { "DrawingGuy", false },
                { "NoClip", false },
                { "HoverMonke", false },
                { "VelocityFly", false },
                { "HighGravity", false },
                { "LowGravity", false },
                { "NoGravity", false },
                { "BigMonkers", false },
                { "SmallMonkers", false },
                { "MonkePunch", false },
                { "MonkeBoing", false },
                { "AirSwim", false },
            };
            modCount = Mods.Count;
        }
        GorillaHuntComputer huntComputer;
        Text huntText;
        void Update()
        {
            if (inModded())
            {
                if (!doneDeletion)
                {
                    huntComputer = GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>();
                    huntText = huntComputer.text;
                    huntComputer.enabled = false;
                    huntComputer.gameObject.SetActive(true);
                    GameObject.Destroy(huntComputer.material);
                    GameObject.Destroy(huntComputer.badge);
                    GameObject.Destroy(huntComputer.leftHand);
                    GameObject.Destroy(huntComputer.rightHand);
                    GameObject.Destroy(huntComputer.hat);
                    GameObject.Destroy(huntComputer.face);
                    Debug.Log("GorillaWatch Has Loaded Successfully");
                    doneDeletion = true;
                }

                if (ControllerInputPoller.instance.rightControllerIndexFloat >= .5f && Time.time > PageCoolDown + 0.5)
                {
                    PageCoolDown = Time.time;
                    counter++;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, true, 1f);
                }
                if (ControllerInputPoller.instance.leftControllerIndexFloat >= .5f && Time.time > PageCoolDown + 0.5)
                {
                    PageCoolDown = Time.time;
                    counter--;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, true, 1f);
                }

                if (counter < 0) counter = modCount;

                if (counter > modCount) counter = 0;

                if (counter != 0)
                {
                    huntText.text = $"{Mods.ElementAt(counter).Key}--{Mods.ElementAt(counter).Value.ToString()}";
                    if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                    {
                        PageCoolDown = Time.time;
                        Mods[Mods.ElementAt(counter).Key] = !Mods.ElementAt(counter).Value;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                    }
                }else huntText.text = Mods.ElementAt(counter).Key;

                #region Platform
                if (Mods[Mods.ElementAt(1).Key])
                {
                    Vector3 leftOffset = new Vector3(0f, -0.06f, 0f);

                    Vector3 rightOffset = new Vector3(0f, -0.06f, 0f);

                    Color playerColor = GorillaTagger.Instance.offlineVRRig.mainSkin.material.color;

                    if (ControllerInputPoller.instance.leftGrab)
                    {
                        if (leftplat == null)
                        {
                            leftplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            leftplat.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                            leftplat.transform.position = GorillaTagger.Instance.leftHandTransform.position + leftOffset;
                            leftplat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                            leftplat.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                            leftplat.GetComponent<Renderer>().material.color = playerColor;
                        }
                    }
                    else
                    {
                        if (leftplat != null)
                        {
                            GameObject.Destroy(leftplat, .2f);
                            leftplat = null;
                        }
                    }

                    if (ControllerInputPoller.instance.rightGrab)
                    {
                        if (rightplat == null)
                        {
                            rightplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            rightplat.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                            rightplat.transform.position = GorillaTagger.Instance.rightHandTransform.position + rightOffset;
                            rightplat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                            rightplat.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                            rightplat.GetComponent<Renderer>().material.color = playerColor;
                        }
                    }
                    else
                    {
                        if (rightplat != null)
                        {
                            GameObject.Destroy(rightplat, .2f);
                            rightplat = null;
                        }
                    }
                }else
                {
                    GameObject.Destroy(leftplat);
                    GameObject.Destroy(rightplat);
                }
                #endregion

                #region Frozone
                if (Mods[Mods.ElementAt(2).Key])
                {
                    Vector3 leftOffset = new Vector3(0f, -0.06f, 0f);
                    Vector3 rightOffset = new Vector3(0f, -0.06f, 0f);

                    if (ControllerInputPoller.instance.leftGrab)
                    {
                        Frozone = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        Frozone.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position + leftOffset;
                        Frozone.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                        Frozone.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                        Frozone.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        Frozone.GetComponent<Renderer>().material.color = Color.cyan;
                        Frozone.AddComponent<GorillaSurfaceOverride>().overrideIndex = 61;
                        GameObject.Destroy(Frozone.GetComponent<Rigidbody>());
                        GameObject.Destroy(Frozone, .2f);
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(AddForceStuff);
                    }

                    if (ControllerInputPoller.instance.rightGrab)
                    {
                        FrozoneR = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        FrozoneR.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position + leftOffset;
                        FrozoneR.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                        FrozoneR.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                        FrozoneR.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        FrozoneR.GetComponent<Renderer>().material.color = Color.cyan;
                        FrozoneR.AddComponent<GorillaSurfaceOverride>().overrideIndex = 61;
                        GameObject.Destroy(FrozoneR.GetComponent<Rigidbody>());
                        GameObject.Destroy(FrozoneR, .2f);
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(AddForceStuff);
                    }
                }else
                {
                    GameObject.Destroy(FrozoneR);
                    GameObject.Destroy(Frozone);
                }
                #endregion

                #region Drawing
                if (Mods[Mods.ElementAt(3).Key])
                {
                    if (ControllerInputPoller.instance.leftGrab)
                    {
                        DrawL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        DrawL.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                        DrawL.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                        DrawL.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        DrawL.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        DrawL.GetComponent<Renderer>().material.color = Color.black;
                        GameObject.Destroy(DrawL.GetComponent<SphereCollider>());
                        GameObject.Destroy(DrawL, 10f);
                    }

                    if (ControllerInputPoller.instance.rightGrab)
                    {
                        DrawR = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        DrawR.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                        DrawR.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                        DrawR.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        DrawR.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        DrawR.GetComponent<Renderer>().material.color = Color.cyan;
                        GameObject.Destroy(DrawR.GetComponent<Rigidbody>());
                        GameObject.Destroy(DrawR.GetComponent<SphereCollider>());
                        GameObject.Destroy(DrawR, 10f);
                    }
                }
                #endregion

                #region NoClip
                if (Mods[Mods.ElementAt(4).Key])
                {
                    MeshCollider[] array = Resources.FindObjectsOfTypeAll<MeshCollider>();
                    foreach (MeshCollider meshCollider in array)
                    {
                        meshCollider.enabled = false;
                    }
                }
                else
                {
                    MeshCollider[] array3 = Resources.FindObjectsOfTypeAll<MeshCollider>();
                    foreach (MeshCollider meshCollider2 in array3)
                    {
                        meshCollider2.enabled = true;
                    }
                }
                #endregion

                #region HoverMonke
                if (Mods[Mods.ElementAt(5).Key])
                {
                    if (ControllerInputPoller.instance.leftControllerPrimaryButton)
                    {
                        GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 1400f;
                    }
                }
                #endregion

                #region VelocityFly
                if (Mods[Mods.ElementAt(6).Key])
                {
                    if (ControllerInputPoller.instance.leftControllerGripFloat > .5)
                    {
                        GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(10 * GorillaTagger.Instance.offlineVRRig.transform.Find("rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L").right, ForceMode.Acceleration);
                        GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 50f * GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.magnitude, GorillaTagger.Instance.tapHapticDuration);
                    }
                    if (ControllerInputPoller.instance.rightControllerGripFloat > .5)
                    {
                        GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(10 * -GorillaTagger.Instance.offlineVRRig.transform.Find("rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").right, ForceMode.Acceleration);
                        GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 50f * GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.magnitude, GorillaTagger.Instance.tapHapticDuration);
                    }
                }
                #endregion

                #region High Gravity
                if (Mods[Mods.ElementAt(7).Key])
                {
                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.down * (Time.deltaTime * (7.77f / Time.deltaTime)), ForceMode.Acceleration);
                }
                #endregion

                #region Low Gravity
                if (Mods[Mods.ElementAt(8).Key])
                {
                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * 10f, ForceMode.Acceleration);
                }
                #endregion

                #region No Gravity
                if (Mods[Mods.ElementAt(9).Key])
                {
                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (6.66f / Time.deltaTime)), ForceMode.Acceleration);
                }
                #endregion

                #region BigMonk
                if (Mods[Mods.ElementAt(10).Key])
                {
                    GorillaLocomotion.Player.Instance.scale = 2f;
                }
                #endregion

                #region SmallMonk
                if (Mods[Mods.ElementAt(11).Key])
                {
                    GorillaLocomotion.Player.Instance.scale = .5f;
                }
                #endregion

                #region Monke Punch
                if (Mods[Mods.ElementAt(12).Key])
                {
                    int index = -1;
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            index++;

                            Vector3 they = vrrig.rightHandTransform.position;
                            Vector3 notthem = GorillaTagger.Instance.offlineVRRig.head.rigTarget.position;
                            float distance = Vector3.Distance(they, notthem);

                            if (distance < 0.25)
                            {
                                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity += Vector3.Normalize(vrrig.rightHandTransform.position - lastRight[index]) * 10f;
                            }
                            lastRight[index] = vrrig.rightHandTransform.position;

                            they = vrrig.leftHandTransform.position;
                            distance = Vector3.Distance(they, notthem);

                            if (distance < 0.25)
                            {
                                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity += Vector3.Normalize(vrrig.leftHandTransform.position - lastLeft[index]) * 10f;
                            }
                            lastLeft[index] = vrrig.leftHandTransform.position;
                        }
                    }
                }
                #endregion

                #region BouncyMonk
                if (Mods[Mods.ElementAt(13).Key])
                {
                    bounce = GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness;
                    PMCombine = GorillaLocomotion.Player.Instance.bodyCollider.material.bounceCombine;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounceCombine = PhysicMaterialCombine.Maximum;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness = 1.0f;
                }
                if (!Mods[Mods.ElementAt(13).Key])
                {
                    bounce = GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness;
                    PMCombine = GorillaLocomotion.Player.Instance.bodyCollider.material.bounceCombine;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounceCombine = PhysicMaterialCombine.Maximum;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness = 0f;
                }
                #endregion

                #region Swim
                if (Mods[Mods.ElementAt(14).Key])
                {
                    if (Swim == null)
                    {
                        Swim = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach/ForestToBeach_Prefab_V4/CaveWaterVolume"));
                        Swim.transform.localScale = new Vector3(5f, 5f, 5f);
                        Swim.GetComponent<Renderer>().enabled = false;
                    }
                    else
                    {
                        GorillaLocomotion.Player.Instance.audioManager.UnsetMixerSnapshot(0.1f);
                        Swim.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 2.5f, 0f);
                    }
                }
                else if (Swim != null)
                {
                    UnityEngine.Object.Destroy(Swim);
                }
                #endregion
            }
        }

        bool inModded()
        {
            if (PhotonNetwork.InRoom)
            {
                return PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString().Contains("MODDED");
            }
            return false;
        }
    }
}

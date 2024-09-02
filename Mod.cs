using BepInEx;
using Photon.Pun;
using UnityEngine;

namespace TheGorillaWatch
{
    [BepInPlugin("com.ArtificialGorillas.gorillatag.GorillaWatch", "GorillaWatch", "1.3.1")]
    public class Mod : BaseUnityPlugin
    {
        bool inRoom;

        public static int counter;

        public static float PageCoolDown;

        public static int layer = 29, layerMask = 1 << layer;

        private LayerMask baseMask;

        float bounce;

        private bool toggleWatch = true;

        PhysicMaterialCombine PMCombine;

        public static bool ToggleMod1;

        public static bool ToggleMod2;

        public static bool ToggleMod3;

        public static bool ToggleMod4;

        public static bool ToggleMod5;

        public static bool ToggleMod6;

        public static bool ToggleMod7;

        public static bool ToggleMod8;

        public static bool ToggleMod9;

        public static bool ToggleMod10;

        public static bool ToggleMod11;

        public static bool ToggleMod12;

        public static bool ToggleMod13;

        public static bool ToggleMod14;

        public static bool ToggleMod15;

        public static bool ToggleMod16;

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

        void Update()
        {
            inRoom = inModded();
            if (inRoom)
            {
                GorillaHuntComputer huntComputer = GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>();
                if (!doneDeletion)
                {
                    GorillaTagger.Instance.offlineVRRig.EnableHuntWatch(true);
                    huntComputer.enabled = false;
                    foreach(GameObject obj in huntComputer.transform.Find("HuntWatch_ScreenLocal/Canvas/Anchor"))
                    {
                        if (obj.name != "Text") GameObject.Destroy(obj);
                    }
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
                if (ControllerInputPoller.instance.leftControllerSecondaryButton && Time.time > PageCoolDown + 0.5f)
                {
                    toggleWatch = !toggleWatch;
                    PageCoolDown = Time.time;

                    huntComputer.gameObject.SetActive(!toggleWatch);
                }
                if (counter < 0) counter = 14;

                if (counter > 14) counter = 0;

                switch (counter)
                {
                    case 0:
                        huntComputer.text.text = "GorillaWatch!\nMade by:\nARTIFICIALGORILLAS";
                        break;

                    case 1:
                        huntComputer.text.text = "PlatformGuy--" + ToggleMod1.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod1 = !ToggleMod1;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("You Have Platforms");
                        }
                        break;

                    case 2:
                        huntComputer.text.text = "FrozoneGuy--" + ToggleMod12.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod12 = !ToggleMod12;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Frozone!");
                        }
                        break;
                    
                    case 3:
                        huntComputer.text.text = "DrawingGuy-- " + ToggleMod13.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod13 = !ToggleMod13;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Now, you can draw!");
                        }
                        break;

                    case 4:
                        huntComputer.text.text = "NoClip-- " + ToggleMod8.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod8 = !ToggleMod8;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Noclip, Go through the map!");
                        }
                        break;

                    case 5:
                        huntComputer.text.text = "HoverMonke-- " + ToggleMod6.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            ToggleMod6 = !ToggleMod6;
                            Debug.Log("You can hover now!");
                        }
                        break;

                    case 6:
                        huntComputer.text.text = "VelocityFly-- " + ToggleMod2.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod2 = !ToggleMod2;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("FLY");
                        }
                        break;

                    case 7:
                        huntComputer.text.text = "HighGravity-- " + ToggleMod11.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod11 = !ToggleMod11;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Woah! Are we on da sun? high gravity");
                        }
                        break;

                    case 8:
                        huntComputer.text.text = "LowGravity-- " + ToggleMod10.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod10 = !ToggleMod10;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Moon Grav");
                        }
                        break;

                    case 9:
                        huntComputer.text.text = "NoGravity-- " + ToggleMod5.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod5 = !ToggleMod5;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("No GRAVITY");
                        }
                        break;

                    case 10:
                        huntComputer.text.text = "BigMonkers---" + ToggleMod3.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod3 = !ToggleMod3;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Become BIGGA");
                        }
                        break;

                    case 11:
                        huntComputer.text.text = "SmallMonkers--" + ToggleMod4.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod4 = !ToggleMod4;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Become SMALLA");
                        }
                        break;

                    case 12:
                        huntComputer.text.text = "MonkePunch-- " + ToggleMod15.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod15 = !ToggleMod15;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Swim Everywhere");
                        }
                        break;

                    case 13:
                        huntComputer.text.text = "MonkeBoing-- " + ToggleMod9.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod9 = !ToggleMod9;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Go BOING BOING BOING");
                        }
                        break;

                    case 14:
                        huntComputer.text.text = "AirSwim-- " + ToggleMod14.ToString();
                        if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time > PageCoolDown + .5)
                        {
                            PageCoolDown = Time.time;
                            ToggleMod14 = !ToggleMod14;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                            Debug.Log("Swim Everywhere");
                        }
                        break;
                }
                

                if (ToggleMod1)
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
                }
                if (!ToggleMod1)
                {
                    GameObject.Destroy(leftplat);
                    GameObject.Destroy(rightplat);
                }
                if (ToggleMod12)
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
                }
                if (!ToggleMod12)
                {
                    GameObject.Destroy(FrozoneR);
                    GameObject.Destroy(Frozone);
                }
                if (ToggleMod13)
                {
                    if (ControllerInputPoller.instance.leftGrab)
                    {
                        DrawL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        DrawL.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                        DrawL.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                        DrawL.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        DrawL.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        DrawL.GetComponent<Renderer>().material.color = Color.black;
                        GameObject.Destroy(DrawL.GetComponent<Rigidbody>());
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
                if (ToggleMod6)
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
                if (ToggleMod2)
                {
                    if (ControllerInputPoller.instance.leftControllerPrimaryButton)
                    {
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 1400f;
                    }
                }
                if (ToggleMod5)
                {
                    GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Acceleration);
                }
                if (ToggleMod3)
                {
                    GorillaLocomotion.Player.Instance.scale = 2f;
                }
                if (ToggleMod4)
                {
                    GorillaLocomotion.Player.Instance.scale = .5f;
                }
                if (ToggleMod8)
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
                if (ToggleMod9)
                {
                    bounce = GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness;
                    PMCombine = GorillaLocomotion.Player.Instance.bodyCollider.material.bounceCombine;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounceCombine = PhysicMaterialCombine.Maximum;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness = 1.0f;
                }
                if (!ToggleMod9)
                {
                    bounce = GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness;
                    PMCombine = GorillaLocomotion.Player.Instance.bodyCollider.material.bounceCombine;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounceCombine = PhysicMaterialCombine.Maximum;
                    GorillaLocomotion.Player.Instance.bodyCollider.material.bounciness = 0f;
                }
                if (ToggleMod10)
                {
                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (6.66f / Time.deltaTime)), ForceMode.Acceleration);
                }
                if (ToggleMod11)
                {
                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.down * (Time.deltaTime * (7.77f / Time.deltaTime)), ForceMode.Acceleration);
                }
                if (ToggleMod14)
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
                else
                {
                    if (Swim != null)
                    {
                        UnityEngine.Object.Destroy(Swim);
                    }
                }
                if (ToggleMod15)
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

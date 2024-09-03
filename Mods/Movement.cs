﻿using UnityEngine;

namespace EveWatch.Mods
{
    public class Movement
    {
        #region Platforms
        static GameObject leftPlat,
                   rightPlat;

        public static void Platforms()
        {
            Vector3 leftOffset = new Vector3(0f, -0.06f, 0f);

            Vector3 rightOffset = new Vector3(0f, -0.06f, 0f);

            Color playerColor = GorillaTagger.Instance.offlineVRRig.mainSkin.material.color;

            if (ControllerInputPoller.instance.leftGrab)
            {
                if (leftPlat == null)
                {
                    leftPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftPlat.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                    leftPlat.transform.position = GorillaTagger.Instance.leftHandTransform.position + leftOffset;
                    leftPlat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                    leftPlat.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                    leftPlat.GetComponent<Renderer>().material.color = playerColor;
                }
            }
            else
            {
                if (leftPlat != null)
                {
                    GameObject.Destroy(leftPlat, .2f);
                    leftPlat = null;
                }
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                if (rightPlat == null)
                {
                    rightPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightPlat.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                    rightPlat.transform.position = GorillaTagger.Instance.rightHandTransform.position + rightOffset;
                    rightPlat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    rightPlat.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                    rightPlat.GetComponent<Renderer>().material.color = playerColor;
                }
            }
            else
            {
                if (rightPlat != null)
                {
                    GameObject.Destroy(rightPlat, .2f);
                    rightPlat = null;
                }
            }
        }
        public static void OnPlatformDisable()
        {
            if (leftPlat) GameObject.Destroy(leftPlat);
            if (rightPlat) GameObject.Destroy(rightPlat);
        }
        #endregion

        #region Frozone
        static GameObject 
            FrozoneL,
            FrozoneR;

        public static void Frozone()
        {
            Vector3 offset = new Vector3(0f, -0.06f, 0f);

            if (ControllerInputPoller.instance.leftGrab)
            {
                FrozoneL = GameObject.CreatePrimitive(PrimitiveType.Cube);
                FrozoneL.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position + offset;
                FrozoneL.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                FrozoneL.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                FrozoneL.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                FrozoneL.GetComponent<Renderer>().material.color = Color.cyan;
                FrozoneL.AddComponent<GorillaSurfaceOverride>().overrideIndex = 61;
                GameObject.Destroy(FrozoneL.GetComponent<Rigidbody>());
                GameObject.Destroy(FrozoneL, .2f);
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 40f, 0f));
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                FrozoneR = GameObject.CreatePrimitive(PrimitiveType.Cube);
                FrozoneR.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position + offset;
                FrozoneR.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                FrozoneR.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                FrozoneR.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                FrozoneR.GetComponent<Renderer>().material.color = Color.cyan;
                FrozoneR.AddComponent<GorillaSurfaceOverride>().overrideIndex = 61;
                GameObject.Destroy(FrozoneR.GetComponent<Rigidbody>());
                GameObject.Destroy(FrozoneR, .2f);
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 40f, 0f));
            }
        }
        #endregion

        #region Noclip
        public static void Noclip()
        {
            MeshCollider[] array = Resources.FindObjectsOfTypeAll<MeshCollider>();
            foreach (MeshCollider meshCollider in array)
            {
                meshCollider.enabled = false;
            }
        }

        public static void NoclipDisable()
        {
            MeshCollider[] array = Resources.FindObjectsOfTypeAll<MeshCollider>();
            foreach (MeshCollider meshCollider in array)
            {
                meshCollider.enabled = true;
            }
        }
        #endregion

        #region Flight
        public static void Fly()
        {
            if (ControllerInputPoller.instance.leftControllerPrimaryButton)
            {
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 1400f;
            }
        }
        #endregion

        #region Iron Monk
        public static void IronMonk()
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
    }
}
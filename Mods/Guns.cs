using EveWatch.Librarys;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EveWatch.Mods
{
    public class Guns
    {
        #region Tp Gun
        static bool lastTriggedTP;
        public static void TpGun()
        {
            GunLib.GunLibData gunLibData = GunLib.Shoot();

            if (gunLibData.isTriggered && gunLibData.isShooting && !lastTriggedTP)
            {
                Camera.main.transform.localPosition = Vector3.zero;
                Vector3 target = new Vector3(gunLibData.hitPosition.x, gunLibData.hitPosition.y, gunLibData.hitPosition.z);
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.transform.position = target;

                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().position = target;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;

                Traverse.Create(GorillaLocomotion.Player.Instance).Field("lastPosition").SetValue(target);
                Traverse.Create(GorillaLocomotion.Player.Instance).Field("lastLeftHandPosition").SetValue(target);
                Traverse.Create(GorillaLocomotion.Player.Instance).Field("lastRightHandPosition").SetValue(target);
                Traverse.Create(GorillaLocomotion.Player.Instance).Field("lastHeadPosition").SetValue(target);

                GorillaLocomotion.Player.Instance.leftControllerTransform.position = target;
                GorillaLocomotion.Player.Instance.rightControllerTransform.position = target;
            }
            lastTriggedTP = gunLibData.isTriggered;
        }
        #endregion

        #region Tp Gun
        static bool lastTriggedButton;
        public static void ButtonGun()
        {
            GunLib.GunLibData gunLibData = GunLib.Shoot();

            if (gunLibData.isTriggered && gunLibData.isShooting && !lastTriggedButton)
            {
                Camera.main.transform.localPosition = Vector3.zero;

                GorillaTagger.Instance.leftHandTriggerCollider.GetComponent<TransformFollow>().enabled = false;
                GorillaTagger.Instance.leftHandTriggerCollider.transform.position = gunLibData.hitPosition;
                GorillaTagger.Instance.leftHandTriggerCollider.GetComponent<TransformFollow>().enabled = true;
            }
            lastTriggedButton = gunLibData.isTriggered;
        }
        #endregion
    }
}

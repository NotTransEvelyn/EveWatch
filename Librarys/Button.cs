﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EveWatch.Librarys
{
    public class Button : MonoBehaviour
    {
        public float debounceTime = 0.25f;

        public float touchTime;

        void OnTriggerEnter(Collider collider)
        {
            if (!base.enabled || !(touchTime + debounceTime < Time.time) || collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
            {
                return;
            }
            touchTime = Time.time;
            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (!component.isLeftHand)
            {
                Main.Toggle();
            }
        }
    }
}
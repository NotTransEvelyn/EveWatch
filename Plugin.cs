using BepInEx;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using EveWatch.Mods;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace EveWatch
{
    [BepInPlugin("Eve.EveWatch", "EveWatch", "1.4.0")]
    public class Main : BaseUnityPlugin
    {
        static int counter;
        static float PageCoolDown;
        int modCount;
        static Dictionary<Mod, bool> Mods;

        bool doneDeletion;
        void Start()
        {
            Mods = new Dictionary<Mod, bool>()
            {
                { new Mod("Eve Watch!", "Triggers To\nSwitch Pages\nA To Toggle.", Empty, Empty, Empty), false },
                { new Mod("Platforms","Press grip to\nuse them!", Empty, Movement.Platforms, Movement.OnPlatformDisable), false },
                { new Mod("Frozone", "Press grip to\nspawn slip plats!", Empty, Movement.Frozone, Empty), false },
                { new Mod("Noclip", "Disables every\ncollider!\n(Plats suggested)", Movement.Noclip, Empty, Movement.NoclipDisable), false },
                { new Mod("Flight", "Press X to\nfly!", Empty, Movement.Fly, Empty), false },
                { new Mod("Iron Monk", "Press grip to\nfly like iron\nman!", Empty, Movement.IronMonk, Empty), false },
                { new Mod("Air Swim", "Swim\neverywhere!", Empty, Movement.Swim, Movement.StopSwim), false },
            };
            modCount = Mods.Count - 1;
        }
        GorillaHuntComputer huntComputer;
        Text huntText;
        void Update()
        {
            huntComputer = GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>();
            if (InModded())
            {
                if (!doneDeletion)
                {
                    huntText = huntComputer.text;
                    huntText.transform.localPosition = new Vector3(0.023f, 0.0004f, 0);
                    huntText.transform.localScale = new Vector3(0.0006f, 0.0006f, 0.0006f);
                    huntText.rectTransform.sizeDelta = new Vector2(160f, 60f);
                    huntComputer.enabled = false;
                    Destroy(huntComputer.badge);
                    Destroy(huntComputer.leftHand);
                    Destroy(huntComputer.rightHand);
                    Destroy(huntComputer.hat);
                    Destroy(huntComputer.face);
                    Material mat = new Material(huntComputer.material.material);
                    huntComputer.material.material = mat;
                    huntComputer.material.transform.localPosition = new Vector3(0.0197f, -0.0096f, 0);
                    foreach(Transform obj in huntComputer.material.transform.parent)
                    {
                        if (obj.name != "Text" && obj.name != "Material") GameObject.Destroy(obj.gameObject);
                    }
                    Debug.Log("EveWatch Has Loaded Successfully");
                    doneDeletion = true;
                }
                huntComputer.gameObject.SetActive(true);

                if ((ControllerInputPoller.instance.rightControllerIndexFloat >= .5f || Keyboard.current.rightArrowKey.isPressed) && Time.time > PageCoolDown + 0.5)
                {
                    PageCoolDown = Time.time;
                    counter++;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, true, 1f);
                }
                if ((ControllerInputPoller.instance.leftControllerIndexFloat >= .5f || Keyboard.current.leftArrowKey.isPressed) && Time.time > PageCoolDown + 0.5)
                {
                    PageCoolDown = Time.time;
                    counter--;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, true, 1f);
                }

                if (counter < 0) counter = modCount;
                if (counter > modCount) counter = 0;

                if (counter != 0)
                {
                    huntText.text = $"{Mods.ElementAt(counter).Key.Name} ({counter}/{modCount})\n{Mods.ElementAt(counter).Key.Desc}".ToUpper();
                    if ((ControllerInputPoller.instance.rightControllerPrimaryButton || Keyboard.current.enterKey.isPressed) && Time.time > PageCoolDown + .5)
                    {
                        PageCoolDown = Time.time;
                        Mods[Mods.ElementAt(counter).Key] = !Mods.ElementAt(counter).Value;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                        if (Mods[Mods.ElementAt(counter).Key]) Mods.ElementAt(counter).Key.OnEnabledMethod();
                        else Mods.ElementAt(counter).Key.OnDisabledMethod();
                    }
                }else huntText.text = Mods.ElementAt(counter).Key.Name + "\n" + Mods.ElementAt(counter).Key.Desc;

                if (Mods.ElementAt(counter).Value) huntComputer.material.material.color = Color.green;
                else huntComputer.material.material.color = new Color(1, 0, 0, 255);

                foreach (var modInfo in Mods)
                {
                    if (modInfo.Value == true)
                    {
                        modInfo.Key.StayEnabledMethod();
                    }
                }
            }else huntComputer.gameObject.SetActive(false);
        }
        void Empty(){} //Used for the mods and if you want them to be empty!

        bool InModded()
        {
            //if (PhotonNetwork.InRoom)
            //{
            //    return PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString().Contains("MODDED");
            //}
            //return false;
            return true;
        }
    }

    public class Mod
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public Action OnEnabledMethod { get; set; }
        public Action StayEnabledMethod { get; set; }
        public Action OnDisabledMethod { get; set; }

        public Mod(string name, string desc, Action onEnabled, Action stayEnabled, Action onDisabled)
        {
            Name = name;
            Desc = desc;
            OnEnabledMethod = onEnabled;
            StayEnabledMethod = stayEnabled;
            OnDisabledMethod = onDisabled;
        }
    }
}
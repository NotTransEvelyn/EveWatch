using BepInEx;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using TheGorillaWatch.Mods;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TheGorillaWatch
{
    [BepInPlugin("ArtificialGorillas.GorillaWatch", "GorillaWatch", "1.4.0")]
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
                { new Mod("Gorilla Watch!", "Triggers To Switch Pages\nA To Toggle.", Empty, Empty, Empty), false },
                { new Mod("Platforms","Press grip to use them!", Empty, Movement.Platforms, Movement.OnPlatformDisable), false },
                { new Mod("Frozone", "Press grip to spawn slip plats!", Empty, Movement.Frozone, Empty), false },
                { new Mod("Drawing", "Press grip to draw!", Empty, Movement.Drawing, Empty), false },
                { new Mod("Noclip", "Disables every collider!\n(Plats suggested)", Movement.Noclip, Empty, Movement.NoclipDisable), false },
                { new Mod("Flight", "Press X to fly!", Empty, Movement.Fly, Empty), false },
                { new Mod("Iron Monk", "Press GRIP to fly like iron man!", Empty, Movement.IronMonk, Empty), false },
                { new Mod("High Gravity", "Makes you have higher gravity!", Empty, Gravity.HighGravity, Gravity.ResetGravity), false },
                { new Mod("Low Gravity", "Makes you have lower gravity!", Empty, Gravity.LowGravity, Gravity.ResetGravity), false },
                { new Mod("No Gravity", "Makes you have no gravity!", Empty, Gravity.NoGravity, Gravity.ResetGravity), false },
                { new Mod("Big Monk", "Makes your monke bigger!", Empty, Size.BigMonke, Empty), false },
                { new Mod("Small Monk", "Makes your monke smaller!", Empty, Size.SmallMonke, Empty), false },
                { new Mod("Monke Punch", "Lets you be punched around by other players!", Empty, Multiplayer.Punch, Empty), false },
                { new Mod("Monke Boing", "Makes the ground bouncy!", Movement.Bounce, Empty, Movement.StopBounce), false },
                { new Mod("Air Swim", "Swim everywhere!", Empty, Movement.Swim, Movement.StopSwim), false },
            };
            modCount = Mods.Count;
        }
        GorillaHuntComputer huntComputer;
        Text huntText;
        void Update()
        {
            if (InModded())
            {
                if (!doneDeletion)
                {
                    huntComputer = GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>();
                    huntText = huntComputer.text;
                    huntComputer.enabled = false;
                    Destroy(huntComputer.material);
                    Destroy(huntComputer.badge);
                    Destroy(huntComputer.leftHand);
                    Destroy(huntComputer.rightHand);
                    Destroy(huntComputer.hat);
                    Destroy(huntComputer.face);
                    Debug.Log("GorillaWatch Has Loaded Successfully");
                    doneDeletion = true;
                }
                huntComputer.gameObject.SetActive(true);

                if ((ControllerInputPoller.instance.rightControllerIndexFloat >= .5f || Keyboard.current.rightArrowKey.wasPressedThisFrame) && Time.time > PageCoolDown + 0.5)
                {
                    PageCoolDown = Time.time;
                    counter++;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, true, 1f);
                }
                if ((ControllerInputPoller.instance.leftControllerIndexFloat >= .5f || Keyboard.current.leftArrowKey.wasPressedThisFrame) && Time.time > PageCoolDown + 0.5)
                {
                    PageCoolDown = Time.time;
                    counter--;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, true, 1f);
                }

                if (counter < 0) counter = modCount;
                if (counter > modCount) counter = 0;

                if (counter != 0)
                {
                    huntText.text = $"{Mods.ElementAt(counter).Key.Name}:\n{Mods.ElementAt(counter).Value}.\n{Mods.ElementAt(counter).Key.Desc}".ToUpper();
                    if ((ControllerInputPoller.instance.rightControllerPrimaryButton || Keyboard.current.enterKey.wasPressedThisFrame) && Time.time > PageCoolDown + .5)
                    {
                        PageCoolDown = Time.time;
                        Mods[Mods.ElementAt(counter).Key] = !Mods.ElementAt(counter).Value;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(69, true, 1f);
                        if (Mods[Mods.ElementAt(counter).Key]) Mods.ElementAt(counter).Key.OnEnabledMethod();
                        else Mods.ElementAt(counter).Key.OnDisabledMethod();
                    }
                }else huntText.text = Mods.ElementAt(counter).Key.Name + "\n" + Mods.ElementAt(counter).Key.Desc;

                foreach(var modInfo in Mods)
                {
                    if (modInfo.Value == true)
                    {
                        modInfo.Key.StayEnabledMethod();
                    }
                }
            }else huntComputer.gameObject.SetActive(true);
        }
        void Empty(){} //Used for the mods and if you want them to be empty!

        bool InModded()
        {
            if (PhotonNetwork.InRoom)
            {
                return PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString().Contains("MODDED");
            }
            return false;
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
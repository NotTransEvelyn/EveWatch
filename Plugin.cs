using BepInEx;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using EveWatch.Mods;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Net;
using TMPro;
using static Photon.Pun.UtilityScripts.TabViewManager;
using ExitGames.Client.Photon;

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
            Movement.SwitchBoostType(true);
            Infection.SwitchTagType(true);
            Mods = new Dictionary<Mod, bool>()
            {
                //Title
                { new Mod("Eve Watch!", "Welcome to\nEveWatch! Look at\nthe CoC board\nfor the controls!", Empty, Empty, Empty), false},

                //Room
                { new Mod("Disconnect","Makes you leave\nthe lobby!", ()=>NetworkSystem.Instance.ReturnToSinglePlayer(), Empty, Empty, true), false },

                //Movement
                { new Mod("Platforms","Press grip to\nuse them!", Empty, Movement.Platforms, Movement.OnPlatformDisable), false },
                { new Mod("Speed Boost", $"Type: {Movement.CurrentSpeedName}\nGives you a\nlittle boost\nin speed!", Empty, Movement.SpeedBoost, Movement.DisableSpeedBoost), false },
                { new Mod("No Tag Freeze", "Just no tag\nfreeze pretty easy\nto understand.", Empty, ()=>GorillaLocomotion.Player.Instance.disableMovement = false, Empty), false },

                //Infection
                { new Mod("Tag Aura", $"Type: {Infection.CurrentTagAuraName}\nLets you tag\npeople easier!", Empty, ()=>Infection.TagAura(), Empty), false },

                //Flights
                { new Mod("Flight", "Press A to\nfly!", Empty, Movement.Fly, Empty), false },
                { new Mod("Iron Monk", "Press grip to\nfly like iron\nman!", Empty, Movement.IronMonk, Empty), false },

                //Visual
                { new Mod("Tracers", "Tracers to every\nmonke!\nGreen = Untagged\nRed = Tagged", Visual.Tracers, Empty, Visual.DisableTracers), false },
                { new Mod("Box ESP", "Boxes around every\nmonke!\nGreen = Untagged\nRed = Tagged", Visual.BoxESP, Empty, Visual.DisableBoxESP), false },
                { new Mod("Watch ESP", "Boxes around every\nEvewatch user!", Visual.WatchESP, Empty, Visual.DisableWatchESP), false },

                //Guns
                { new Mod("Tp Gun", "Teleport around\nwith a gun!", Empty, Guns.TpGun, Empty), false },
                { new Mod("Button Gun", "Press buttons\nwith a gun!", Empty, Guns.ButtonGun, Empty), false },

                //Settings
                { new Mod("Change Speed", $"Changes your speed\nboost, boost.\nType: {Movement.CurrentSpeedName}", ()=>Movement.SwitchBoostType(), Empty, Empty, true), false },
                { new Mod("Change Distance", $"Distance: {Infection.CurrentTagAuraName}\nChanges your\nTag Aura Distance", ()=>Infection.SwitchTagType(), Empty, Empty, true), false },
                { new Mod("Swap Theme","Changes the menus\ntheme!", Themes.SwitchTheme, Empty, Empty, true), false },
            };
            modCount = Mods.Count - 1;
        }
        public static GorillaHuntComputer huntComputer;
        Text huntText;
        bool lookedAtMainPage;
        bool lastY;
        bool hideAndLock;

        public static GameObject button;
        void Update()
        {
            huntComputer = GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>();
            if (InModded() && GorillaTagger.Instance.offlineVRRig != null)
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
                    button = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    button.transform.SetParent(huntComputer.material.transform, false);
                    button.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                    button.AddComponent<Librarys.Button>();
                    button.transform.localScale = new Vector3(12, 12, 10);
                    button.layer = 18;
                    button.GetComponent<BoxCollider>().isTrigger = true;
                    Destroy(button.GetComponent<Renderer>());
                    Destroy(button.GetComponent<MeshFilter>());

                    GameObject title = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/UI/CodeOfConduct_Group/CodeOfConduct");
                    title.GetComponent<TextMeshPro>().richText = true;
                    title.GetComponent<TextMeshPro>().text = "<color=#FF0000>E</color><color=#FFAA00>V</color><color=#AAFF00>E</color><color=#00FFAA>W</color><color=#00A9FF>A</color><color=#0000FF>T</color><color=#AA00FF>C</color><color=#FF00AA>H</color>";

                    GameObject desc = title.transform.GetChild(0).gameObject;
                    desc.GetComponent<TextMeshPro>().richText = true;
                    desc.GetComponent<TextMeshPro>().text = new WebClient().DownloadString("https://pastebin.com/raw/wErPZy4f").ToUpper();
                    new GameObject("EveWatch").AddComponent<Visual>();

                    Hashtable hash = new Hashtable()
                    {
                        {"EveWatch", true}
                    };

                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

                    Debug.Log("EveWatch Has Loaded Successfully");
                    doneDeletion = true;
                }

                if ((ControllerInputPoller.instance.leftControllerSecondaryButton && !lastY) || Keyboard.current.hKey.wasPressedThisFrame) hideAndLock = !hideAndLock;

                if (!hideAndLock)
                {
                    huntComputer.gameObject.SetActive(true);
                    if ((ControllerInputPoller.instance.leftControllerIndexFloat >= .5f || Keyboard.current.rightArrowKey.isPressed) && Time.time > PageCoolDown + 0.5)
                    {
                        PageCoolDown = Time.time;
                        counter++;
                        lookedAtMainPage = true;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, true, 1f);
                    }
                    if ((ControllerInputPoller.instance.leftControllerGripFloat >= .5f || Keyboard.current.leftArrowKey.isPressed) && Time.time > PageCoolDown + 0.5)
                    {
                        PageCoolDown = Time.time;
                        counter--;
                        lookedAtMainPage = true;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, true, 1f);
                    }
                    if (counter < (lookedAtMainPage ? 1 : 0)) counter = modCount;
                    if (counter > modCount) counter = (lookedAtMainPage ? 1 : 0);

                    if (counter != 0)
                    {
                        huntText.text = $"{Mods.ElementAt(counter).Key.Name} ({counter}/{modCount})\n{Mods.ElementAt(counter).Key.Desc}".ToUpper();
                        if ((ControllerInputPoller.instance.leftControllerPrimaryButton || Keyboard.current.enterKey.isPressed) && Time.time > PageCoolDown + .5)
                        {
                            Toggle();
                        }
                    }
                    else huntText.text = Mods.ElementAt(counter).Key.Name + "\n" + Mods.ElementAt(counter).Key.Desc;

                    if (counter == 0) huntComputer.material.enabled = false;
                    else huntComputer.material.enabled = true;

                    if (Mods.ElementAt(counter).Value) huntComputer.material.material.color = Color.green;
                    else huntComputer.material.material.color = new Color(1, 0, 0, 255);
                }
                else
                {
                    huntComputer.gameObject.SetActive(false);
                }

                foreach (var modInfo in Mods)
                {
                    if (modInfo.Value == true)
                    {
                        modInfo.Key.StayEnabledMethod();
                    }
                }

                lastY = ControllerInputPoller.instance.leftControllerSecondaryButton;
            }else huntComputer.gameObject.SetActive(false);
        }
        void Empty(){} //Used for the mods and if you want them to be empty!

        public static void Toggle()
        {
            PageCoolDown = Time.time;
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, true, 1f);
            if (Mods.ElementAt(counter).Key.Toggle)
            {
                Mods.ElementAt(counter).Key.OnEnabledMethod();
                return;
            }
            Mods[Mods.ElementAt(counter).Key] = !Mods.ElementAt(counter).Value;
            if (Mods[Mods.ElementAt(counter).Key]) Mods.ElementAt(counter).Key.OnEnabledMethod();
            else Mods.ElementAt(counter).Key.OnDisabledMethod();
        }

        void OnGUI()
        {
            if (!hideAndLock)
            {
                GUIStyle style = new GUIStyle();
                style.font = huntText.font;
                GUI.Label(new Rect(0, 0, 200000000, 20000000), huntText.text, style);
            }
        }

        public static Mod GetMod(string name)
        {
            foreach(Mod mod in Mods.Keys)
            {
                if (mod.Name == name) return mod;
            }
            return null;
        }

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

        public bool Toggle { get; set; }

        public Mod(string name, string desc, Action onEnabled, Action stayEnabled, Action onDisabled, bool toggle = false)
        {
            Name = name;
            Desc = desc;
            OnEnabledMethod = onEnabled;
            StayEnabledMethod = stayEnabled;
            OnDisabledMethod = onDisabled;
            Toggle = toggle;
        }
    }
}
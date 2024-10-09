using BepInEx;
using EveWatch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MonkeNotificationLib
{
    internal class TextEffect : MonoBehaviour
    {
        public float Delay;

        private Text textObject;
        private Color color { get => textObject.color; set => textObject.color = value; }

        private void Start()
        {
            textObject = GetComponent<Text>();
            StartCoroutine(DelayedFade());
        }

        private IEnumerator DelayedFade()
        {
            yield return new WaitForSeconds(Delay);

            while (color.a > 0)
            {
                color = new Color(color.r, color.g, color.b, color.a - 0.01f);
                yield return new WaitForSeconds(0.1f);
            }
            gameObject.SetActive(false);
            Destroy(this);
        }
    }

    internal class NotificationManager
    {
        public static NotificationManager Instance;
        private static bool initialized;

        public GameObject ConsoleCanvasObject;
        public GameObject ConsoleLinePrefab;

        private int availableLines => linePool.Count(x => !x.gameObject.activeSelf);
        private List<Text> linePool = new List<Text>();

        public NotificationManager()
        {
            Instance = this;
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("EveWatch.Resources.console"))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromStream(stream);

                ConsoleCanvasObject = GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("ConsoleCanvas"));
                Transform consoleTransform = ConsoleCanvasObject.transform;
                consoleTransform.SetParent(GorillaTagger.Instance.offlineVRRig.transform.Find("RigAnchor/rig/body/head/").transform);
                consoleTransform.localPosition = new Vector3(-0.816f, -0.157f, 1.604f);
                consoleTransform.localRotation = Quaternion.Euler(-0.816f, -0.057f, 1.304f);

                ConsoleLinePrefab = consoleTransform.GetChild(0).gameObject;
                Text prefabText = ConsoleLinePrefab.GetComponent<Text>();
                Material newMaterial = GameObject.Instantiate(prefabText.material);
                newMaterial.shader = Shader.Find("GUI/Text Shader");
                prefabText.material = newMaterial;

                ConsoleLinePrefab.SetActive(false);

                const int linePoolAmount = 750;
                for (int i = 0; i < linePoolAmount; i++) AddLineToPool();
                assetBundle.Unload(false);
            }
            initialized = true;
        }

        private void AddLineToPool()
        {
            var newLine = GameObject.Instantiate(ConsoleLinePrefab, ConsoleCanvasObject.transform);
            linePool.Add(newLine.GetComponent<Text>());
        }

        public Text NewLine(string text, float fadeOutDelay = 3)
        {
            if (!initialized) return null;
            if (availableLines == 0)
            {
                AddLineToPool();
            }

            Text newLine = linePool.First(x => !x.gameObject.activeSelf);
            newLine.text = text;
            newLine.color = Color.white;
            GameObject newLineObject = newLine.gameObject;
            newLineObject.AddComponent<TextEffect>().Delay = fadeOutDelay;
            newLineObject.SetActive(true);
            newLineObject.transform.SetAsFirstSibling();

            return newLine;
        }
    }
    public static class NotificationController
    {
        /// <summary>
        /// Pulls a text object from the object pool and sets the text to '[{timestamp : {source}] {message}'
        /// </summary>
        /// <param name="includeTimeStamp">Default false | Should there be a timestamp?</param>
        /// <param name="fadeOutDelay">How long should the text stay on screen before it begins to fade out?</param>
        /// <returns>The text that was pulled from the pool. If null the text wasn't shown.</returns>
        public static Text AppendMessage(string source, string message, bool includeTimeStamp = false, float fadeOutDelay = 3)
        {
            string timeStampt = includeTimeStamp ? $"[{System.DateTime.Now.ToString("hh:mm:ss")} : " : "";
            string messageFormat = $"<b>[{timeStampt}{source}]</b> {message}";
            return NotificationManager.Instance?.NewLine(messageFormat, fadeOutDelay);
        }
        /// <summary>
        /// Pulls a text object from the object pool and sets the text to your text.
        /// </summary>
        /// <param name="fadeOutDelay">How long should the text stay on screen before it begins to fade out?</param>
        /// <returns>The text that was pulled from the pool. If null the text wasn't shown.</returns>
        public static Text AppendMessage(string message, float fadeOutDelay) =>
            NotificationManager.Instance?.NewLine(message, fadeOutDelay);


        /* Extension methods */

        /// <summary>Wraps your string in <color="color"></color>. Read the methods code from the Github page to see the presets. 
        /// (may add more later)</summary>
        public static string WrapColor(this string str, string color)
        {
            // Also I know Unity has rich text colors, but I perfer these colors over the presets.
            colorMapDictionary.TryGetValue(color.ToLower(), out color);
            return $"<color=#{color}>{str}</color>";
        }

        private static Dictionary<string, string> colorMapDictionary = new Dictionary<string, string>()
            {
                { "green", "09ff00" },
                { "red", "ff0800" },
                { "gray", "ffffff50" },
                { "warning", "f0ad4e" },
                { "danger", "d9534f" }
            };
    }

}
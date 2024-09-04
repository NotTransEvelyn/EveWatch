using System.Collections.Generic;
using UnityEngine;

namespace EveWatch.Mods
{
    public class Themes
    {
        #region Change Theme
        static int currentThemeIndex;
        static List< Color> currentThemes =  new List<Color>
        {
            new Color(1, 1, 1, 1), //Normal
            new Color(1, 0, 0, 1), //Red, Black Background
            new Color(1, 0, 1, 1), //Purple, Black Background
            new Color(0, 0, 1, 1), //Blue, Black Background
        };
        public static void SwitchTheme()
        {
            currentThemeIndex++;
            if (currentThemeIndex == currentThemes.Count) currentThemeIndex = 0;
            Main.huntComputer.transform.GetChild(1).GetComponent<Renderer>().sharedMaterial.color = currentThemes[currentThemeIndex];
        }
        #endregion
    }
}

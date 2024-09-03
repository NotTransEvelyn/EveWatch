using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EveWatch.Mods
{
    public class Gravity
    {
        #region Low Gravity
        public static void LowGravity() => Physics.gravity = new Vector3(0, -5, 0);
        #endregion

        #region High Gravity
        public static void HighGravity() => Physics.gravity = new Vector3(0, -19, 0);
        #endregion

        #region No Gravity
        public static void NoGravity() => Physics.gravity = Vector3.zero;
        #endregion

        public static void ResetGravity() => Physics.gravity = new Vector3(0, -9.8f, 0);
    }
}

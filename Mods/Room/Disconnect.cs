using System;
using System.Collections.Generic;
using System.Text;

namespace EveWatch.Mods.Room
{
    public class Disconnect : WatchMod
    {
        public override string Name() => "Disconnect";

        public override bool Toggle() => true;

        public override void ModEnabled()
        {
            NetworkSystem.Instance.ReturnToSinglePlayer();
        }
    }
}

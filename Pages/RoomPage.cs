using EveWatch.Mods.Room;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveWatch.Pages
{
    public class RoomPage : WatchPage
    {
        public override string PageName() => "Room";

        public override List<WatchMod> Mods => new List<WatchMod>()
        {
            new Disconnect()
        };
    }
}

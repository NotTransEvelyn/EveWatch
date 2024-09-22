using System.Collections.Generic;
using UnityEngine;
namespace EveWatch
{
    public abstract class WatchPage : MonoBehaviour
    {
        public abstract string PageName();
        public virtual List<WatchMod> Mods { get; } = new List<WatchMod>();
        public virtual List<WatchPage> Pages { get; } = new List<WatchPage>();
    }
}
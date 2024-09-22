using UnityEngine;
namespace EveWatch
{
    public abstract class WatchMod : MonoBehaviour
    {
        public abstract string Name();

        public abstract bool Toggle();

        public virtual void ModEnabled()
        {
        }
        public virtual void ModStayEnabled()
        {
        }
        public virtual void ModDisabled()
        {
        }
        public virtual void OnUpdate()
        {
        }

        void Update() => OnUpdate();
    }
}
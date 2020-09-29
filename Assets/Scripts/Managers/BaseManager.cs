using UnityEngine;

namespace Managers
{
    public abstract class BaseManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            RegisterToMaster(this);
        }

        private void RegisterToMaster(BaseManager baseManager)
        {
            Manager.Register(this);
        }
    }
}
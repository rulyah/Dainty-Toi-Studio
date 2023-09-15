using UnityEngine;

namespace Utils.FactoryTool
{
    public abstract class PoolableMonoBehaviour : MonoBehaviour
    {
        public virtual void Init() {}
        
        public virtual void Dispose() {}
    }
}
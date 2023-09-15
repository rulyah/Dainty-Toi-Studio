using System.Collections.Generic;
using UnityEngine;

namespace Utils.FactoryTool
{
    public class Factory<T> where T : PoolableMonoBehaviour
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly List<T> _cachedObjects;

        public Factory(T prefab, int preloadCount = 0)
        {
            _parent = new GameObject("[POOL]" + typeof(T).Name + "s").transform;
            
            _prefab = prefab;
            _cachedObjects = new List<T>(preloadCount);

            for (int i = 0; i < preloadCount; i++)
            {
                T obj = CreateNew();
                Release(obj);
            }
        }

        public T Produce()
        {
            return _cachedObjects.Count > 0 ? TakeFromCache() : CreateNew();
        }

        public void Release(T obj)
        {
            _cachedObjects.Add(obj);
            obj.Dispose();
            obj.transform.SetParent(_parent);
            obj.gameObject.SetActive(false);
        }

        private T TakeFromCache()
        {
            var obj = _cachedObjects[0];
            _cachedObjects.Remove(obj);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(null);
            obj.Init();
            return obj;
        }

        private T CreateNew()
        {
            var obj = Object.Instantiate(_prefab);
            obj.Init();
            return obj;
        }

        public void Dispose()
        {
            while (_cachedObjects.Count > 0)
            {
                var obj = _cachedObjects[0];
                _cachedObjects.Remove(obj);
                obj.Dispose();
                Object.Destroy(obj);
            }
        }
    }
}
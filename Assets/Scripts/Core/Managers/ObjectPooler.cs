using System.Collections.Generic;
using UnityEngine;

namespace Core.Managers
{
    [System.Serializable]
    public struct PoolObject
    {
        public GameObject pooledObject;
        public int amountToPool;
        public bool isExpandable;
    }
    
    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler Instance;

        [SerializeField] private List<PoolObject> objects;
        [SerializeField] private Transform pooledObjectsParent;
        private List<GameObject> _pooledObjects;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _pooledObjects = new List<GameObject>();
            foreach (var item in objects) {
                for (int i = 0; i < item.amountToPool; i++) {
                    var obj = (GameObject)Instantiate(item.pooledObject, pooledObjectsParent);
                    obj.SetActive(false);
                    _pooledObjects.Add(obj);
                }
            }
        }

        public GameObject GetPooledObject()
        {
            foreach (var item in _pooledObjects)
            {
                if (!item.activeInHierarchy && item.CompareTag(tag)) {
                    return item;
                }
            }
            
            foreach (var item in objects) {
                if (item.pooledObject.CompareTag(tag)) {
                    if (item.isExpandable) {
                        GameObject obj = (GameObject)Instantiate(item.pooledObject, pooledObjectsParent);
                        obj.SetActive(false);
                        _pooledObjects.Add(obj);
                        return obj;
                    }
                }
            }
            
            return null;
        }
    }
}

using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace Core.Managers
{
    [System.Serializable]
    public struct PoolObject
    {
        public PooledObjectType objectTag;
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
                    var comp = obj.AddComponent<PooledObjectData>();
                    comp.SetObjectTag(item.objectTag);
                    
                    obj.SetActive(false);
                    _pooledObjects.Add(obj);
                }
            }
        }

        public GameObject GetPooledObject(PooledObjectType objectTag)
        {
            foreach (var item in _pooledObjects)
            {
                if (!item.activeInHierarchy && item.GetComponent<PooledObjectData>().GetObjectTag() == objectTag) {
                    return item;
                }
            }
            
            foreach (var item in objects) {
                if (item.objectTag == objectTag) {
                    if (item.isExpandable) {
                        var obj = (GameObject)Instantiate(item.pooledObject, pooledObjectsParent);
                        var comp = obj.AddComponent<PooledObjectData>();
                        comp.SetObjectTag(item.objectTag);
                        
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

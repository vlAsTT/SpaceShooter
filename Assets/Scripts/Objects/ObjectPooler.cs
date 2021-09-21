using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    /// <summary>
    /// Single Object that should be used by Object Pooling
    /// </summary>
    [System.Serializable]
    public struct PoolObject
    {
        public PooledObjectType objectTag;
        public GameObject pooledObject;
        public int amountToPool;
        public bool isExpandable;
    }
    
    /// <summary>
    /// Class that handles Object Pooling and related actions
    /// </summary>
    public class ObjectPooler : MonoBehaviour
    {
        #region Variables

        public static ObjectPooler Instance;

        /// <summary>
        /// List of objects to pool
        /// </summary>
        [SerializeField] private List<PoolObject> objects;

        /// <summary>
        /// Transform of an object where all pooled objects should be spawned
        /// </summary>
        [SerializeField] private Transform pooledObjectsParent;
        
        private List<GameObject> _pooledObjects;

        #endregion

        #region Methods

        private void Awake()
        {
            Instance = this;
            
            _pooledObjects = new List<GameObject>();
            foreach (var item in objects) {
                for (int i = 0; i < item.amountToPool; i++) {
                    var obj = (GameObject)Instantiate(item.pooledObject, pooledObjectsParent);
                    var comp = obj.AddComponent<PooledObject>();
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
                if (!item.activeInHierarchy && item.GetComponent<PooledObject>().GetObjectTag() == objectTag) {
                    return item;
                }
            }
            
            foreach (var item in objects) {
                if (item.objectTag == objectTag) {
                    if (item.isExpandable) {
                        var obj = (GameObject)Instantiate(item.pooledObject, pooledObjectsParent);
                        var comp = obj.AddComponent<PooledObject>();
                        comp.SetObjectTag(item.objectTag);
                        
                        obj.SetActive(false);
                        _pooledObjects.Add(obj);
                        return obj;
                    }
                }
            }
            
            return null;
        }

        #endregion
    }
}

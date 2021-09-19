using UnityEngine;

namespace Objects
{
    public enum PooledObjectType
    {
        Enemy_01 = 0,
        Asteroid_01,
        Asteroid_02,
        Asteroid_03,
        Asteroid_04,
        Asteroid_05,
        Asteroid_06,
        Asteroid_07,
        Asteroid_08,
        Asteroid_09,
        Asteroid_10,
        Asteroid_11,
        Asteroid_12
    }
    
    public class PooledObjectData : MonoBehaviour
    {
        private PooledObjectType objectTag;
        
        public PooledObjectType GetObjectTag() => objectTag;

        public void SetObjectTag(PooledObjectType objectTag) => this.objectTag = objectTag;
    }
}

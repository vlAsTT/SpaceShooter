using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace Core.Managers
{
    [System.Serializable]
    public struct PooledObjectToValue
    {
        public PooledObjectType Type;
        public float Value;
    }
    
    public class PowerupManager : MonoBehaviour
    {
        public static PowerupManager Instance;
        
        [SerializeField] private List<PooledObjectToValue> _powerups = new List<PooledObjectToValue>();

        private void Awake()
        {
            Instance = this;
        }

        public float GetPowerupValue(PooledObjectType type)
        {
            foreach (var powerup in _powerups)
            {
                if (powerup.Type == type)
                {
                    return powerup.Value;
                }
            }

            return -1;
        }
    }
}

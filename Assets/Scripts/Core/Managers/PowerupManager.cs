using System.Collections.Generic;
using Objects;
using UI;
using UnityEngine;

namespace Core.Managers
{
    [System.Serializable]
    public struct PooledObjectToValue
    {
        public PooledObjectType type;
        public float value;
    }
    
    public class PowerupManager : MonoBehaviour
    {
        public static PowerupManager Instance;
        
        [SerializeField] private List<PooledObjectToValue> powerups = new List<PooledObjectToValue>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameOverlayManager.Instance.SetPooledObjectUIStatus(PooledObjectType.Powerup_Armor, false);
            GameOverlayManager.Instance.SetPooledObjectUIStatus(PooledObjectType.Powerup_Booster, false);
        }

        public float GetPowerupValue(PooledObjectType type)
        {
            foreach (var powerup in powerups)
            {
                if (powerup.type == type)
                {
                    return powerup.value;
                }
            }

            return -1;
        }
    }
}

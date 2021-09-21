using System.Collections.Generic;
using Objects;
using UI;
using UnityEngine;

namespace Core.Managers
{
    /// <summary>
    /// Data struct that allows to map PooledObjectType to certain float value
    /// Can be used to define score awards for destroying certain pooled objects, etc.
    /// </summary>
    /// <see cref="PooledObjectType"/>
    [System.Serializable]
    public struct PooledObjectToValue
    {
        public PooledObjectType type;
        public float value;
    }
    
    /// <summary>
    /// Handles logic of powerups and their UI initialization
    /// </summary>
    public class PowerupManager : MonoBehaviour
    {
        public static PowerupManager Instance;
        
        [SerializeField] private List<PooledObjectToValue> powerups = new List<PooledObjectToValue>();

        #region Methods

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

        #endregion
    }
}

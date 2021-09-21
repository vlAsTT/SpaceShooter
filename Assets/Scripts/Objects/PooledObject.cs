using System;
using System.Collections;
using Core.Managers;
using Player.Combat;
using Player.Core;
using UI;
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
        Asteroid_12,
        Powerup_Armor,
        Powerup_Booster,
        Bullet
    }
    
    /// <summary>
    /// Attached to every pooled object at creation
    /// Stores the most important information of a pooled object as well as self-destroy logic
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        #region Variables

        public PooledObjectType GetObjectTag() => _objectTag;
        
        private PooledObjectType _objectTag;
        private bool _destroyed = false;
        private ParticleSystem _explosionFX;
        private GameObject _player;
        private ParticleSystem _vfx;
        
        #endregion

        #region Methods

        #region Unity Methods

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _explosionFX = Resources.Load<ParticleSystem>("VFX/FX_Explosion");

            if (!_explosionFX)
            {
                Debug.unityLogger.Log(LogType.Assert, "Explosion VFX was not found");
            }
        }
        
        private void OnEnable()
        {
            _destroyed = false;
            ClearVFX();
        }

        private void OnDisable()
        {
            ClearVFX();
        }

        // In case when the game is over to clear up all memory
        private void OnDestroy()
        {
            ClearVFX();
        }

        #endregion

        #region Self Destroy

        private void ClearVFX()
        {
            if (!_vfx) return;
            
            Destroy(_vfx);
        }
        
        public void StartSelfDestroy()
        {
            StartCoroutine(SelfDestroy());
        }

        private IEnumerator SelfDestroy()
        {
            var objTransform = transform;
            _vfx = Instantiate(_explosionFX, objTransform.position, Quaternion.identity, objTransform);
            _destroyed = true;

            yield return new WaitForSeconds(_vfx.main.duration);
            
            Destroy(_vfx);
            gameObject.SetActive(false);
        }

        #endregion
        
        public void SetObjectTag(PooledObjectType objectTag) => _objectTag = objectTag;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // PooledObject - Player
            {
                if (_objectTag == PooledObjectType.Powerup_Armor)
                {
                    var combatController = _player.GetComponentInParent<PlayerCombatController>();

                    if (!combatController)
                    {
                        Debug.unityLogger.Log(LogType.Log, $"Player : {_player.name} does not contain Player Combat Controller");
                        return;
                    }

                    combatController.SetArmor(1f);
                    GameOverlayManager.Instance.SetPooledObjectUIStatus(PooledObjectType.Powerup_Armor, true);
                    
                    gameObject.SetActive(false);
                }
                else if (_objectTag == PooledObjectType.Powerup_Booster)
                {
                    var movementController = _player.GetComponentInParent<PlayerMovementController>();

                    if (!movementController)
                    {
                        Debug.unityLogger.Log(LogType.Log, $"Player : {_player.name} does not contain Player Movement Controller");
                        return;
                    }
                    
                    movementController.SetSpeedMultiplier(
                        PowerupManager.Instance.GetPowerupValue(PooledObjectType.Powerup_Booster));
                    GameOverlayManager.Instance.SetPooledObjectUIStatus(PooledObjectType.Powerup_Booster, true);
                    
                    gameObject.SetActive(false);
                }
                else
                {
                    // Send Destroy Signal to Player Health Manager
                    
                    StartCoroutine(SelfDestroy());
                }
            }
            else
            {
                var component = other.gameObject.GetComponentInParent<PooledObject>();

                if (component) // Pooled Object - Pooled Object
                {
                    // If any of the objects is powerup - ignore it
                    if (_objectTag == PooledObjectType.Powerup_Armor ||
                        _objectTag == PooledObjectType.Powerup_Booster ||
                        component._objectTag == PooledObjectType.Powerup_Armor ||
                        component._objectTag == PooledObjectType.Powerup_Booster) return;

                    // If we have bullet vs Pooled Object (not powerup)
                    if (_objectTag == PooledObjectType.Bullet && !component._destroyed) 
                    {
                        _destroyed = true;
                        ScoreManager.Instance.AddScore(component._objectTag);
                    }
                    
                    StartCoroutine(SelfDestroy());
                    component.StartSelfDestroy();
                }
                else
                {
                    Debug.unityLogger.Log(LogType.Warning, $"{_objectTag} with {other.gameObject.name}"); // Unexpected Case
                }
                
            }
        }

        #endregion
    }
}

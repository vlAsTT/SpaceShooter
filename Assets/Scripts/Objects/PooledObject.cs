using System;
using System.Collections;
using Core.Managers;
using Player.Core;
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
    
    public class PooledObject : MonoBehaviour
    {
        private PooledObjectType _objectTag;
        private float _destroyTime;
        
        public PooledObjectType GetObjectTag() => _objectTag;
        private ParticleSystem _explosionFX;

        private GameObject _player;
        
        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _explosionFX = Resources.Load<ParticleSystem>("VFX/FX_Explosion");

            if (!_explosionFX)
            {
                Debug.unityLogger.Log(LogType.Assert, "Explosion VFX was not found");
            }
        }

        public void SetObjectTag(PooledObjectType objectTag) => _objectTag = objectTag;

        public void SetDestroyTime(float time) => _destroyTime = time;

        public void StartSelfDestroy()
        {
            StartCoroutine(SelfDestroy());
        }
        
        private IEnumerator SelfDestroy()
        {
            var vfx = Instantiate(_explosionFX, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(_destroyTime);

            Debug.unityLogger.Log(LogType.Log, $"{gameObject.name} is destroyed");
            
            Destroy(vfx);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // PooledObject - Player
            {
                if (_objectTag == PooledObjectType.Powerup_Armor)
                {
                    // Add Armor in CombatController
                    
                    gameObject.SetActive(false);
                }
                else if (_objectTag == PooledObjectType.Powerup_Booster)
                {
                    var movementController = _player.GetComponent<PlayerMovementController>();

                    if (!movementController)
                    {
                        Debug.unityLogger.Log(LogType.Log, $"Player : {_player.name} does not contain Player Movement Controller");
                        return;
                    }
                    
                    movementController.SetSpeedMultiplier(
                        PowerupManager.Instance.GetPowerupValue(PooledObjectType.Powerup_Booster));
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
                    Debug.unityLogger.Log(LogType.Log, $"Destroying {gameObject.name} and {component.gameObject.name}");
                    
                    // If any of the objects is powerup - ignore it
                    if (_objectTag == PooledObjectType.Powerup_Armor ||
                        _objectTag == PooledObjectType.Powerup_Booster ||
                        component._objectTag == PooledObjectType.Powerup_Armor ||
                        component._objectTag == PooledObjectType.Powerup_Booster) return;
                    
                    StartCoroutine(SelfDestroy());
                    component.StartSelfDestroy();
                }
                else
                {
                    Debug.unityLogger.Log(LogType.Warning, $"{_objectTag} with {other.gameObject.name}"); // Unexpected Case
                }
                
            }
        }
    }
}

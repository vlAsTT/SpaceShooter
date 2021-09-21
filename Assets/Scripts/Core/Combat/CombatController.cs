using System;
using System.Collections.Generic;
using Core.Managers;
using Objects;
using UnityEngine;

namespace Core.Combat
{
    /// <summary>
    /// Base class that holds common logic for the combat for both Enemy AI and Player
    /// </summary>
    public abstract class CombatController : MonoBehaviour
    {
        [Tooltip("Transforms of enemy weapons located on the ship")]
        [SerializeField] private List<Transform> shipWeapons;
        [Tooltip("A point in the world that weapons are heading towards to")]
        [SerializeField] protected Transform lookPoint;
        [Header("Effects")] [Tooltip("Visual FX that is used when ship is being destroyed")]
        [SerializeField] protected ParticleSystem explosionFX;

        protected virtual void Shoot()
        {
            for (int i = 0; i < shipWeapons.Count; ++i)
            {
                var bullet = ObjectPooler.Instance.GetPooledObject(PooledObjectType.Bullet);
                bullet.SetActive(true);
                bullet.transform.position = shipWeapons[i].position;
                var shootDirection = (lookPoint.position - shipWeapons[i].position).normalized;

                bullet.transform.LookAt(lookPoint);
                
                var missileMovement = bullet.GetComponent<MissileMovement>();
                missileMovement.OnInstantiate(shootDirection);
            }
        }
    }
}

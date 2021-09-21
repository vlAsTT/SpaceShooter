using System.Collections.Generic;
using Core.Managers;
using Objects;
using UnityEngine;

namespace Core.Combat
{
    public class CombatController : MonoBehaviour
    {
        [SerializeField] private List<Transform> shipWeapons;
        [SerializeField] protected Transform lookPoint;
        
        [SerializeField] protected ParticleSystem explosionFX;
        
        protected void Shoot()
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

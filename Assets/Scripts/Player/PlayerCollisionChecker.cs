using Objects;
using Player.Combat;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionChecker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // Check if it is a powerup
            var pooledObj = other.GetComponent<PooledObject>();

            if (pooledObj.GetObjectTag() == PooledObjectType.Powerup_Armor ||
                pooledObj.GetObjectTag() == PooledObjectType.Powerup_Booster)
            {
                return;
            }
            
            var combatController = GetComponentInParent<PlayerCombatController>();
            
            if (combatController)
            {
                combatController.ApplyHit();
            }
        }
    }
}

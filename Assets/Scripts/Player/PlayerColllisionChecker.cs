using Player.Combat;
using UnityEngine;

namespace Player
{
    public class PlayerColllisionChecker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var combatController = GetComponentInParent<PlayerCombatController>();

            if (combatController)
            {
                combatController.ApplyHit();
            }
        }
    }
}

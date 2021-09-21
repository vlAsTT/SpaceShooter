using System.Collections;
using Core.Combat;
using Objects;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Combat
{
    /// <summary>
    /// Holds logic of a combat for the Player
    /// </summary>
    /// <inheritdoc cref="CombatController"/>
    public class PlayerCombatController : CombatController
    {
        #region Variables

        [Tooltip("Visual FX that is being played when Player is being hit")]
        [SerializeField] private ParticleSystem hitFX;
        private float _armor;

        #endregion

        #region Methods

        public void SetArmor(float val) => _armor = val;
        
        public void ApplyHit()
        {
            if (_armor > 0f)
            {
                _armor = 0f;
                GameOverlayManager.Instance.SetPooledObjectUIStatus(PooledObjectType.Powerup_Armor, false);
            }
            else
            {
                StartCoroutine(SelfDestroy());
            }
        }
        
        /// <summary>
        /// Handles Shoot Input logic
        /// </summary>
        /// <param name="ctx"></param>
        public void OnShoot(InputAction.CallbackContext ctx)
        {
            if (!ctx.started) return;
            
            Shoot();
        }

        #region Self Destroy

        private IEnumerator SelfDestroy()
        {
            yield return DestroyFX(false);
            PlayerDelegates.OnPlayerDeath();
        }

        private IEnumerator DestroyFX(bool isHit)
        {
            var vfx = Instantiate(isHit ? hitFX : explosionFX, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(vfx.main.duration);
            
            Destroy(vfx);
        }

        #endregion

        #endregion
    }
}

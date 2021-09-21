using System.Collections;
using Core;
using Core.Combat;
using Objects;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Combat
{
    public class PlayerCombatController : CombatController
    {
        #region Variables

        private float _armor;
        [SerializeField] private ParticleSystem explosionFX;
        [SerializeField] private ParticleSystem hitFX;

        #endregion

        public void SetArmor(float val) => _armor = val;

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

        public void OnShoot(InputAction.CallbackContext ctx)
        {
            if (!ctx.started) return;
            
            Shoot();
        }
    }
}

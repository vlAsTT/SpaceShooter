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
        [Tooltip("Sound FX that is being played on shot")]
        [SerializeField] private AudioClip shotFX;

        [Tooltip("Volume of Shot FX")] 
        [SerializeField] [Range(0.01f, 1f)] private float shotVolume = 0.8f;

        private AudioSource _audioSource;
        private float _armor;

        #endregion

        #region Methods

        private void Start()
        {
            if (!Camera.main)
            {
                Debug.unityLogger.Log(LogType.Error, "Main Camera is missing");
                return;
            }
            
            _audioSource = Camera.main.GetComponent<AudioSource>();
        }
        
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

        protected override void Shoot()
        {
            base.Shoot();
            
            _audioSource.PlayOneShot(shotFX, shotVolume);
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

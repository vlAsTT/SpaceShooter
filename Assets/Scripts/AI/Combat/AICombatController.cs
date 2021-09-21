using System.Collections;
using Core.Combat;
using UnityEngine;

namespace AI.Combat
{
    /// <summary>
    /// Holds logic of a combat for the enemy AI
    /// </summary>
    /// <inheritdoc cref="CombatController"/>
    public class AICombatController : CombatController
    {
        #region Variables

        [Tooltip("Shoot Rate of an enemy ship")]
        [SerializeField][Range(0.5f, 10f)] private float shootRate = 3f;
        [Tooltip("Minimal distance between Player and Ship to shoot")]
        [SerializeField] private float attackRange = 30f;
        [Tooltip("How long enemy will shoot player before switching state")]
        [SerializeField] private float attackTime = 5f;
        
        private bool _isShootingEnabled;
        private IEnumerator _currentCoroutine;

        #endregion

        #region Methods

        #region Getters / Setters

        public float GetAttackRange() => attackRange;

        public float GetAttackTime() => attackTime;

        public void SetShootingStatus(bool status) => _isShootingEnabled = status;

        #endregion

        #region Unity Methods

        private void Start()
        {
            lookPoint = GameObject.FindWithTag("Player").transform;
        }
        
        // Necessary to avoid undefined behavior with object pooling
        private void OnDisable()
        {
            if (_currentCoroutine == null) return;
            
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        #endregion

        #region Shooting

        public void EnableShooting()
        {
            _currentCoroutine = Shooting();
            StartCoroutine(_currentCoroutine);
        }
        
        private IEnumerator Shooting()
        {
            while (_isShootingEnabled)
            {
                Shoot();

                yield return new WaitForSeconds(shootRate);
            }
        }

        #endregion

        #endregion
    }
}

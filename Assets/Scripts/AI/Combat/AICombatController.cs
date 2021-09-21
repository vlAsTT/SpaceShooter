using System;
using System.Collections;
using Core.Combat;
using UnityEngine;

namespace AI.Combat
{
    public class AICombatController : CombatController
    {
        [SerializeField][Range(0.5f, 10f)] private float shootRate = 3f;

        private bool _isShootingEnabled;
        private IEnumerator _currentCoroutine;

        public void SetShootingStatus(bool status) => _isShootingEnabled = status;
        
        private void Start()
        {
            lookPoint = GameObject.FindWithTag("Player").transform;
        }

        public void EnableShooting()
        {
            _currentCoroutine = Shooting();
            StartCoroutine(_currentCoroutine);
        }

        private void OnDisable()
        {
            if (_currentCoroutine == null) return;
            
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        private IEnumerator Shooting()
        {
            while (_isShootingEnabled)
            {
                Shoot();

                yield return new WaitForSeconds(shootRate);
            }
        }
    }
}

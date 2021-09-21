using System;
using System.Collections;
using AI.Combat;
using AI.Movement;
using Unity.Mathematics;
using UnityEngine;

namespace AI.Core
{
    public class EnemyBrain : MonoBehaviour
    {
        #region Variables

        // reference to all controllers

        public EnemyState _state = EnemyState.Idle;

        [SerializeField] private float AttackRange = 30f;
        [SerializeField] private float AttackTime = 5f;
        [SerializeField] private float DormantTime = 8f;
        [SerializeField] private float ChaseSpeedMultiplier = 1.5f;
        [SerializeField] private float SpeedGainTime = 3f;

        private Transform _player;
        private AIMovementController _aiMovementController;
        private AICombatController _aiCombatController;
        private IEnumerator _currentCoroutine;

        private float _timeHandler;
        
        #endregion
        
        // movement related stuff - find a player

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").transform;
            
            _aiMovementController = GetComponent<AIMovementController>();
            _aiMovementController.SetTarget(_player);
        }

        private void OnEnable()
        {
            // Stops previously running coroutine if it's an enemy that was already obtained from the object pooling
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            
            // Disable VFX & SFX
            _aiMovementController.SetSpeedMultiplier(0f);
            _currentCoroutine = PerformIdle();
            StartCoroutine(_currentCoroutine);
        }

        private IEnumerator PerformIdle()
        {
            while (_timeHandler < DormantTime)
            {
                // do lerp till speed multiplier is 0
                _aiMovementController.SetSpeedMultiplier(math.lerp(_aiMovementController.GetSpeedMultiplier(), 0f,
                    Time.deltaTime * SpeedGainTime));

                _timeHandler += Time.deltaTime;

                yield return null;
            }
            
            _timeHandler = 0f;
            
            _state = EnemyState.Chase;
            _currentCoroutine = PerformChase();
            StartCoroutine(_currentCoroutine);
        }

        private IEnumerator PerformChase()
        {
            while (Vector3.Distance(_player.position, transform.position) > AttackRange)
            {
                // Tell Movement Controller to Chase & Increase speed multiplier by X - lerping
                _aiMovementController.SetSpeedMultiplier(math.lerp(1f, ChaseSpeedMultiplier, Time.deltaTime * SpeedGainTime));
                
                yield return null;
            }

            _state = EnemyState.Attack;
            _aiCombatController.SetShootingStatus(true);
            _aiCombatController.EnableShooting();
            _currentCoroutine = PerformAttack();
            StartCoroutine(_currentCoroutine);
        }

        private IEnumerator PerformAttack()
        {
            // Enable Attack Mode in Combat Controller
            // Combat Controller should handle the attack cooldown

            while (_timeHandler < AttackTime)
            {
                // Slows down multiplier to be optimal to keep distance between player and enemy
                _aiMovementController.SetSpeedMultiplier(Mathf.Clamp(Vector3.Distance(_player.position, transform.position) * Time.deltaTime, 0f, ChaseSpeedMultiplier));
                
                _timeHandler += Time.deltaTime;

                yield return null;
            }

            _timeHandler = 0f;
            
            _state = EnemyState.Idle;
            _aiCombatController.SetShootingStatus(false);
            _currentCoroutine = PerformIdle();
            StartCoroutine(_currentCoroutine);
        }
    }
}

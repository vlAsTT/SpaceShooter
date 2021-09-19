using System;
using System.Collections;
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

        private float _timeHandler;
        
        #endregion
        
        // movement related stuff - find a player

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").transform;
        }

        private void Start()
        {
            _aiMovementController = GetComponent<AIMovementController>();
            _aiMovementController.SetTarget(_player);
            
            // We start from Idle and go to chase in dormant time seconds
            // Disable VFX & SFX
            _aiMovementController.SetSpeedMultiplier(0f);
            StartCoroutine(PerformIdle());
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
            StartCoroutine(PerformChase());
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
            StartCoroutine(PerformAttack());
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
            StartCoroutine(PerformIdle());
        }
    }
}

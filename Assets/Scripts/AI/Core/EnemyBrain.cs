using System.Collections;
using AI.Combat;
using AI.Movement;
using UnityEngine;

namespace AI.Core
{
    /// <summary>
    /// Controlling AI State Machine
    /// </summary>
    public class EnemyBrain : MonoBehaviour
    {
        #region Variables

        /// <summary>
        /// Current Enemy State
        /// Not being used at the current moment, but might be useful on expansion of State Machine
        /// </summary>
        private EnemyState _state = EnemyState.Idle;
        private IEnumerator _currentCoroutine;
        private float _timeHandler;

        #region References

        private Transform _player;
        private AIMovementController _aiMovementController;
        private AICombatController _aiCombatController;

        #endregion

        #region Movement / Combat

        /// <summary>
        /// Minimal Distance between Player and Enemy to switch to Attack State
        /// </summary>
        private float _attackRange;
        /// <summary>
        /// Duration of an Attack State
        /// </summary>
        private float _attackTime;
        /// <summary>
        /// Duration of an Idle State
        /// </summary>
        private float _dormantTime;

        #endregion

        #endregion

        #region Methods

        #region Unity Methods

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").transform;
            
            _aiMovementController = GetComponent<AIMovementController>();
            _aiMovementController.SetTarget(_player);

            _aiCombatController = GetComponent<AICombatController>();
        }

        /// <summary>
        /// Combat & Movement Variables Initialization
        /// </summary>
        private void Start()
        {
            _attackRange = _aiCombatController.GetAttackRange();
            _attackTime = _aiCombatController.GetAttackTime();
            _dormantTime = _aiMovementController.GetDormantTime();
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

        #endregion

        #region States
        
        private IEnumerator PerformIdle()
        {
            while (_timeHandler < _dormantTime)
            {
                _aiMovementController.GainSpeed(_aiMovementController.GetSpeedMultiplier(), 0f);

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
            while (Vector3.Distance(_player.position, transform.position) > _attackRange)
            {
                _aiMovementController.GainSpeed(1f, _aiMovementController.GetChaseSpeedMultiplier());
                
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
            while (_timeHandler < _attackTime)
            {
                // Slows down multiplier to be optimal to keep distance between player and enemy
                _aiMovementController.SetSpeedMultiplier(Mathf.Clamp((Vector3.Distance(_player.position, transform.position) - _attackRange) * Time.deltaTime, 0f, _aiMovementController.GetSpeedMultiplier()));

                _timeHandler += Time.deltaTime;

                yield return null;
            }

            _timeHandler = 0f;
            
            _state = EnemyState.Idle;
            _aiCombatController.SetShootingStatus(false);
            _currentCoroutine = PerformIdle();
            StartCoroutine(_currentCoroutine);
        }

        #endregion

        #endregion
    }
}

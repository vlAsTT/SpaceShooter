using Core.Movement;
using Unity.Mathematics;
using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Holds logic of a movement for the enemy AI
    /// </summary>
    /// <inheritdoc cref="MovementController"/>
    public class AIMovementController : MovementController
    {
        #region Variables

        [Tooltip("Duration of the ship being in Idle State")]
        [SerializeField] private float dormantTime = 8f;
        [Tooltip("being multiplied by a regular forward speed of a ship when state equals to Chase")]
        [SerializeField] private float chaseSpeedMultiplier = 1.5f;
        [Tooltip("How long it takes for a ship to gain or reduce speed")]
        [SerializeField] private float speedGainTime = 3f;
        
        private Transform _followTarget;

        #endregion

        #region Methods

        #region Getters / Setters

        public void SetTarget(Transform followGoal)
        {
            _followTarget = followGoal;
        }

        public float GetChaseSpeedMultiplier() => chaseSpeedMultiplier;

        public float GetDormantTime() => dormantTime;

        #endregion

        protected override void Update()
        {
            base.Update();

            if (!_followTarget) return;

            transform.position = Vector3.MoveTowards(transform.position, _followTarget.position, Time.deltaTime * CurrentForwardSpeed);
            transform.LookAt(_followTarget);
        }

        public void GainSpeed(float current, float next)
        {
            SpeedMultiplier = math.lerp(current, next, Time.deltaTime * speedGainTime);
        }

        #endregion
    }
}

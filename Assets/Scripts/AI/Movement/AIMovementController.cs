using System;
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

        [Header("Base Flight Settings")][Tooltip("Duration of the ship being in Idle State")]
        [SerializeField][Min(1f)] private float dormantTime = 8f;
        [Tooltip("being multiplied by a regular forward speed of a ship when state equals to Chase")]
        [SerializeField][Min(1.1f)] private float chaseSpeedMultiplier = 1.5f;
        [Tooltip("How long it takes for a ship to gain or reduce speed")]
        [SerializeField][Min(0.1f)] private float speedGainTime = 3f;

        [Header("Advanced Flight Settings")][Tooltip("Minimal Distance between enemy ships")]
        [SerializeField] private float closestShipDistanceAllowed = 150f;
        [Tooltip("Distance where enemy ships detect each other and change path")]
        [SerializeField] private float detectDistance = 15f;
        
        
        private Transform _followTarget;
        private float _sideOffset;

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

        private void OnEnable()
        {
            _sideOffset = 0f;
        }

        protected override void Update()
        {
            base.Update();

            if (!_followTarget) return;

            // Sphere Casts to left & right from the ship to detect possible crash and avoid it
            if(Physics.SphereCast(transform.position, closestShipDistanceAllowed, transform.right, out _, detectDistance))
            {
                _sideOffset += -closestShipDistanceAllowed;
            }

            if (Physics.SphereCast(transform.position, closestShipDistanceAllowed, -transform.right, out _, detectDistance))
            {
                _sideOffset += closestShipDistanceAllowed;
            }

            transform.position = Vector3.MoveTowards(transform.position, _followTarget.position - new Vector3(_sideOffset, 0f, _sideOffset), Time.deltaTime * CurrentForwardSpeed);
            transform.LookAt(_followTarget);
        }

        public void GainSpeed(float current, float next)
        {
            SpeedMultiplier = math.lerp(current, next, Time.deltaTime * speedGainTime);
        }

        #endregion
    }
}

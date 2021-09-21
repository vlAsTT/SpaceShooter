using System;
using Core.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Core
{
    public class PlayerMovementController : MovementController
    {
        [Header("Model Data")]
        [SerializeField] private Transform model;
        [SerializeField] private float modelRotateAngle = 50f;

        private bool _isGameRunning = true;

        private void OnEnable()
        {
            PlayerDelegates.onPlayerDeath += OnDeath;
        }

        private void OnDisable()
        {
            PlayerDelegates.onPlayerDeath -= OnDeath;
        }

        protected override void Update()
        {
            if (!_isGameRunning) return;
            
            base.Update();

            // Model Rotation
            model.Rotate(MovementDirections.y * modelRotateAngle * Time.deltaTime, 0f, MovementDirections.x * modelRotateAngle * Time.deltaTime);
            
            // Model Position Adjustment
            var modelTransform = model.transform;
            modelTransform.position += (modelTransform.right * (CurrentStrafeSpeed * Time.deltaTime)) + (modelTransform.up * (CurrentHoverSpeed * Time.deltaTime)) + (modelTransform.forward * (CurrentForwardSpeed * Time.deltaTime));
        }

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            MovementDirections = ctx.ReadValue<Vector2>();
        }

        private void OnDeath()
        {
            _isGameRunning = false;
        }
    }
}

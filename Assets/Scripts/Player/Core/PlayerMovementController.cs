using Core.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Core
{
    /// <summary>
    /// Holds logic of a movement for the Player
    /// </summary>
    /// <inheritdoc cref="MovementController"/>
    public class PlayerMovementController : MovementController
    {
        #region Variables

        [Header("Model Data")][Tooltip("Reference to Player's Ship Model Object")]
        [SerializeField] private Transform model;
        [Tooltip("Angle of Rotation of Player's Ship Model")]
        [SerializeField] private float modelRotateAngle = 50f;

        private bool _isGameRunning = true;

        #endregion

        #region Methods

        #region Unity Methods

        private void OnEnable()
        {
            _isGameRunning = true;
            
            PlayerDelegates.onPlayerDeath += () =>
            {
                _isGameRunning = false;
            };
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

        #endregion

        /// <summary>
        /// Handles Input Movement logic
        /// </summary>
        /// <param name="ctx"></param>
        public void OnMovement(InputAction.CallbackContext ctx)
        {
            MovementDirections = ctx.ReadValue<Vector2>();
        }

        #endregion
    }
}

using Unity.Mathematics;
using UnityEngine;

namespace Core.Movement
{
    public abstract class MovementController : MonoBehaviour
    {
        #region Variables

        #region Editor Variables

        [Header("Base Speeds & Accelerations")]
        [SerializeField] private float forwardSpeed = 50f;
        [SerializeField] private float forwardAcceleration = 10f;

        [Space(5)]
        [SerializeField] private float strafeSpeed = 20f;
        [SerializeField] private float strafeAcceleration = 2f;
        
        [Space(5)]
        [SerializeField] private float hoverSpeed = 15f;
        [SerializeField] private float hoverAcceleration = 2f;

        #endregion

        #region Input Variables
        
        protected Vector2 MovementDirections;

        #endregion

        #region Speed Variables

        protected float CurrentForwardSpeed, CurrentStrafeSpeed, CurrentHoverSpeed;
        private float _speedMultiplier = 1f;

        #endregion
        
        #endregion

        #region Methods

        public void SetSpeedMultiplier(float multiplier) => _speedMultiplier = multiplier;

        public float GetSpeedMultiplier() => _speedMultiplier;
        
        protected virtual void Update()
        {
            // Movement Calculations & Applying new movement values
            CurrentForwardSpeed = math.lerp(CurrentForwardSpeed, forwardSpeed * _speedMultiplier,
                forwardAcceleration * Time.deltaTime);
            CurrentHoverSpeed = math.lerp(CurrentHoverSpeed, MovementDirections.y * hoverSpeed, hoverAcceleration * Time.deltaTime);
            CurrentStrafeSpeed = math.lerp(CurrentStrafeSpeed, MovementDirections.x * strafeSpeed, strafeAcceleration * Time.deltaTime);
        }

        #endregion
    }
}

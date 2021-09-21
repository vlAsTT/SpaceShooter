using Unity.Mathematics;
using UnityEngine;

namespace Core.Movement
{
    /// <summary>
    /// Base class that holds common logic for the movement for both Enemy AI and Player
    /// </summary>
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

        #region Speed Variables

        protected float CurrentForwardSpeed, CurrentStrafeSpeed, CurrentHoverSpeed;
        protected float SpeedMultiplier = 1f;

        #endregion
        
        protected Vector2 MovementDirections;
        
        #endregion

        #region Methods

        #region Getters / Setters

        public void SetSpeedMultiplier(float multiplier) => SpeedMultiplier = multiplier;

        public float GetSpeedMultiplier() => SpeedMultiplier;

        #endregion
        
        protected virtual void Update()
        {
            // Movement Calculations & Applying new movement values
            CurrentForwardSpeed = math.lerp(CurrentForwardSpeed, forwardSpeed * SpeedMultiplier,
                forwardAcceleration * Time.deltaTime);
            CurrentHoverSpeed = math.lerp(CurrentHoverSpeed, MovementDirections.y * hoverSpeed, hoverAcceleration * Time.deltaTime);
            CurrentStrafeSpeed = math.lerp(CurrentStrafeSpeed, MovementDirections.x * strafeSpeed, strafeAcceleration * Time.deltaTime);
        }

        #endregion
    }
}

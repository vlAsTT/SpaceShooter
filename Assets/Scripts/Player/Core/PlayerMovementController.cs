using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Core
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Variables

        private static ILogger _logger = Debug.unityLogger;

        [SerializeField] private float forwardSpeed = 50f;
        [SerializeField] private float strafeSpeed = 20f;
        [SerializeField] private float hoverSpeed = 15f;

        [SerializeField] private float lookSpeed = 90f;
        private Vector2 lookInput, screenCenter, mouseDistance;

        [SerializeField] private float rollSpeed = 90f;
        [SerializeField] private float rollAcceleration = 3.5f;
        
        private Rigidbody _rb;
        private Camera _camera;
        private float currentForwardSpeed, currentStrafeSpeed, currentHoverSpeed, currentRollSpeed;
        [SerializeField] private float forwardAcceleration = 10f;
        [SerializeField] private float strafeAcceleration = 2f;
        [SerializeField] private float hoverAcceleration = 2f;

        [SerializeField] private Transform model;
        [SerializeField] private float modelRotateAngle = 50f;
        
        private Vector2 input;
        private float rollInput;
        
        private float speedMultiplier = 1f;
        
        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _camera = Camera.main;
        }

        private void Start()
        {
            screenCenter.x = Screen.width * .5f;
            screenCenter.y = Screen.height * .5f;

            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            // Rotation Calculations & Applying new rotation values
            currentRollSpeed = math.lerp(currentRollSpeed, rollInput, rollAcceleration * Time.deltaTime);
            
            transform.Rotate(-mouseDistance.y * lookSpeed * Time.deltaTime, mouseDistance.x * lookSpeed * Time.deltaTime, currentRollSpeed * rollSpeed * Time.deltaTime, Space.Self);
            
            // Rotate Model
            model.Rotate(0f, 0f, input.x * modelRotateAngle * Time.deltaTime);

            // Movement Calculations & Applying new movement values
            currentForwardSpeed = math.lerp(currentForwardSpeed, forwardSpeed * speedMultiplier,
                forwardAcceleration * Time.deltaTime);
            currentHoverSpeed = math.lerp(currentHoverSpeed, input.y * hoverSpeed, hoverAcceleration * Time.deltaTime);
            currentStrafeSpeed = math.lerp(currentStrafeSpeed, input.x * strafeSpeed, strafeAcceleration * Time.deltaTime);
            
            transform.position += (transform.right * (currentStrafeSpeed * Time.deltaTime)) + (transform.up * (currentHoverSpeed * Time.deltaTime)) + (transform.forward * (currentForwardSpeed * Time.deltaTime));
        }

        #region Input Methods

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            input = ctx.ReadValue<Vector2>();
            _logger.Log(LogType.Log, $"Movement called, value: {input}");
        }
        
        public void OnCameraMovement(InputAction.CallbackContext ctx)
        {
            if (ctx.started) return;
            
            lookInput = ctx.ReadValue<Vector2>();
            _logger.Log(LogType.Log, $"Camera Movement Called, value: {lookInput}");

            mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
            mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        }

        public void OnRollMovement(InputAction.CallbackContext ctx)
        {
            rollInput = ctx.ReadValue<float>();
            _logger.Log(LogType.Log, $"Camera Roll Called, value: {rollInput}");
        }

        #endregion
    }
}

using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Core
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float forwardSpeed = 50f;
        [SerializeField] private float strafeSpeed = 20f;
        [SerializeField] private float hoverSpeed = 15f;

        private Vector2 _lookInput, _screenCenter, _mouseDistance;

        [SerializeField] private float rollSpeed = 90f;
        [SerializeField] private float rollAcceleration = 3.5f;
        
        private Camera _camera;
        private float _currentForwardSpeed, _currentStrafeSpeed, _currentHoverSpeed, _currentRollSpeed;
        [SerializeField] private float forwardAcceleration = 10f;
        [SerializeField] private float strafeAcceleration = 2f;
        [SerializeField] private float hoverAcceleration = 2f;

        [SerializeField] private Transform model;
        [SerializeField] private float modelRotateAngle = 50f;
        
        private Vector2 _input;
        private float _rollInput;
        
        private float speedMultiplier = 1f;
        
        #endregion

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            _screenCenter.x = Screen.width * .5f;
            _screenCenter.y = Screen.height * .5f;

            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            // Rotation Calculations & Applying new rotation values
            _currentRollSpeed = math.lerp(_currentRollSpeed, _rollInput, rollAcceleration * Time.deltaTime);
            
            // _camera.transform.Rotate(-_mouseDistance.y * lookSpeed * Time.deltaTime, _mouseDistance.x * lookSpeed * Time.deltaTime, _currentRollSpeed * rollSpeed * Time.deltaTime, Space.Self);
            
            // Model Rotation
            model.Rotate(_input.y * modelRotateAngle * Time.deltaTime, 0f, _input.x * modelRotateAngle * Time.deltaTime);

            // Camera Rotation
            Vector3 moveCamTo = model.position - model.forward * 25f + Vector3.up * 10f;
            _camera.transform.position = math.lerp(_camera.transform.position, moveCamTo, Time.deltaTime * 2f);
            _camera.transform.LookAt(model.position + model.forward * 30f);

            // Movement Calculations & Applying new movement values
            _currentForwardSpeed = math.lerp(_currentForwardSpeed, forwardSpeed * speedMultiplier,
                forwardAcceleration * Time.deltaTime);
            _currentHoverSpeed = math.lerp(_currentHoverSpeed, _input.y * hoverSpeed, hoverAcceleration * Time.deltaTime);
            _currentStrafeSpeed = math.lerp(_currentStrafeSpeed, _input.x * strafeSpeed, strafeAcceleration * Time.deltaTime);

            var modelTransform = model.transform;
            modelTransform.position += (modelTransform.right * (_currentStrafeSpeed * Time.deltaTime)) + (modelTransform.up * (_currentHoverSpeed * Time.deltaTime)) + (modelTransform.forward * (_currentForwardSpeed * Time.deltaTime));
        }

        #region Input Methods

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            _input = ctx.ReadValue<Vector2>();
        }
        
        public void OnCameraMovement(InputAction.CallbackContext ctx)
        {
            if (ctx.started) return;
            
            _lookInput = ctx.ReadValue<Vector2>();

            _mouseDistance.x = (_lookInput.x - _screenCenter.x) / _screenCenter.y;
            _mouseDistance.y = (_lookInput.y - _screenCenter.y) / _screenCenter.y;

            _mouseDistance = Vector2.ClampMagnitude(_mouseDistance, 1f);
        }

        public void OnRollMovement(InputAction.CallbackContext ctx)
        {
            _rollInput = ctx.ReadValue<float>();
        }

        #endregion
    }
}

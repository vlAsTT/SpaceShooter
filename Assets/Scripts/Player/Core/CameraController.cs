using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Core
{
    public class CameraController : MonoBehaviour
    {
        #region Variables

        [Header("Camera Information")] 
        [SerializeField] private float cameraXOffset = 25f;
        [SerializeField] private float cameraYOffset = 10f;
        [SerializeField] private float cameraAcceleration = 2f;

        [Header("Model Data")]
        [SerializeField] private Transform model;
        
        private Vector2 _lookInput, _screenCenter, _mouseDistance;
        private Camera _camera;

        #endregion

        #region Methods

        #region Unity Methods

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
        
        private void LateUpdate()
        {
            Vector3 moveCamTo = model.position - model.forward * cameraXOffset + Vector3.up * cameraYOffset;
            _camera.transform.position = math.lerp(_camera.transform.position, moveCamTo, Time.deltaTime * cameraAcceleration);
            _camera.transform.LookAt(model.position + model.forward * cameraXOffset);
        }

        #endregion
        
        public void OnCameraMovement(InputAction.CallbackContext ctx)
        {
            if (ctx.started) return;
            
            _lookInput = ctx.ReadValue<Vector2>();

            _mouseDistance.x = (_lookInput.x - _screenCenter.x) / _screenCenter.y;
            _mouseDistance.y = (_lookInput.y - _screenCenter.y) / _screenCenter.y;

            _mouseDistance = Vector2.ClampMagnitude(_mouseDistance, 1f);
        }

        #endregion
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class MovementController : MonoBehaviour
    {
        #region Variables

        private static ILogger _logger = Debug.unityLogger;

        [SerializeField] private float movementSpeed = 50f;

        #endregion

        #region Input Methods

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            if (ctx.started) return;
            
            _logger.Log(LogType.Log, $"Movement called, value: {ctx.ReadValue<Vector2>()}");
            
            
        }

        #endregion
    }
}

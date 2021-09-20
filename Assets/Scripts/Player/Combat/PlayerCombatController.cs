using Core.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Combat
{
    public class PlayerCombatController : CombatController
    {
        #region Variables

        

        #endregion
        
        public void OnShoot(InputAction.CallbackContext ctx)
        {
            if (!ctx.started) return;
            
            Debug.unityLogger.Log(LogType.Log, "Shooting");
            
            Shoot();
        }
    }
}

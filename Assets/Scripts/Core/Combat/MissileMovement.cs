using UnityEngine;

namespace Core.Combat
{
    /// <summary>
    /// Handles a single missile movement logic and its components
    /// </summary>
    public class MissileMovement : MonoBehaviour
    {
        #region Variable

        [Tooltip("How fast a projectile should move")]
        [SerializeField] private float speed;
        
        private Vector3 _shootDirection;

        #endregion

        #region Methods

        // To prevent trail rendering on object pooling
        private void OnDisable()
        {
            var trail = GetComponentInChildren<TrailRenderer>();

            if (trail)
            {
                trail.Clear();
            }
        }

        public void OnInstantiate(Vector3 direction)
        {
            _shootDirection = direction;
        }

        private void Update()
        {
            transform.position += _shootDirection * (speed * Time.deltaTime);
        }

        #endregion
    }
}

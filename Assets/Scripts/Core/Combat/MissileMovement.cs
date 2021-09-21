using UnityEngine;

namespace Core.Combat
{
    public class MissileMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Vector3 _shootDirection;

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
    }
}

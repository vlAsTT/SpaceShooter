using System;
using UnityEngine;

namespace Core.Combat
{
    public class MissileMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Vector3 _shootDirection;

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
            // transform.eulerAngles = new Vector3(90f, transform.eulerAngles.y,GetAngleFromVector(_shootDirection));
        }

        private void Update()
        {
            transform.position += _shootDirection * (speed * Time.deltaTime);
        }
    }
}

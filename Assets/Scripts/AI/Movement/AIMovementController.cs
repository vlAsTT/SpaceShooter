using Core.Movement;
using UnityEngine;

namespace AI.Movement
{
    public class AIMovementController : MovementController
    {
        private Transform _followTarget;
        
        public void SetTarget(Transform followGoal)
        {
            _followTarget = followGoal;
        }

        protected override void Update()
        {
            base.Update();

            if (!_followTarget) return;

            transform.position = Vector3.MoveTowards(transform.position, _followTarget.position, Time.deltaTime * CurrentForwardSpeed);
            transform.LookAt(_followTarget);
        }
    }
}

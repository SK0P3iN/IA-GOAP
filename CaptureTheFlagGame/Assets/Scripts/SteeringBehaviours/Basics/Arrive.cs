using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Basics
{
    [RequireComponent(typeof(SteeringBasics))]
    [RequireComponent(typeof(Seek))]
    public class Arrive : MonoBehaviour
    {
        /// <summary>
        /// Target reference. Cannot be null
        /// </summary>
        [Tooltip("Target reference. Cannot be null")]
        public Transform Target;
        /// <summary>
        /// Indicates if we are following a target or not.
        /// </summary>
        [Tooltip("Indicates if we are following a target or not.")]
        public bool ArriveToTarget = false;

        /// <summary>
        /// Cached reference for the steering basics
        /// </summary>
        private SteeringBasics _steeringBasics;
        /// <summary>
        /// Cached reference for the rigid body
        /// </summary>
        private Rigidbody _rb;
        
        private void Awake()
        {
            _steeringBasics = GetComponent<SteeringBasics>();
            _rb = GetComponent<Rigidbody>();
            SteeringBasics.RbConstraints(_rb);
        }
        
        private void Update()
        {
            if (ArriveToTarget == false) return;

            var accel = GetSteering(Target.position);

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }

        /// <summary>
        /// Returns the steering for a character so it arrives at the target
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public Vector3 GetSteering(Vector3 targetPosition)
        {
            /* Get the right direction for the linear acceleration */
            var direction = targetPosition - transform.position;

            /* Get the distance to the target */
            var distance = direction.magnitude;

            /* If we are within the stopping radius then stop */
            if (distance < _steeringBasics.TargetRadius)
            {
                _rb.velocity = Vector3.zero;
                return Vector3.zero;
            }

            /* Calculate the ideal speed, full speed at slowRadius distance and 0 speed at 0 distance */
            float idealSpeed;
            if (distance > _steeringBasics.SlowRadius)
            {
                idealSpeed = _steeringBasics.MaxVelocity;
            }
            else
            {
                idealSpeed = _steeringBasics.MaxVelocity * (distance / _steeringBasics.SlowRadius);
            }

            /* Give velocity the correct speed */
            direction.Normalize();
            var velocity = direction * idealSpeed;
            
            /* Calculate the linear acceleration we want */
            var acceleration = velocity - _rb.velocity;

            /*
             Rather than accelerate the character to the correct speed in 1 second, 
             accelerate so we reach the desired speed in timeToTarget seconds 
             (if we were to actually accelerate for the full timeToTarget seconds).
            */
            acceleration *= 1 / _steeringBasics.TimeToTarget;

            /* Make sure we are accelerating at max acceleration */
            if (acceleration.sqrMagnitude > _steeringBasics.MaxAcceleration * _steeringBasics.MaxAcceleration)
            {
                acceleration.Normalize();
                acceleration = acceleration * _steeringBasics.MaxAcceleration;
            }

            return acceleration;
        }
    }
}

using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Basics
{
    [RequireComponent(typeof(SteeringBasics))]
    public class Flee : MonoBehaviour
    {
        /// <summary>
        /// Target to flee from
        /// </summary>
        [Tooltip("Target to flee from")]
        public Transform Target;

        /// <summary>
        /// Defines the distance from the target we start fleeing
        /// </summary>
        [Tooltip("Defines the distance from the target we start fleeing")]
        public float PanicDist = 10f;

        /// <summary>
        /// Perform a controlled deceleration
        /// </summary>
        [Tooltip("Perform a controlled deceleration")]
        public bool DecelerateOnStop = true;
        /// <summary>
        /// The time in which we want to achieve the targetSpeed
        /// </summary>
        [Tooltip("The time in which we want to achieve the targetSpeed")]
        public float TimeToTarget = 0.1f;

        /// <summary>
        /// Maximum acceleration added by this behaviour
        /// </summary>
        [Tooltip("Maximum acceleration added by this behaviour")]
        public float MaxAcceleration = 15f;

        /// <summary>
        /// Is this behaviour active?
        /// </summary>
        [Tooltip("Is this behaviour active?")]
        public bool IsFleeingTarget = false;

        /// <summary>
        /// Cached reference for the rigid body
        /// </summary>
        private Rigidbody _rb;

        /// <summary>
        /// Cached reference for the SteeringBasics
        /// </summary>
        private SteeringBasics _steeringBasics;
        
        private void Awake()
        {
            _steeringBasics = GetComponent<SteeringBasics>();
            _rb = GetComponent<Rigidbody>();
            SteeringBasics.RbConstraints(_rb);
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsFleeingTarget == false || Target == null) return;
            var accel = GetSteering(Target.position);

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }

        /// <summary>
        /// Gets the steering direction required to flee from the target.
        /// </summary>
        public Vector3 GetSteering(Vector3 targetPosition)
        {
            //Get the direction
            var direction = transform.position - targetPosition;

            //If the target is far way then don't flee
            if (direction.sqrMagnitude > PanicDist * PanicDist)
            {
                //Slow down if we should decelerate on stop
                if (DecelerateOnStop && _rb.velocity.magnitude > 0.001f)
                {
                    //Decelerate to zero velocity in time to target amount of time
                    var accelerationToStop = -_rb.velocity / TimeToTarget;

                    if (accelerationToStop.sqrMagnitude > MaxAcceleration * MaxAcceleration)
                        accelerationToStop = GiveMaxAccel(accelerationToStop);

                    return accelerationToStop;
                }

                _rb.velocity = Vector3.zero;
                return Vector3.zero;
            }

            //get the max acceleration
            var acceleration = GiveMaxAccel(direction);

            return acceleration;
        }

        /// <summary>
        /// Keep the Vector3 direction but change its magnitude to match the max acceleration
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        private Vector3 GiveMaxAccel(Vector3 velocity)
        {
            //normalize the velocity vector
            velocity.Normalize();

            //Accelerate to the target
            velocity *= MaxAcceleration;

            return velocity;
        }
    }
}
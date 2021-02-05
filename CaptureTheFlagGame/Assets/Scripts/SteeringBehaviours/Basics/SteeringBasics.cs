using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Basics
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public sealed class SteeringBasics : MonoBehaviour
    {
        /// <summary>
        /// Te maximum velocity
        /// </summary>
        [Header("Movements")]
        [Tooltip("The maximum velocity")]
        public float MaxVelocity = 3.5f;
        /// <summary>
        /// The maximum acceleration
        /// </summary>
        [Tooltip("The maximum acceleration")]
        public float MaxAcceleration = 10f;
        /// <summary>
        /// The radius from the target that means we are close enough and have arrived
        /// </summary>
        [Tooltip("The radius from the target that means we are close enough and have arrived")]
        public float TargetRadius = 0.005f;
        /// <summary>
        /// The radius from the target where we start to slow down
        /// </summary>
        [Tooltip("The radius from the target where we start to slow down")]
        public float SlowRadius = 1f;
        /// <summary>
        /// The time in which we want to achieve the targetSpeed
        /// </summary>
        [Tooltip("The time in which we want to achieve the targetSpeed")]
        public float TimeToTarget = 0.1f;
        /// <summary>
        /// The turning speed
        /// </summary>
        [Tooltip("The turning speed")]
        public float TurnSpeed = 20f;

        /// <summary>
        /// Smooths the character movement
        /// </summary>
        [Header("Smoothing")]
        [Tooltip("Smooths the character movement")]
        public bool Smoothing = true;
        /// <summary>
        /// Number of samples used for the smoothing
        /// </summary>
        [Tooltip("Number of samples used for the smoothing")]
        public int NumSamplesForSmoothing = 5;
        /// <summary>
        /// Velocities used for the smoothing
        /// </summary>
        private readonly Queue<Vector3> _velocitySamples = new Queue<Vector3>();
        /// <summary>
        /// Cached reference for this character's rigid body
        /// </summary>
        private Rigidbody _rb;

        // Use this for initialization
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        
        /// <summary>
        /// Updates the velocity of the current game object by the given linear acceleration
        /// </summary>
        /// <param name="linearAcceleration"></param>
        public void Steer(Vector3 linearAcceleration)
        {
            //We go from point A to point B in a certain amount of time
            _rb.velocity += linearAcceleration * Time.deltaTime;

            //if our new velocity is greater than the maximum velocity specified in out property then we set the new velocity equal to the max velocity
            if (_rb.velocity.sqrMagnitude > MaxVelocity * MaxVelocity)
            {
                _rb.velocity = _rb.velocity.normalized * MaxVelocity;
            }
        }

        /// <summary>
        /// Rotates "this" transform to the given direction
        /// </summary>
        /// <param name="direction"></param>
        public void Face(Vector3 direction)
        {
            direction.Normalize();
            // If we have a non-zero direction then look towards that direciton otherwise do nothing
            if (direction.sqrMagnitude < 0.001f) return;
            
            var toRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime * TurnSpeed);

            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }

        /// <summary>
        /// Rotates "this" transform to the given direction
        /// </summary>
        /// <param name="toRotation"></param>
        public void Face(Quaternion toRotation)
        {
            Face(toRotation.eulerAngles.y);
        }

        /// <summary>
        /// Rotates "this" transform to the given direction
        /// </summary>
        /// <param name="toRotation"></param>
        public void Face(float toRotation)
        {
            var rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime * TurnSpeed);

            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }


        /// <summary>
        /// Makes the current game object look where he is going
        /// </summary>
        public void LookWhereYoureGoing()
        {
            var direction = _rb.velocity;

            if (Smoothing)
            {
                if (_velocitySamples.Count == NumSamplesForSmoothing)
                {
                    _velocitySamples.Dequeue();
                }

                _velocitySamples.Enqueue(_rb.velocity);

                direction = Vector3.zero;

                foreach (var v in _velocitySamples)
                {
                    direction += v;
                }

                direction /= _velocitySamples.Count;
            }
            
            Face(direction);
        }
        
        /// <summary>
        /// Checks to see if the target is in front of the character
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsInFront(Vector3 target)
        {
            return IsFacing(target, 0);
        }

        /// <summary>
        /// Checks to see if the target is in front of the character
        /// </summary>
        public bool IsFacing(Vector3 target, float cosineValue)
        {
            var facing = transform.forward.normalized;

            var directionToTarget = (target - transform.position);
            directionToTarget.Normalize();

            return Vector3.Dot(facing, directionToTarget) >= cosineValue;
        }


        public static void RbConstraints(Rigidbody rb)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        public void Stop()
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
}
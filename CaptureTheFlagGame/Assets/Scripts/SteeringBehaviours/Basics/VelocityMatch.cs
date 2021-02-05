using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Basics
{
    [RequireComponent(typeof(SteeringBasics))]
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityMatch : MonoBehaviour
    {
        /// <summary>
        /// Angle to be facing
        /// </summary>
        [Tooltip("Angle to be facing")]
        public float FacingCosine = 90;
        /// <summary>
        /// Used for controlled acceleration
        /// </summary>
        [Tooltip("Used for controlled acceleration")]
        public float TimeToTarget = 0.1f;
        /// <summary>
        /// The maximum acceleration
        /// </summary>
        [Tooltip("The maximum acceleration")]
        public float MaxAcceleration = 4f;

        /// <summary>
        /// The cached cosine result
        /// </summary>
        private float _facingCosineVal;

        /// <summary>
        /// Cached reference for the steering basics
        /// </summary>
        private SteeringBasics _steeringBasics;
        /// <summary>
        /// Cached reference for the rigid body
        /// </summary>
        private Rigidbody _rb;

        // Use this for initialization
        private void Awake()
        {
            _facingCosineVal = Mathf.Cos(FacingCosine * Mathf.Deg2Rad);

            _rb = GetComponent<Rigidbody>();
            _steeringBasics = GetComponent<SteeringBasics>();

            SteeringBasics.RbConstraints(_rb);
        }

        /// <summary>
        /// Ge the required acceleration to match the velocity of all my targets
        /// </summary>
        public Vector3 GetSteering(ICollection<Rigidbody> targets)
        {
            var accel = Vector3.zero;
            var count = 0;

            foreach (var r in targets)
            {
                if (_steeringBasics.IsFacing(r.position, _facingCosineVal))
                {
                    /* Calculate the acceleration we want to match this target */
                    var a = r.velocity - _rb.velocity;
                    /*
                     Rather than accelerate the character to the correct speed in 1 frame, 
                     accelerate so we reach the desired speed in timeToTarget seconds 
                     (if we were to actually accelerate for the full timeToTarget seconds).
                    */
                    a /= TimeToTarget;

                    accel += a;

                    count++;
                }
            }

            if (count > 0)
            {
                accel = accel / count;

                /* Make sure we are accelerating at max acceleration */
                if (accel.sqrMagnitude > MaxAcceleration * MaxAcceleration)
                {
                    accel = accel.normalized * MaxAcceleration;
                }
            }

            return accel;
        }
    }
}
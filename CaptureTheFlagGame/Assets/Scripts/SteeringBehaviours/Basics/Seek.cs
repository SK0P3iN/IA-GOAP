using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Basics
{
    [RequireComponent(typeof(SteeringBasics))]
    public class Seek : MonoBehaviour
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
        public bool IsSeekingTarget = false;

        /// <summary>
        /// Cached reference for the steering basics
        /// </summary>
        private SteeringBasics _steeringBasics;
        
        private void Awake()
        {
            _steeringBasics = GetComponent<SteeringBasics>();
            SteeringBasics.RbConstraints(GetComponent<Rigidbody>());
        }
        
        private void Update()
        {
            if (IsSeekingTarget == false) return;

            var accel = GetSteering(Target.position);

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }

        /// <summary>
        /// A seek steering behavior. Will return the steering for the current game object to seek a given position
        /// </summary>
        public Vector3 GetSteering(Vector3 targetPosition, float maxSeekAccel)
        {
            //Get the direction
            var acceleration = targetPosition - transform.position;
            
            //normalize the acceleration
            acceleration.Normalize();

            //Accelerate to the target
            acceleration *= maxSeekAccel;

            return acceleration;
        }

        /// <summary>
        /// A seek steering behavior. Will return the steering for the current game object to seek a given position. Assumes default max acceleration.
        /// </summary>
        public Vector3 GetSteering(Vector3 targetPosition)
        {
            return GetSteering(targetPosition, _steeringBasics.MaxAcceleration);
        }
    }
}

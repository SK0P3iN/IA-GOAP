using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Advanced
{
    public class WanderTarget : MonoBehaviour
    {
        /* The radius of the wander circle */
        [Tooltip("The radius of the wander circle")]
        public float WanderRadius = 1.2f;

        /* The distance we are wandering around the target */
        [Tooltip("The distance we are wandering around the target")]
        public float WanderDistance = 2f;

        //maximum amount of random displacement a second
        [Tooltip("maximum amount of random displacement a second")]
        public float WanderJitter = 800f;

        /*The target to wander around*/
        [Tooltip("The target to wander around")]
        public Transform WanderTargetTransform;

        [Tooltip("True if we are using this behavior in the update")]
        public bool IsWanderingTarget = true;

        private Vector3 _wanderTargetPosition;

        /// <summary>
        /// Cached SteeringBasics component
        /// </summary>
        private SteeringBasics _steeringBasics;

        /// <summary>
        /// Cached Seek component
        /// </summary>
        private Seek _seek;

        private void Awake()
        {
            _steeringBasics = GetComponent<SteeringBasics>();
            _seek = GetComponent<Seek>();
            SteeringBasics.RbConstraints(GetComponent<Rigidbody>());
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (IsWanderingTarget == false) return;

            var accel = GetSteering();

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }


        /// <summary>
        /// The Wander behavior. Will return a steering based on its current postion and the its target
        /// </summary>
        /// <returns></returns>
        public Vector3 GetSteering()
        {
            _wanderTargetPosition = WanderTargetTransform.position;

            //get the jitter for this time frame
            var jitter = WanderJitter * Time.deltaTime;

            //add a small random vector to the target's position
            _wanderTargetPosition += new Vector3(Random.Range(-1f, 1f) * jitter, 0, Random.Range(-1f, 1f) * jitter);

            //make the wanderTarget fit on the wander circle again
            _wanderTargetPosition *= WanderRadius;

            //move the target in front of the character
            var targetPosition = transform.forward * WanderDistance + _wanderTargetPosition;

            // TODO this line is not optimal and should be only used for debugging
            Debug.DrawLine(transform.position, targetPosition);

            return _seek.GetSteering(targetPosition);
        }
    }
}
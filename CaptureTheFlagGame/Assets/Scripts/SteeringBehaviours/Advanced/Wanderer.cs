using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Advanced
{
    [RequireComponent(typeof(SteeringBasics))]
    [RequireComponent(typeof(Seek))]
    public class Wanderer : MonoBehaviour
    {
        /* The forward offset of the wander square */
        [Tooltip("The forward offset of the wander square")]
        public float WanderOffset = 1.5f;

        /* The radius of the wander square */
        [Tooltip("The radius of the wander square")]
        public float WanderRadius = 4;

        /* The rate at which the wander orientation can change */
        [Tooltip("The rate at which the wander orientation can change")]
        public float WanderRate = 0.4f;


        private float _wanderOrientation;

        /// <summary>
        /// Cached SteeringBasics component
        /// </summary>
        private SteeringBasics _steeringBasics;

        /// <summary>
        /// Cached Seek component
        /// </summary>
        private Seek _seek;

        //private GameObject debugRing;

        private void Awake()
        {
            //DebugDraw debugDraw = gameObject.GetComponent<DebugDraw> ();
            //debugRing = debugDraw.createRing (Vector3.zero, wanderRadius);

            _steeringBasics = GetComponent<SteeringBasics>();
            _seek = GetComponent<Seek>();
            SteeringBasics.RbConstraints(GetComponent<Rigidbody>());
        }

        // Update is called once per frame
        private void Update()
        {
            var accel = GetSteering();

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }

        /// <summary>
        /// The Wander behavior. Will return a steering based on its current postion and the radius where it can wander
        /// </summary>
        /// <returns></returns>
        public Vector3 GetSteering()
        {
            var characterOrientation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

            /* Update the wander orientation */
            _wanderOrientation += RandomBinomial() * WanderRate;

            /* Calculate the combined target orientation */
            var targetOrientation = _wanderOrientation + characterOrientation;

            /* Calculate the center of the wander circle */
            var targetPosition = transform.position + OrientationToVector(characterOrientation) * WanderOffset;

            //debugRing.transform.position = targetPosition;

            /* Calculate the target position */
            targetPosition = targetPosition + OrientationToVector(targetOrientation) * WanderRadius;

            //Debug.DrawLine (transform.position, targetPosition);

            return _seek.GetSteering(targetPosition);
        }

        /// <summary>
        /// Returns a random number between -1 and 1. Values around zero are more likely.
        /// </summary>
        /// <returns></returns>
        private float RandomBinomial()
        {
            return Random.value - Random.value;
        }

        /// <summary>
        /// Returns the orientation as a unit vector
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        private Vector3 OrientationToVector(float orientation)
        {
            return new Vector3(Mathf.Cos(orientation), 0, Mathf.Sin(orientation));
        }
    }
}
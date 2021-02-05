using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Advanced
{
    public class Pursue : MonoBehaviour
    {
        /// <summary>
        /// Maximum prediction time the pursue will predict in the future
        /// </summary>
        [Tooltip("Maximum prediction time the pursue will predict in the future")]
        public float MaxPrediction = 1f;

        /// <summary>
        /// "Our target's rigidbody reference"
        /// </summary>
        [Tooltip("Our target's rigidbody reference")]
        public Rigidbody Target;

        /// <summary>
        /// Cached reference of my rigidbody
        /// </summary>
        private Rigidbody _rb;
        /// <summary>
        /// Cached reference for the steering basics
        /// </summary>
        private SteeringBasics _steeringBasics;
        /// <summary>
        /// Cached reference for the seek behavior
        /// </summary>
        private Seek _seek;

        public bool IsPursuing;

        // Use this for initialization
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _steeringBasics = GetComponent<SteeringBasics>();
            _seek = GetComponent<Seek>();
            SteeringBasics.RbConstraints(_rb);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (IsPursuing == false) return;

            var accel = GetSteering(Target);

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }

        /// <summary>
        /// The Pursue behavior. Will return a steering based on prediction of the movement of the target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Vector3 GetSteering(Rigidbody target)
        {
            /* Calculate the distance to the target */
            var direction = target.position - transform.position;
            var distance = direction.magnitude;

            /* Get the character's speed */
            var speed = _rb.velocity.magnitude;

            /* Calculate the prediction time */
            float prediction;
            if (speed <= distance / MaxPrediction)
                prediction = MaxPrediction;
            else
                prediction = distance / speed;

            /* Put the target together based on where we think the target will be */
            var predictedTarget = target.position + target.velocity * prediction;

            return _seek.GetSteering(predictedTarget);
        }
    }
}
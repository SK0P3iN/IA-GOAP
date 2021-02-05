using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Advanced
{
    [RequireComponent(typeof(SteeringBasics))]
    [RequireComponent(typeof(Flee))]
    public class Evade : MonoBehaviour
    {
        /// <summary>
        /// Maximum prediction time the pursue will predict in the future
        /// </summary>
        [Tooltip("Maximum prediction time the pursue will predict in the future")]
        public float MaxPrediction = 1f;
        /// <summary>
        /// Our target's rigidbody reference
        /// </summary>
        [Tooltip("Our target's rigidbody reference")]
        public Rigidbody Target;
        /// <summary>
        /// Check if we are using this behavior in the update
        /// </summary>
        [Tooltip("Check if we are using this behavior in the update")]
        public bool IsEvadingTarget = true;

        /// <summary>
        /// Cached reference for the Flee
        /// </summary>
        private Flee _flee;
        /// <summary>
        /// Cached reference for the steering basics
        /// </summary>
        private SteeringBasics _steeringBasics;

        // Use this for initialization
        private void Start()
        {
            _flee = GetComponent<Flee>();
            _steeringBasics = GetComponent<SteeringBasics>();
            SteeringBasics.RbConstraints(GetComponent<Rigidbody>());
        }

        private void FixedUpdate()
        {
            if (IsEvadingTarget == false || Target == null) return;

            var accel = GetSteering(Target);

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }

        /// <summary>
        /// The Evade behavior. Will return a steering based on prediction of the best movement away from the target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Vector3 GetSteering(Rigidbody target)
        {
            /* Calculate the distance to the target */
            var direction = target.position - transform.position;
            var distance = direction.magnitude;

            /* Get the targets's speed */
            var speed = target.velocity.magnitude;

            /* Calculate the prediction time */
            float prediction;
            if (speed <= distance / MaxPrediction)
            {
                prediction = MaxPrediction;
            }
            else
            {
                prediction = distance / speed;
                //Place the predicted position a little before the target reaches the character
                prediction *= 0.9f;
            }

            /* Put the target together based on where we think the target will be */
            var predictedTargetLocation = target.position + target.velocity * prediction;

            return _flee.GetSteering(predictedTargetLocation);
        }
    }
}
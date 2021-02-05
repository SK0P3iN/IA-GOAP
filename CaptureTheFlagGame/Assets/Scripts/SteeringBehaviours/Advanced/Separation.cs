using System.Collections.Generic;
using Assets.Scripts.SteeringBehaviours.Basics;
using Assets.Scripts.SteeringBehaviours.Utils;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Advanced
{
    [RequireComponent(typeof(ObjectCollisionProps))]
    public class Separation : MonoBehaviour
    {
        /// <summary>
        /// The maximum acceleration for separation
        /// </summary>
        [Tooltip("The maximum acceleration for separation")]
        public float SepMaxAcceleration = 3;

        /// <summary>
        /// This should be the maximum separation distance possible between a separation target and the character. 
        /// </summary>
        [Tooltip("This should be the maximum separation distance possible between a separation target and the character. ")]
        public float MaxSepDist = 1f;

        private ObjectCollisionProps _boundingRadius;

        // Use this for initialization
        private void Awake()
        {
            _boundingRadius = GetComponent<ObjectCollisionProps>();
            SteeringBasics.RbConstraints(GetComponent<Rigidbody>());
        }

        /// <summary>
        /// The Separation behavior. Will return a steering based all the agents around it
        /// </summary>
        /// <returns></returns>
        public Vector3 GetSteering(ICollection<Rigidbody> agents)
        {
            if (agents == null || agents.Count == 0) return Vector3.one;


            var acceleration = Vector3.zero;

            foreach (var r in agents)
            {
                //Get the direction and distance from this agent
                var direction = transform.position - r.position;
                var dist = direction.magnitude;

                if (dist > MaxSepDist) continue; // already far enough

                var targetRadius = r.GetComponent<ObjectCollisionProps>().BodyRadius;

                //Calculate the separation strength.  Our body radius and the target body radius are removed from the strength
                var strength = SepMaxAcceleration * (MaxSepDist - dist) / (MaxSepDist - _boundingRadius.BodyRadius - targetRadius);

                //Added separation acceleration to the existing steering
                direction.Normalize();
                acceleration += direction * strength;
            }

            return acceleration;
        }
    }
}
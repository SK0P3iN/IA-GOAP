using System.Collections.Generic;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Advanced
{
    [RequireComponent(typeof(SteeringBasics))]
    [RequireComponent(typeof(Arrive))]
    public class Cohesion : MonoBehaviour
    {
        /// <summary>
        /// The facing angle in degrees
        /// </summary>
        [Tooltip("The facing angle")]
        public float FacingCosine = 30f;


        /// <summary>
        /// The cached calculated cosine value
        /// </summary>
        private float _facingCosineVal;
        /// <summary>
        /// Cached reference for the steering basics
        /// </summary>
        private SteeringBasics _steeringBasics;
        /// <summary>
        /// Cached reference for the Arrive component
        /// </summary>
        private Arrive _arrive;

        // Use this for initialization
        private void Awake()
        {
            _facingCosineVal = Mathf.Cos(FacingCosine * Mathf.Deg2Rad);
            _steeringBasics = GetComponent<SteeringBasics>();
            _arrive = GetComponent<Arrive>();
            SteeringBasics.RbConstraints(GetComponent<Rigidbody>());
        }

        /// <summary>
        /// The Cohesion behavior. Returns the required acceleration for the cohesion behaviour
        /// </summary>
        public Vector3 GetSteering(ICollection<Rigidbody> agents)
        {
            if (agents == null || agents.Count == 0) return Vector3.zero;

            var centerOfMass = Vector3.zero;
            var count = 0;

            

            /* Sums up everyone's position who is close enough and in front of the character */
            foreach (var r in agents)
            {
                if (_steeringBasics.IsFacing(r.position, _facingCosineVal) == false) continue;

                centerOfMass += r.position;
                count++;
            }

            return count == 0 ? Vector3.zero : _arrive.GetSteering(centerOfMass / count);
        }
    }
}
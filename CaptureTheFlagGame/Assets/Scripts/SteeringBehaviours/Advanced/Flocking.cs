using Assets.Scripts.SteeringBehaviours.Basics;
using Assets.Scripts.SteeringBehaviours.Utils;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Advanced
{
    [RequireComponent(typeof(SteeringBasics))]
    [RequireComponent(typeof(Wanderer))]
    [RequireComponent(typeof(Cohesion))]
    [RequireComponent(typeof(Separation))]
    [RequireComponent(typeof(NearSensor))]
    public class Flocking : MonoBehaviour
    {
        /// <summary>
        /// The additional cohesion weight to add to the Cohesion behavior
        /// </summary>
        [Tooltip("The additional cohesion weight to add to the Cohesion behavior")]
        public float CohesionWeight = 500f;
        /// <summary>
        /// The additional separation weight to add to the separation behavior
        /// </summary>
        [Tooltip("The additional separation weight to add to the separation behavior")]
        public float SeparationWeight = 2f;


        /// <summary>
        /// Cached SteeringBasics component
        /// </summary>
        private SteeringBasics _steeringBasics;
        /// <summary>
        /// Cached WanderTarget component
        /// </summary>
        private Wanderer _wander;
        /// <summary>
        /// Cached Cohesion component
        /// </summary>
        private Cohesion _cohesion;
        /// <summary>
        /// Cached Separation component
        /// </summary>
        private Separation _separation;
        /// <summary>
        /// Cached NearSensor component
        /// </summary>
        private NearSensor _sensor;

        // Use this for initialization
        private void Start()
        {
            _steeringBasics = GetComponent<SteeringBasics>();
            _wander = GetComponent<Wanderer>();
            _cohesion = GetComponent<Cohesion>();
            _separation = GetComponent<Separation>();
            _sensor = GetComponent<NearSensor>();
            SteeringBasics.RbConstraints(GetComponent<Rigidbody>());
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            var accel = Vector3.zero;

            accel += _cohesion.GetSteering(_sensor.Targets) * CohesionWeight;
            accel += _separation.GetSteering(_sensor.Targets) * SeparationWeight;
            accel += _wander.GetSteering();

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }
    }
}
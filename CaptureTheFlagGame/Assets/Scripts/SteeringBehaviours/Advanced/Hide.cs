using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.SteeringBehaviours.Basics;
using Assets.Scripts.SteeringBehaviours.Utils;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Advanced
{
    [RequireComponent(typeof(SteeringBasics))]
    [RequireComponent(typeof(Evade))]
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(WallAvoidance))]
    public class Hide : MonoBehaviour
    {
        /// <summary>
        /// Our target's rigidbody reference
        /// </summary>
        [Tooltip("Our target's rigidbody reference")]
        public Rigidbody Target;
        /// <summary>
        /// List of objects that we can use to hide
        /// </summary>
        [Tooltip("List of objects that we can use to hide")]
        public List<Rigidbody> Objs;

        private SteeringBasics _steeringBasics;
        private Arrive _arrive;
        private Evade _evade;
        private WallAvoidance _wallAvoid;

        // Use this for initialization
        private void Awake()
        {
            _steeringBasics = GetComponent<SteeringBasics>();
            _arrive = GetComponent<Arrive>();
            _evade = GetComponent<Evade>();
            _wallAvoid = GetComponent<WallAvoidance>();
            SteeringBasics.RbConstraints(GetComponent<Rigidbody>());
            Objs = GameObject.FindGameObjectsWithTag("HidingSpot").Select(h => h.GetComponent<Rigidbody>()).ToList();
        }
        // Update is called once per frame
        private void Update()
        {
            //get the best hidePosition
            var hideAccel = GetSteering(Target, Objs, out var hidePosition);

            //make sure we avoid the walls when we move to the hidding spot
            var accel = _wallAvoid.GetSteering(hidePosition - transform.position);

            //if the acceleration from the wall avoidance is too small we use the one returned by out Hide behavior
            if (accel.magnitude < 0.005f) 
                accel = hideAccel;

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }

        /// <summary>
        /// The Evade behavior. Will return a steering based on prediction of the best movement away from the target
        /// </summary>
        /// <param name="target">The target we want to hide from</param>
        /// <param name="obstacles">The list of obstacles that we can use to hide on</param>
        /// <param name="bestHidingSpot">The best place to hide</param>
        /// <returns></returns>
        public Vector3 GetSteering(Rigidbody target, ICollection<Rigidbody> obstacles, out Vector3 bestHidingSpot)
        {
            //Find the closest hiding spot
            var distToClosest = Mathf.Infinity;
            bestHidingSpot = Vector3.zero;
            foreach (var obstacle in obstacles)
            {
                var hidingPositionSpot = GetHidingPosition(obstacle, target);
                var dist = Vector3.Distance(hidingPositionSpot, transform.position);
                //Gizmos.DrawSphere(hidingPositionSpot, 1);

                //checks if the distance of this obstacle is less than the last closest obstacle saved
                if (dist < distToClosest)
                {
                    distToClosest = dist;
                    bestHidingSpot = hidingPositionSpot;
                }
            }

            //If no hiding spot is found then just evade the enemy
            if (float.IsPositiveInfinity(distToClosest))
                return _evade.GetSteering(target);
            
            return _arrive.GetSteering(bestHidingSpot);
        }


        private Vector3 GetHidingPosition(Rigidbody obstacle, Rigidbody target)
        {
            if(target == null || obstacle == null) return Vector3.zero;
            ;
            //get the bodyRadius from my obstacle
            var distAway = obstacle.GetComponent<ObjectCollisionProps>().BodyRadius;

            //get the direction to hide
            var dir = obstacle.position - target.position;
            dir.Normalize();
            
            //get the position of the obstacle
            return obstacle.position + dir * distAway;
        }
    }
}
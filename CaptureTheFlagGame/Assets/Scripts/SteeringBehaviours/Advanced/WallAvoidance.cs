using System.Collections.Generic;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Advanced
{
    [RequireComponent(typeof(SteeringBasics))]
    [RequireComponent(typeof(Seek))]
    public class WallAvoidance : MonoBehaviour
    {
        /* The minimum distance to a wall to know how far to avoid the collision. Should be greated than the radius of the character */
        [Tooltip("The minimum distance to a wall to know how far to avoid the collision. Should be greated than the radius of the character")]
        public float WallAvoidDistance = 0.5f;

        /* How far ahead should we look for a collision */
        [Tooltip("How far ahead should we look for a collision")]
        public float LookAheadDistance = 1.25f;

        /* How far to the side the ray should extend */
        [Tooltip("How far to the side the ray should extend")]
        public float LookToSideDistance = 0.701f;

        /* How much is the agle to look to the side */
        [Tooltip("How much is the agle to look to the side")]
        public float LookToSideAngle = 45f;

        /* The maximum acceleration */
        [Tooltip("The maximum acceleration")]
        public float MaxAcceleration = 40f;

        /* True if we want to use the wall avoidance each update */
        [Tooltip("True if we want to use the wall avoidance each update")]
        public bool AvoidingWallAllTheTime = true;

        private Rigidbody _rb;
        private SteeringBasics _steeringBasics;
        private Seek _seek;


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
            if (AvoidingWallAllTheTime == false) return;

            var accel = GetSteering();

            _steeringBasics.Steer(accel);
            _steeringBasics.LookWhereYoureGoing();
        }

        /// <summary>
        /// The WallAvoidance behavior. Will return a steering based on finding collisions and avoid them
        /// </summary>
        /// <param name="facingDir">The direction we are facing</param>
        /// <returns></returns>
        public Vector3 GetSteering(Vector3 facingDir)
        {
            var acceleration = Vector3.zero;

            /* Creates the ray direction vector */
            var rayDirs = new Vector3[3];
            rayDirs[0] = facingDir.normalized;

            var orientation = Mathf.Atan2(_rb.velocity.x, _rb.velocity.z);

            rayDirs[1] = OrientationToVector(orientation + LookToSideAngle * Mathf.Deg2Rad);
            rayDirs[2] = OrientationToVector(orientation - LookToSideAngle * Mathf.Deg2Rad);

            /* If no collision do nothing */
            if (FindObstacle(rayDirs, out var hit) == false)
            {
                return acceleration;
            }

            /* Create a target away from the wall to seek */
            var targetPosition = hit.point + hit.normal * WallAvoidDistance;

            /* If velocity and the collision normal are parallel then move the target a bit to the left or right of the normal */
            var cross = Vector3.Cross(_rb.velocity, hit.normal);
            if (cross.magnitude < 0.005f)
            {
                targetPosition +=  new Vector3(-hit.normal.z, hit.normal.y, hit.normal.x);
            }

            return _seek.GetSteering(targetPosition, MaxAcceleration);
        }

        /// <summary>
        /// The WallAvoidance behavior. Will return a steering based on finding collisions and avoiding them. Uses the chracter's ridigbody as the facing direction
        /// </summary>
        /// <returns></returns>
        public Vector3 GetSteering()
        {
            if (_rb.velocity.magnitude > 0.005f)
            {
                return GetSteering(_rb.velocity);
            }
            else
            {
                var rotInRad = _rb.rotation.eulerAngles.y * Mathf.Deg2Rad;
                return GetSteering(new Vector3(Mathf.Cos(rotInRad), Mathf.Sin(rotInRad), 0));
            }
        }

        /// <summary>
        /// Returns the orientation as a unit vector
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        private Vector3 OrientationToVector(float orientation)
        {
            return new Vector3(Mathf.Cos(orientation), Mathf.Sin(orientation), 0);
        }

        /// <summary>
        /// Checks if any of the rays received hit an obstacle
        /// </summary>
        /// <param name="rayDirs">List of raycasts</param>
        /// <param name="firstHit">Saves here the first raycast that hit</param>
        /// <returns></returns>
        private bool FindObstacle(IReadOnlyList<Vector3> rayDirs, out RaycastHit firstHit)
        {
            firstHit = new RaycastHit();
            var foundObs = false;

            var position = transform.position;
            //Checks if any of the rays hits
            for (var i = 0; i < rayDirs.Count; i++)
            {
                var rayDist = i == 0 ? LookAheadDistance : LookToSideDistance;

                if (Physics.Raycast(position, rayDirs[i], out var hit, rayDist))
                {
                    foundObs = true;
                    firstHit = hit;
                    break;
                }

                Debug.DrawLine(position, position + rayDirs[i] * rayDist);
            }

            return foundObs;
        }
    }
}
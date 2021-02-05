using Assets.Scripts.Pathfinding.Scripts.AStar;
using Assets.Scripts.Pathfinding.Scripts.Grid;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.AI_Team.Pathfinding
{
    public class PathfindingUnit : MonoBehaviour, IPathfindingUnit
    {
        /// <summary>
        /// The minimum distance the target has to move before we recalculate a new path to it
        /// </summary>
        [Tooltip("minimum distance the target has to move before we recalculate a new path to it")]
        private const float PathUpdateMoveThreashold = .5f;
        /// <summary>
        /// The minimum time we wait before recalculating the path.
        /// </summary>
        [Tooltip("The minimum time we wait before recalculating the path.")]
        private const float MinPathUpdateTime = .2f;
        [Tooltip("Distance from the final node where we start to slowdown")]
        public float StoppingDist = 10;
        [Tooltip("The distance from the nodes where we start turning. The bigger the distance, the more time we will have to turn.")]
        public float TurnDist = 5;
        /// <summary>
        /// The target we are following
        /// </summary>
        [Tooltip("The target we are following")]
        public Transform Target;

        /// <summary>
        /// true if is following path
        /// </summary>
        public bool FollowingPath { get; set; }

        public bool Arrived
        {
            get => _arrived;
            set => _arrived = value;
        }

        /// <summary>
        /// The current path we are following
        /// </summary>
        private Path _path;
        /// <summary>
        /// The previous target position
        /// </summary>
        private Vector3 _previousTargetPosition = Vector3.zero;
        /// <summary>
        /// How much the target must move before we request an update
        /// </summary>
        private float _sqrMoveThreshold;

        /// <summary>
        /// Cache of this unit transform for optimization
        /// </summary>
        private Transform _myTransform;
        /// <summary>
        /// My current path request
        /// </summary>
        private PathRequest _myPathRequest;
        /// <summary>
        /// current path index
        /// </summary>
        private int _pathIndex;

        public bool DrawWithGizmos;


        private Seek _seek;
        private SteeringBasics _steeringBasics;
        [SerializeField] private bool _arrived;
        private Transform _target;

        private void Awake()
        {
            _myTransform = transform;
            _seek = GetComponent<Seek>();
            _steeringBasics = GetComponent<SteeringBasics>();
            _sqrMoveThreshold = PathUpdateMoveThreashold * PathUpdateMoveThreashold;
        }


        /// <summary>
        /// Callback for the path found. If it was successful, update that path. If not and we have a target, request a new one.
        /// </summary>
        private void OnPathFound(Node[] waypoints, bool pathSuccessful)
        {
            if (pathSuccessful == false)
            {
                if (_target != null)
                    RequestPath(_target.transform, true);

                _steeringBasics.Stop();
                return;
            }
            _pathIndex = 0;
            _path = new Path(waypoints, _myTransform.position, TurnDist, StoppingDist);
        }

        /// <summary>
        /// Do validations to see if we need to request a new path.
        /// </summary>
        /// <param name="target"> The target</param>
        /// <param name="forceRequest"> Forces the request of a new path</param>
        public void RequestPath(Transform target, bool forceRequest = false)
        {
            // if the target has moved more than PathUpdateMoveThreashold, then request a new Path
            if (target != null && ((target.position - _previousTargetPosition).sqrMagnitude > _sqrMoveThreshold || forceRequest || _path != null && _path.FinishLineIndex == -1))
            {
                //print("requesting path ");
                if (_myPathRequest == null)
                    _myPathRequest = new PathRequest(_myTransform.position, target.position, OnPathFound);
                else
                    _myPathRequest.UpdatePathRequest(_myTransform.position, target.position, OnPathFound);

                PathRequestManager.Instance.RequestPath(_myPathRequest);
                // update the previous position
                _previousTargetPosition = target.position;
            }
        }

        /// <summary>
        /// returns true while still following a path. Returns false when it arrives
        /// </summary>
        /// <returns></returns>
        public bool DoFollowPathStep()
        {
            RequestPath(_target.transform);
            var followingPath = true;
            var pos2D = Path.Vector3ToVector2(_myTransform.position);

            if (_path == null) // we are still waiting for the path to be calculated
                return true; // we are technically still following the path (we have not arrived)

            if (_pathIndex > _path.TurnBoundaries.Length || _path.FinishLineIndex == -1)
            {
                _pathIndex = 0;
                return true;
            }

            // loops through the turn boundaries till it founds the last boundary it has crossed. 
            // This solves the pathing problem if we move through several turn boundaries for each turn.
            while (_path.TurnBoundaries[_pathIndex].HasCrossedLine(pos2D))
            {
                // we arrived at the end of the path
                if (_pathIndex >= _path.FinishLineIndex)
                {
                    break;
                }

                _pathIndex++;
            }
            //print(Vector3.Distance(transform.position, _target.position));

            if (Vector3.Distance(transform.position, _target.position) < 2f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                followingPath = false;
            }

            if (followingPath)
            {
                var acc = _seek.GetSteering(_path.Waypoints[_pathIndex]);
                //if (Math.Abs(acc.x) < 0.01f && Math.Abs(acc.z) < 0.01f) // something went wrong with the pathfinding and we are stuck. Do a new plan.
                //{
                //    print("REQUESTING NEW PATH");
                //    if (_target != null)
                //        RequestPath(_target.transform, true);
                //}
                _steeringBasics.Steer(acc);
                _steeringBasics.LookWhereYoureGoing();
            }
            return followingPath;
        }

        /// <summary>
        /// Sets the target
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform target)
        {
            if (_target != target)
                RequestPath(target.transform, true);
            Target = target; // debug
            _target = target;
        }


        /// <summary>
        /// Notify the listners if the grid has changed
        /// </summary>
        public void Notify()
        {
            if (_path == null) return;

            if (_path.PathValuesHasChanged(_pathIndex, 5) && _target != null)
                RequestPath(_target.transform, true);
        }


        ///// <summary>
        ///// Gets the target
        ///// </summary>
        ///// <returns></returns>
        //public Transform GetTarget()
        //{
        //    return _target;
        //}

        /// <summary>
        /// Visualize with gizmos
        /// </summary>
        private void OnDrawGizmos()
        {
            if (_path != null && DrawWithGizmos)
            {
                _path.DrawWithGizmos();
            }
        }
    }
}
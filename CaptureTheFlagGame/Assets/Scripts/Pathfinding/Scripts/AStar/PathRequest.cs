using System;
using Assets.Scripts.Pathfinding.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Pathfinding.Scripts.AStar
{
    /// <summary>
    /// Represents the request for a new path
    /// </summary>
    public class PathRequest
    {
        /// <summary>
        /// Start point
        /// </summary>
        public Vector3 PathStart;
        /// <summary>
        /// End point
        /// </summary>
        public Vector3 PathEnd;
        /// <summary>
        /// Callback to be invoked when the path is found
        /// </summary>
        public Action<Node[], bool> Callback;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Node[], bool> callback)
        {
            PathStart = pathStart;
            PathEnd = pathEnd;
            Callback = callback;
        }

        /// <summary>
        /// Update the values of this request
        /// </summary>
        public void UpdatePathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Node[], bool> callback)
        {
            PathStart = pathStart;
            PathEnd = pathEnd;
            Callback = callback;
        }
    }
}
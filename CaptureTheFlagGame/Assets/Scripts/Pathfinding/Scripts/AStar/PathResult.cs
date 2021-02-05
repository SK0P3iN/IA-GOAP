using System;
using Assets.Scripts.Pathfinding.Scripts.Grid;

namespace Assets.Scripts.Pathfinding.Scripts.AStar
{
    /// <summary>
    /// Represents a result from the pathfinding
    /// </summary>
    public struct PathResult
    {
        /// <summary>
        /// The path
        /// </summary>
        private readonly Node[] _path;
        /// <summary>
        /// True if the pathfinding was successful
        /// </summary>
        private readonly bool _success;
        /// <summary>
        /// The callback
        /// </summary>
        private readonly Action<Node[], bool> _callback;

        public PathResult(Node[] path, bool success, Action<Node[], bool> callback) : this()
        {
            _path = path;
            _success = success;
            _callback = callback;
        }

        public void Invoke()
        {
            _callback?.Invoke(_path, _success);
        }
    }
}

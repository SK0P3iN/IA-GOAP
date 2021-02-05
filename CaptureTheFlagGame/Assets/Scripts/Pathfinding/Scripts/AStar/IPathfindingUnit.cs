using UnityEngine;

namespace Assets.Scripts.Pathfinding.Scripts.AStar
{
    public interface IPathfindingUnit
    {
        void SetTarget(Transform transform);
        bool DoFollowPathStep();

    }
}
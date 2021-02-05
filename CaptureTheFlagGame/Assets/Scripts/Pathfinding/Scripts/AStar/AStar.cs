using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Pathfinding.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Pathfinding.Scripts.AStar
{
    public class AStar : MonoBehaviour
    {
        /// <summary>
        /// Reference to the Grid Object
        /// </summary>
        public PathfindingGrid Grid;

        /// <summary>
        /// Plot a path for the PathRequest received
        /// </summary>
        public void FindPath(PathRequest request, Action<PathResult> callback)
        {
            if (request == null) return;

            var pathSuccess = false;

            // Get the start and end nodes
            var startNode = Grid.NodeFromWorldPoint(request.PathStart);
            var targetNode = Grid.NodeFromWorldPoint(request.PathEnd);
            startNode.Parent = startNode;

            if (startNode.Walkable && targetNode.Walkable)
            {
                var frontier = new Heap<Node>(Grid.MaxSize); // open set
                var visited = new HashSet<Node>(); // closed set
                frontier.Add(startNode);

                while (frontier.Count > 0) // While there are nodes in the frontier
                {
                    var currentNode = frontier.RemoveFirst(); // get the cheapest node

                    // if the current node is the target node, then we arrived
                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    // add the current node to the visited set
                    visited.Add(currentNode);

                    foreach (var neighbor in Grid.GetNeighbours(currentNode))
                    {
                        // skip if this neighbor is not walkable or if it was already visited
                        if (neighbor.Walkable == false || visited.Contains(neighbor))
                            continue;

                        // calculate the G cost for this neighbor. The G cost is the Real Cost
                        var newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor) +
                                                 neighbor.MovementPenalty;

                        // if the new cost is lower than the previous GCost or if the neighbor is not in the frontier (first time visited), we then update the cost
                        if (newCostToNeighbor < neighbor.GCost || frontier.Contains(neighbor) == false)
                        {
                            neighbor.GCost = newCostToNeighbor;
                            neighbor.HCost = GetDistance(neighbor, targetNode); // distance in straight line from this neighbor to the target node
                            neighbor.Parent = currentNode; // set the current node as the node we used to arrive to this neighbor

                            // if not in the frontier, add it to the frontier
                            if(frontier.Contains(neighbor) == false)
                                frontier.Add(neighbor);
                            else
                                frontier.UpdateItem(neighbor);
                        }
                    }
                }
            }

            var wayPoints = new Node[0];
            if (pathSuccess)
                wayPoints = RetracePath(startNode, targetNode);

            callback(new PathResult(wayPoints, pathSuccess, request.Callback));
        }

        /// <summary>
        /// Retrace the path. Returns all nodes, begining at the start and finishing at the end.
        /// </summary>
        private Node[] RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            var waypoints = path.ToArray();

            //invert the list so it will be from the start
            Array.Reverse(waypoints);

            return waypoints;
        }

        /// <summary>
        /// Removes the redundant nodes in the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Node[] SimplifyPath(List<Node> path)
        {
            var waypoints = new List<Node>();
            var directionOld = Vector2.zero;
            var last = path.Last();

            for (var i = 1; i < path.Count; i++)
            {
                var directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
                if (directionNew == directionOld) continue;

                waypoints.Add(path[i]);
                directionOld = directionNew;
            }

            waypoints.Add(last);

            return waypoints.ToArray();
        }

        /// <summary>
        /// Returns a int with the distance of two notes. Horizontal or Vertical moves costs 10, diagonal moves costs 14 (~sqrt(2)*10)
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        public int GetDistance(Node nodeA, Node nodeB)
        {
            var dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX); // absolute value for the X's difference
            var dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY); // absolute value for the Y's difference

            if (dstX > dstY)
                return 14*dstY + 10 * (dstX-dstY);

            return 14*dstX + 10 * (dstY-dstX);
        }
    }
}

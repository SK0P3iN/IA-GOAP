using System.Collections.Generic;
using Assets.Scripts.Pathfinding;
using UnityEngine;

namespace Assets.GrupoCJJ.Scripts.GameData.Soldiers
{
    [RequireComponent(typeof(PathfindingUnit))]
    public class BlockSoldier : Soldier
    {
        public override HashSet<KeyValuePair<string, object>> CreateGoalState()
        {
            var goal = new HashSet<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("blockBridge", true)
            };

            return goal;
        }
    }
}

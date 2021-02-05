using System.Collections.Generic;
using Assets.Scripts.Pathfinding;
using UnityEngine;

namespace Assets.GrupoCJJ.Scripts.GameData.Soldiers
{
    [RequireComponent(typeof(PathfindingUnit))]
    public class ConquerSoldier : Soldier
    {
        public override HashSet<KeyValuePair<string, object>> CreateGoalState()
        {
            var goal = new HashSet<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Captured", true)
            };

            return goal;
        }
    }
}

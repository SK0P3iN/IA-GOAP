using System.Linq;
using Assets.AI_Team.GoalOrientedBehaviour.Scripts.AI.GOAP;
using Assets.EOTS;
using Assets.General_Scripts;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;
using Soldier = Assets.AI_Team.GoalOrientedBehaviour.Scripts.GameData.Soldiers.Soldier;

namespace Assets.AI_Team.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    public class ConquerClosestBase : GoapAction
    {
        
        private Base _conqueringBase;
        private ISoldier _soldier;


        
        private void Awake()
        {
            _soldier = GetComponent<Soldier>();
            AddEffect("conquerBase", true);
            //AddPrecondition("Cleared", true);
        }


        public override void Reset()
        {
            _conqueringBase = null;
        }

        public override bool IsDone()
        {
            return _conqueringBase.MyTeam == _soldier.MyTeam;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            if (Utils.GetClosest(
                FindObjectsOfType<Base>().
                    Where(b => _soldier.MyTeam == Teams.RedTeam && (b.MyTeam == Teams.BlueTeam || b.MyTeam == Teams.Neutral) || 
                               _soldier.MyTeam == Teams.BlueTeam && (b.MyTeam == Teams.RedTeam || b.MyTeam == Teams.Neutral )), _soldier.MyTransform, out _conqueringBase))
            {
                Target = _conqueringBase.gameObject;
                return true;
            }

            return false;

        }

        public override bool Perform(GameObject agent)
        {
            _soldier.MyTransform.GetComponent<SteeringBasics>().Stop();
            return true; // is performing
        }

        public override bool RequiresInRange()
        {
            return true;
        }
    }
}
using System.Linq;
using Assets.AI_Team.GoalOrientedBehaviour.Scripts.AI.GOAP;
using Assets.EOTS;
using Assets.General_Scripts;
using UnityEngine;

namespace Assets.AI_Team.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    public class ClearedBase : GoapAction
    {

        public  Base ConqueringBase;
        private ISoldier _soldier;
        private DemoIaTeamManager _tm;

        private void Awake()
        {
            _soldier = GetComponent<ISoldier>();
            _tm = FindObjectOfType<DemoIaTeamManager>();
            AddEffect("Cleared", true);

        }


        public override void Reset()
        {
            ConqueringBase = null;
        }

        public override bool IsDone()
        {
            return true; // just need to move to base
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            if (Utils.GetClosest(
                FindObjectsOfType<Base>().
                    Where(b => _soldier.MyTeam == Teams.RedTeam && b.MemberOfTeamBlue.Count == 0 && (b.MyTeam == Teams.RedTeam || b.MyTeam == Teams.Neutral) || 
                               _soldier.MyTeam == Teams.BlueTeam && b.MemberOfTeamRed.Count == 0 && b.MyTeam == Teams.BlueTeam || b.MyTeam == Teams.Neutral), _soldier.MyTransform, out ConqueringBase))
            {
                Target = ConqueringBase.gameObject;
                return true;
            }

            return false;
        }

        public override bool Perform(GameObject agent)
        {
            return true;
        }

        public override bool RequiresInRange()
        {
            return true;
        }
    }
}
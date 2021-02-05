using System.Linq;
using Assets.AI_Team.GoalOrientedBehaviour.Scripts.AI.GOAP;
using Assets.EOTS;
using Assets.General_Scripts;
using UnityEngine;

namespace Assets.AI_Team.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    public class AttackBase : GoapAction
    {

        public Base ConqueringBase;
        private ISoldier _soldier;
        private DemoIaTeamManager _tm;


        private void Awake()
        {
            _soldier = GetComponent<ISoldier>();
            _tm = FindObjectOfType<DemoIaTeamManager>();
            AddEffect("Cleared", true);
            AddPrecondition("attacked", true);

        }

        public override void Reset()
        {
            ConqueringBase = null;
            
        }

        public override bool IsDone()
        {
            if (_soldier.MyTeam == Teams.RedTeam)
                return ConqueringBase.MemberOfTeamBlue.Count == 0;

            return ConqueringBase.MemberOfTeamRed.Count == 0;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            if (Utils.GetClosest(
                FindObjectsOfType<Base>().
                    Where(b => _soldier.MyTeam == Teams.BlueTeam && b.MemberOfTeamBlue.Count != 0 ||
                               _soldier.MyTeam == Teams.RedTeam && b.MemberOfTeamRed.Count != 0), _soldier.MyTransform, out ConqueringBase))
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
using Assets.EOTS;
using Assets.GrupoCJJ.Scripts.AI.GOAP;
using Assets.GrupoCJJ.Scripts.GameData.Soldiers;
using Assets.GrupoCJJ;
using System.Linq;
using UnityEngine;

namespace Assets.GrupoCJJ.Scripts.GameData.Actions
{
    public class ScoreFlag : GoapAction
    {
        /// <summary>
        /// The object used for the effect
        /// </summary>
        private bool _scored;

        /// <summary>
        /// Target of this action
        /// </summary>
        private Transform _myTeamBase;

        private Base _droppingBase;
        private FlagComponent _flag;

        private Soldier _soldier;

        private TeamManager _tm;

        private void Awake()
        {
            _tm = FindObjectOfType<TeamManager>();
            _soldier = GetComponent<Soldier>();
            _flag = FindObjectOfType<FlagComponent>();
            AddPrecondition("hasFlag", true); // we must have the flag to drop it at the base
            AddEffect("scored", true); // we will have dropped the flag once we finish
            AddEffect("hasFlag", false); // we will no longer have the flag after we drop it

        }

        public override void Reset()
        {
            //print("Reset action");
            _scored = false;
            StartTime = 0;
        }

        public override bool IsDone()
        {
            print("GP3 - FLAG SCORED DONE = " + _scored);
            return _scored;
        }

        public override bool RequiresInRange()
        {
            return true; // you must be in range to drop the flag
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            //if (Utils.GetClosest(FindObjectsOfType<Base>(), transform, out _droppingBase))
            if (Utils.GetClosest(FindObjectsOfType<Base>().Where(b => b.MyTeam == _soldier.MyTeam), transform, out _droppingBase))
            {
                Target = _droppingBase.gameObject;
                return true;
            }
            
            return false;
        }

        public override bool Perform(GameObject agent)
        {

            if (_soldier.HasFlag == false)
                return false; // lost the flag somewhere

            if(_droppingBase.MyTeam != _soldier.MyTeam)
            {
                _tm.ResetTeamPlan();
                return false;
            }

            _flag.Score(_droppingBase);
            _soldier.HasFlag = false;
            _scored = true; // you have dropped the flag

            print("GP3 - Scored Flag");
            //_tm.DoWaitFlag(_soldier);

            return true;
        }
    }
}
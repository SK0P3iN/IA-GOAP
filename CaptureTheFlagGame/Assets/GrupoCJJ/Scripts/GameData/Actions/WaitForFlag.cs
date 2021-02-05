using Assets.EOTS;
using Assets.General_Scripts;
using Assets.GrupoCJJ.Scripts.AI.GOAP;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.GrupoCJJ.Scripts.GameData.Actions
{
    public class WaitForFlag : GoapAction
    {
        private FlagComponent _flag;
        private ISoldier _soldier;

        //private TeamManager _tm;
        // Start is called before the first frame update
        private void Awake()
        {
            //_tm = FindObjectOfType<TeamManager>();
            _soldier = GetComponent<ISoldier>();
            //AddPrecondition("hasFlag", false); // we cannot have the flag to pick up the flag
            //AddEffect("hasFlag", true); // we will have the flag after we picked it up

            // cache the flag
            _flag = FindObjectOfType<FlagComponent>();
            //Target = GameObject.Find("NE");
        }

        public override void Reset()
        {
            //print("Reset action");
        }

        public override bool IsDone()
        {
            if (_flag.CanBeCarried)
            {
                //_tm.DoNotWaitFlag(_soldier);
                return true;
            }
            return false;
        }

        public override bool RequiresInRange()
        {
            return true; // yes we need to be near the flag to pick it up  
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {

            Target = GameObject.Find("flag spawn");

            return true;
        }

        public override bool Perform(GameObject agent)
        {
            if (Target == null)
                return false;

            print("GP3 - WAIT FOR FLAG");
            _soldier.MyTransform.GetComponent<SteeringBasics>().Stop();
            return true;
        }
    }
}

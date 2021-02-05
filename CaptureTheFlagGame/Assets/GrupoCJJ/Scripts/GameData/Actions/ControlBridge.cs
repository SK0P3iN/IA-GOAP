using Assets.EOTS;
using Assets.GrupoCJJ.Scripts.AI.GOAP;
using Assets.GrupoCJJ.Scripts.GameData.Soldiers;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.GrupoCJJ.Scripts.GameData.Actions
{
    public class ControlBridge : GoapAction
    {
        //private bool _flagDropped;
        private Soldier _soldier;
        //private FlagComponent _flag;

        Vector3 bridgePosition1, bridgePosition2, bridgePosition3;

        private void Awake()
        {
            _soldier = GetComponent<Soldier>();
            AddPrecondition("blockBridge", true);
            AddEffect("holdBridge", true); 
            AddEffect("blockBridge", false); 

        }

        public override void Reset()
        {
            //_flagDropped = false;
            //StartTime = 0;
        }

        public override bool IsDone()
        {
            //return _flagDropped
            return false;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            return true;
        }

        public override bool Perform(GameObject agent)
        {
            /*if (_soldier.HasFlag == false)
                return false;

            _flag.Drop();
            _soldier.HasFlag = false;
            _flagDropped = false;*/

            //_soldier.MyTransform.GetComponent<SteeringBasics>().Stop();
            print("HOLD PONTE!");

            return true;
        }

        public override bool RequiresInRange()
        {
            return true;
        }
    }
}
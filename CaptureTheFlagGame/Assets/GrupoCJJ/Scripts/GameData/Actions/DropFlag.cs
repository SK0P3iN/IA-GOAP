using Assets.EOTS;
using Assets.GrupoCJJ.Scripts.AI.GOAP;
using Assets.GrupoCJJ.Scripts.GameData.Soldiers;
using UnityEngine;

namespace Assets.GrupoCJJ.Scripts.GameData.Actions
{
    public class DropFlag : GoapAction
    {
        private bool _flagDropped;
        private Soldier _soldier;
        private FlagComponent _flag;

        private void Awake()
        {
            _soldier = GetComponent<Soldier>();
            _flag = FindObjectOfType<FlagComponent>();
            AddPrecondition("hasFlag", true); // we must have the flag to drop it at the base
            AddEffect("hasFlag", false); // we will no longer have the flag after we drop it

        }

        public override void Reset()
        {
            _flagDropped = false;
            StartTime = 0;
        }

        public override bool IsDone()
        {
            return _flagDropped;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            return _soldier.HasFlag;
        }

        public override bool Perform(GameObject agent)
        {
            if (_soldier.HasFlag == false)
                return false;

            _flag.Drop();
            _soldier.HasFlag = false;
            _flagDropped = false;

            print("GP3 - FLAG DROPPED");

            return true;
        }

        public override bool RequiresInRange()
        {
            return false;
        }
    }
}
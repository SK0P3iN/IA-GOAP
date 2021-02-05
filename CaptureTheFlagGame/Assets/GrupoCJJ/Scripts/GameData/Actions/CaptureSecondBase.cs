using System.Linq;
using Assets.GrupoCJJ.Scripts.AI.GOAP;
using Assets.EOTS;
using Assets.GrupoCJJ.Scripts.GameData.Soldiers;
using UnityEngine;
using Assets.Scripts.SteeringBehaviours.Basics;

namespace Assets.GrupoCJJ.Scripts.GameData.Actions
{
    public class CaptureSecondBase : GoapAction
    {

        //public Base ConqueringBase;
        private Soldier _soldier;
        private TeamManager _tm;


        private void Awake()
        {
            _soldier = GetComponent<Soldier>();

            _tm = FindObjectOfType<TeamManager>();
            AddEffect("Captured", true);

            //AddPrecondition("blockBridge", true);

        }

        public override void Reset()
        {
            //ConqueringBase = null;
        }

        public override bool IsDone()
        {
            //_tm.DecideBase2(_soldier);
            //return ConqueringBase.MyTeam == _soldier.MyTeam;
            /*if (FindObjectsOfType<Enemies>()
                .Where(en => Vector3.Distance(en.MyTransform.position, _soldier.MyTransform.position) < 2.0f).FirstOrDefault())*/
            if (FindObjectsOfType<MonoBehaviour>().OfType<Soldier>()
                .Where(s => s.MyTeam != _soldier.MyTeam
                && Vector3.Distance(s.MyTransform.position, _soldier.MyTransform.position) < 2.0f).Count() > 0)
            {
                print("GP3 - ENEMY FOUND AT BASE 2!!");
                return true;
            }
            else
                return false;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            /*if (Utils.GetClosest(FindObjectsOfType<Base>().Where(b => _soldier.MyTeam == Teams.RedTeam && b._myMat.color != Color.red || _soldier.MyTeam == Teams.BlueTeam && b._myMat.color != Color.blue), _soldier.MyTransform, out ConqueringBase))
            {
                Target = ConqueringBase.gameObject;
                return true;
            }*/

            if (_soldier.MyTeam == Teams.RedTeam)
                Target = GameObject.Find("SW");
            else
                Target = GameObject.Find("NW");

            return true;
        }

        public override bool Perform(GameObject agent)
        {
            _soldier.MyTransform.GetComponent<SteeringBasics>().Stop();

            /*print("TEAM MANAGER: Reforce");
            _tm.ReforceBridge();*/

            return true;
        }

        public override bool RequiresInRange()
        {
            return true;
        }
    }
}
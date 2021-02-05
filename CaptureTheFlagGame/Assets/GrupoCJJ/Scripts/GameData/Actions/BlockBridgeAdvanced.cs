using Assets.EOTS;
using Assets.GrupoCJJ.Scripts.AI.GOAP;
using Assets.GrupoCJJ.Scripts.GameData.Soldiers;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.GrupoCJJ.Scripts.GameData.Actions
{
    public class BlockBridgeAdvanced : GoapAction
    {
        //private bool _flagDropped;
        private Soldier _soldier;
        //private FlagComponent _flag;

        Vector3 bridgePosition;

        private void Awake()
        {
            _soldier = GetComponent<Soldier>();

            AddPrecondition("blockBridge", true);
            AddEffect("blockBridgeAdvanced", true); // we will no longer have the flag after we drop it
        }

        public override void Reset()
        {
        }

        public override bool IsDone()
        {
            if (Vector3.Distance(_soldier.MyTransform.position, bridgePosition) < 3.0f)
            {
                print("GP3 - Cheguei a outra ponta!");
                return true;
            }
            else
            {
                print("GP3 - VAI PARA O OUTRO LADO!");
                return false;
            }
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            /*if (_soldier.MyTeam == Teams.RedTeam)
                bridgePosition = new Vector3(-7.0f, 0.0f, Random.Range(11.2f, 13.0f));
            else
                bridgePosition = new Vector3(3.0f, 0.0f, Random.Range(11.2f, 13.0f));
            var go = new GameObject("bridgePointAdvanced");
            go.transform.position = bridgePosition;

            Target = go;
            return true;*/

            if (_soldier.MyTeam == Teams.RedTeam)
            {
                bridgePosition = new Vector3(-7.0f, 0.0f, Random.Range(11.2f, 13.0f));
                if (!GameObject.Find("bridgePointAdvanced"))
                {
                    var go = new GameObject("bridgePointAdvanced");
                    go.transform.position = bridgePosition;
                    Target = go;
                }
                else if (!GameObject.Find("bridgePointAdvanced2"))
                {
                    var go = new GameObject("bridgePointAdvanced2");
                    go.transform.position = bridgePosition;
                    Target = go;
                }
                else
                {
                    Target = GameObject.Find("bridgePointAdvanced2");
                }
            }
            else
            {
                bridgePosition = new Vector3(3.0f, 0.0f, Random.Range(11.2f, 13.0f));
                if (!GameObject.Find("bridgePointAdvanced3"))
                {
                    var go = new GameObject("bridgePointAdvanced3");
                    go.transform.position = bridgePosition;
                    Target = go;
                }
                else if (!GameObject.Find("bridgePointAdvanced4"))
                {
                    var go = new GameObject("bridgePointAdvanced4");
                    go.transform.position = bridgePosition;
                    Target = go;
                }
                else
                {
                    Target = GameObject.Find("bridgePointAdvanced4");
                }
            }
            return true;
        }

        public override bool Perform(GameObject agent)
        {
            _soldier.MyTransform.GetComponent<SteeringBasics>().Stop();

            return true;
        }

        public override bool RequiresInRange()
        {
            return true;
        }
    }
}
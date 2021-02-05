using Assets.EOTS;
using Assets.GrupoCJJ.Scripts.AI.GOAP;
using Assets.GrupoCJJ.Scripts.GameData.Soldiers;
using Assets.Scripts.SteeringBehaviours.Basics;
using System.Linq;
using UnityEngine;

namespace Assets.GrupoCJJ.Scripts.GameData.Actions
{
    public class BlockBridge : GoapAction
    {
        private Soldier _soldier;

        Vector3 bridgePosition;

        private TeamManager _tm;

        private void Awake()
        {
            _soldier = GetComponent<Soldier>();
            _tm = FindObjectOfType<TeamManager>();
            AddEffect("blockBridge", true);
        }

        public override void Reset()
        {
        }

        public override bool IsDone()
        {
            /*var inimigos = FindObjectsOfType<EnemySoldier>().Where(s => s.MyTeam != _soldier.MyTeam);
            foreach (var inimigo in inimigos)
            {
                if (Vector3.Distance(_soldier.MyTransform.position, inimigo.MyTransform.position) < 20.0f)
                {
                    print("GP3 - Inimigo detetado!");
                    return true;
                }
                else
                {
                    print("GP3 - Ponte bloqueada!");
                    _tm.DecidePonte(_soldier);
                    return false;
                }
            }*/

            if (FindObjectsOfType<MonoBehaviour>().OfType<Soldier>()
                .Where(s => s.MyTeam != _soldier.MyTeam
                && Vector3.Distance(s.MyTransform.position, _soldier.MyTransform.position) < 20.0f).Count() > 0)
            {
                print("GP3 - Inimigo detetado!");
                return true;
            }
            else
            {
                print("GP3 - Ponte bloqueada!");
                _tm.DecidePonte(_soldier);
                return false;
            }
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            //criar game objects
            if(_soldier.MyTeam == Teams.RedTeam)
            {
                bridgePosition = new Vector3(3.0f, 0.0f, Random.Range(11.2f, 13.0f));
                if (!GameObject.Find("bridgePoint"))
                {
                    var go = new GameObject("bridgePoint");
                    go.transform.position = bridgePosition;
                    Target = go;
                }
                else if (!GameObject.Find("bridgePoint2"))
                {
                    var go = new GameObject("bridgePoint2");
                    go.transform.position = bridgePosition;
                    Target = go;
                }
                else
                {
                    Target = GameObject.Find("bridgePoint2");
                }
            }
            else
            {
                bridgePosition = new Vector3(-7.0f, 0.0f, Random.Range(11.2f, 13.0f));
                if(!GameObject.Find("bridgePoint3"))
                {
                    var go = new GameObject("bridgePoint3");
                    go.transform.position = bridgePosition;
                    Target = go;
                }
                else if (!GameObject.Find("bridgePoint4"))
                {
                    var go = new GameObject("bridgePoint4");
                    go.transform.position = bridgePosition;
                    Target = go;
                }
                else
                {
                    Target = GameObject.Find("bridgePoint4");
                }
            }
            
            

            /*var go = new GameObject("bridgePoint");
            go.transform.position = bridgePosition;
            
            Target = go;*/
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
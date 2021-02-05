using System.Collections;
using System.Linq;
using Assets.AI_Team.GoalOrientedBehaviour.Scripts.AI.GOAP;
using Assets.EOTS;
using Assets.General_Scripts;
using UnityEngine;

namespace Assets.AI_Team.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    public class AttackPlayer : GoapAction
    {
        private bool _attacked;
        private bool _onCooldown;
        private ISoldier _target;
        private ISoldier _me;

        public GameObject AttTarget;

        private DemoIaTeamManager _tm;

        private void Awake()
        {
            _me = GetComponent<ISoldier>();
            _tm = FindObjectOfType<DemoIaTeamManager>();

            AddEffect("attacked", true);
        }


        public override void Reset()
        {
            _attacked = false;
        }

        public override bool IsDone()
        {
            return _attacked;
        }

        public override bool RequiresInRange()
        {
            return true;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            // can be changed to several strategies. in this case, we will attack the closest agent
            //_target = 
            //        _enemies
            //        .OrderBy(go => Vector3.Distance(go.MyTransform.position, _me.MyTransform.position))
            //        .FirstOrDefault();

            //var list = _me.MyTeam == Teams.BlueTeam
            //    ? _ab.ConqueringBase.MemberOfTeamRed
            //    : _ab.ConqueringBase.MemberOfTeamBlue;

            ////GameObject go;
            //if (Utils.GetClosest(list.Select(s => s.MyTransform.GetComponent<MonoBehaviour>()), agent.transform, out var mono))
            //{
            //    _target = mono.gameObject.GetComponent<ISoldier>();
            //    Target = mono.gameObject;
            //    return true;
            //}

            //return false;



            if (Utils.GetClosest(
                FindObjectsOfType<Base>().
                    Where(b => _me.MyTeam == Teams.RedTeam && b.MemberOfTeamBlue.Count != 0 && b._myMat.color != Color.red ||
                               _me.MyTeam == Teams.BlueTeam && b.MemberOfTeamRed.Count != 0 && b._myMat.color != Color.blue), _me.MyTransform, out var @base))
            {

                var list = _me.MyTeam == Teams.BlueTeam
                    ? @base.MemberOfTeamRed
                    : @base.MemberOfTeamBlue;

                //GameObject go;
                if (Utils.GetClosest(list.Select(s => s.MyTransform.GetComponent<MonoBehaviour>()), agent.transform, out var mono))
                {
                    _target = mono.gameObject.GetComponent<ISoldier>();
                    Target = mono.gameObject;
                    AttTarget = Target;
                    return true;
                }

                return true;
            }

            return false;


        }
         
        public override bool Perform(GameObject agent)
        {

            if (Target == null || _target.Invulnerable || _onCooldown || Vector3.Distance(agent.transform.position, _target.MyTransform.position) > 1.5f)
                return false;

            
            _target.Died();
            _target = null;
            StartCoroutine(StartCooldown());

            _attacked = true;

            print("ATTACKED");
            _tm.ResetTeamPlan();


            return true;
        }


        private IEnumerator StartCooldown()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(20f);
            _onCooldown = false;
        }

    }
}
using System.Collections;
using System.Linq;
using Assets.EOTS;
using Assets.GrupoCJJ.Scripts.AI.GOAP;
using Assets.GrupoCJJ.Scripts.GameData.Soldiers;
using UnityEngine;

namespace Assets.GrupoCJJ.Scripts.GameData.Actions
{
    public class AttackPlayer : GoapAction
    {
        private bool _attacked;
        private bool _onCooldown;
        private Soldier _target;
        private Soldier _me;

        private FlagComponent _flag;

        private TeamManager _tm;

        private void Awake()
        {
            _me = GetComponent<Soldier>();
            _tm = FindObjectOfType<TeamManager>();
            _flag = FindObjectOfType<FlagComponent>();

            AddEffect("Captured", false);
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

            /*if(Utils.GetClosest(FindObjectsOfType<Enemies>().
                Where(en => Vector3.Distance(en.MyTransform.position, _me.MyTransform.position) < 1.6f), transform, out _target))*/
            if (Utils.GetClosest(FindObjectsOfType<MonoBehaviour>().OfType<Soldier>()
                .Where(s => s.MyTeam != _me.MyTeam
                && Vector3.Distance(s.MyTransform.position, _me.MyTransform.position) < 1.5f), transform, out _target ))
            {
                print("GP3 - ATACAR");
                Target = _target.gameObject;

                return true;
            }
            //se nao conseguiu atacar decide o que fazer
            if (_me.GetComponent<CaptureSecondBase>())
                _tm.DecideBase2(_me);
            else if (_me.MyTransform.GetComponent<CaptureFourthBase>())
                _tm.DecidePonte(_me);

            return false;

            /*print("GP3 - ATACAR: " + (_onCooldown == false && Utils.GetClosest(FindObjectsOfType<Enemies>().ToList(), transform, out _target)));

            return _onCooldown == false && Utils.GetClosest(FindObjectsOfType<Enemies>().ToList(), transform, out _target);*/
        }
         
        public override bool Perform(GameObject agent)
        {
            print("GP3 - PERFORM");
            print("GP3 - T: " + Target + "I: " + _target.Invulnerable + "t: " + _target + "d: " + Vector3.Distance(agent.transform.position, _target.transform.position));
            if (Target == null || _target.Invulnerable || _onCooldown || Vector3.Distance(agent.transform.position, _target.transform.position) > 1.5f)
            {
                return false;
            }
                

            print("GP3 - ATTACKED!!");
            _target.Died();
            _target = null;
            StartCoroutine(StartCooldown());

            _attacked = true;
            if (_me.GetComponent<CaptureSecondBase>()) //se consegui atacar, verifica se ja reconquistamos ou nao a base
                _tm.DecideBase2(_me);
            else if (_me.MyTransform.GetComponent<CaptureFourthBase>())
                _tm.DecidePonte(_me);

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
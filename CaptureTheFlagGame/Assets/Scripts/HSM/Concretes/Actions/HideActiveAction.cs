using System.Linq;
using Assets.Scripts.HSM.Abstracts;
using Assets.Scripts.HSM.Concretes.Conditions;
using Assets.Scripts.SteeringBehaviours.Advanced;
using UnityEngine;

namespace Assets.Scripts.HSM.Concretes.Actions
{
    public class HideActiveAction : IAction
    {
        private readonly GameObject _agent;
        private readonly Transform _agentsTransform;
        private Hide _agentsHide;


        public HideActiveAction(GameObject agent)
        {
            _agent = agent;
            _agentsTransform = agent.transform;
            _agentsHide = agent.GetComponent<Hide>();
        }

        public void Execute()
        {
            // just ignore this... as you add and remove components, you'll loose the reference, that's why this is here
            if (_agentsHide == null)
                _agentsHide = _agent.GetComponent<Hide>();
            if (_agentsHide == null)
                return;


            // get all object in a sphere from certain distance
            var results = Physics.SphereCastAll(_agentsTransform.position, Constants.CHASER_THRESHOLD, Vector3.forward, 2).Where(hit => hit.transform.CompareTag("Chaser")).ToList();

            if (results.Count == 0) 
            {
                _agentsHide.Target = null; 
                return;
            }

            var closest = results[0];

            foreach (var hit in results)
            {
                if (Vector3.Distance(hit.transform.position, _agentsTransform.position) <
                    Vector3.Distance(closest.transform.position, _agentsTransform.position))
                    closest = hit;
            }

            _agentsHide.Target = closest.rigidbody;
        }
    }
}
using System.Linq;
using Assets.Scripts.HSM.Abstracts;
using Assets.Scripts.HSM.Concretes.Conditions;
using Assets.Scripts.SteeringBehaviours.Advanced;
using UnityEngine;

namespace Assets.Scripts.HSM.Concretes.Actions
{
    public class EvadeActiveAction : IAction
    {
        private readonly GameObject _agent;
        private Evade _agentsEvade;
        private Transform _agentsTransform;

        public EvadeActiveAction(GameObject agent)
        {
            _agent = agent;
            _agentsEvade = agent.GetComponent<Evade>();
            _agentsTransform = agent.transform;

        }

        public void Execute()
        {
            if (_agentsEvade == null)
                _agentsEvade = _agent.GetComponent<Evade>();


            // if there is target withing threshold, return closest

            // get all object in a sphere from certain distance
            var results = Physics.SphereCastAll(_agentsTransform.position, Constants.CHASER_THRESHOLD, Vector3.forward, 2).Where(hit => hit.transform.CompareTag("Chaser")).ToList();

            if (results.Count == 0) return; // do nothing cause trigger will occur

            var closest = results[0];

            foreach (var hit in results)
            {
                if (Vector3.Distance(hit.transform.position, _agentsTransform.position) <
                    Vector3.Distance(closest.transform.position, _agentsTransform.position))
                    closest = hit;
            }

            _agentsEvade.Target = closest.rigidbody;
        }
    }
}
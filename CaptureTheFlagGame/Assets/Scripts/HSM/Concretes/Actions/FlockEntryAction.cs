using Assets.Scripts.HSM.Abstracts;
using Assets.Scripts.SteeringBehaviours.Advanced;
using UnityEngine;

namespace Assets.Scripts.HSM.Concretes.Actions
{
    public class FlockEntryAction : IAction
    {
        private readonly GameObject _agent;

        public FlockEntryAction(GameObject agent)
        {
            _agent = agent;
        }


        public void Execute()
        {
            _agent.AddComponent<Flocking>();
        }
    }
}
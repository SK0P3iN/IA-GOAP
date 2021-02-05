using Assets.Scripts.HSM.Abstracts;
using Assets.Scripts.SteeringBehaviours.Advanced;
using UnityEngine;

namespace Assets.Scripts.HSM.Concretes.Actions
{
    public class EvadeEntryAction : IAction
    {
        private readonly GameObject _agent;

        public EvadeEntryAction(GameObject agent)
        {
            _agent = agent;
        }

        public void Execute()
        {
            _agent.AddComponent<Evade>();
        }
    }
}
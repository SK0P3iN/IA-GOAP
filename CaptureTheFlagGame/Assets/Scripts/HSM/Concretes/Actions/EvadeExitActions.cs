using Assets.Scripts.HSM.Abstracts;
using Assets.Scripts.SteeringBehaviours.Advanced;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.Scripts.HSM.Concretes.Actions
{
    public class EvadeExitActions : IAction
    {
        private readonly GameObject _agent;

        public EvadeExitActions(GameObject agent)
        {
            _agent = agent;
        }

        public void Execute()
        {
            Object.DestroyImmediate(_agent.GetComponent<Evade>());
            Object.DestroyImmediate(_agent.GetComponent<Flee>());
            Object.DestroyImmediate(_agent.GetComponent<SteeringBasics>());
        }
    }
}
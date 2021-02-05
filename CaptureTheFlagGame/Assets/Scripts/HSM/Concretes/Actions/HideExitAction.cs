using Assets.Scripts.HSM.Abstracts;
using Assets.Scripts.SteeringBehaviours.Advanced;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.Scripts.HSM.Concretes.Actions
{
    public class HideExitAction : IAction
    {
        private readonly GameObject _agent;

        public HideExitAction(GameObject agent)
        {
            _agent = agent;
        }

        public void Execute()
        {
            Object.DestroyImmediate(_agent.GetComponent<Hide>());
            Object.DestroyImmediate(_agent.GetComponent<Evade>());
            Object.DestroyImmediate(_agent.GetComponent<Arrive>());
            Object.DestroyImmediate(_agent.GetComponent<WallAvoidance>());
            Object.DestroyImmediate(_agent.GetComponent<Seek>());
            Object.DestroyImmediate(_agent.GetComponent<Flee>());
            Object.DestroyImmediate(_agent.GetComponent<SteeringBasics>());
        }
    }
}
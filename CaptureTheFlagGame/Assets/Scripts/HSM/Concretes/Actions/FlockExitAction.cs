using Assets.Scripts.HSM.Abstracts;
using Assets.Scripts.SteeringBehaviours.Advanced;
using Assets.Scripts.SteeringBehaviours.Basics;
using Assets.Scripts.SteeringBehaviours.Utils;
using UnityEngine;

namespace Assets.Scripts.HSM.Concretes.Actions
{
    public class FlockExitAction : IAction
    {
        private readonly GameObject _agent;

        public FlockExitAction(GameObject agent)
        {
            _agent = agent;
        }

        public void Execute()
        {
            Object.DestroyImmediate(_agent.GetComponent<Flocking>());
            Object.DestroyImmediate(_agent.GetComponent<NearSensor>());
            Object.DestroyImmediate(_agent.GetComponent<Separation>());
            Object.DestroyImmediate(_agent.GetComponent<Cohesion>());
            Object.DestroyImmediate(_agent.GetComponent<Arrive>());
            Object.DestroyImmediate(_agent.GetComponent<Wanderer>());
            Object.DestroyImmediate(_agent.GetComponent<Seek>());
            Object.DestroyImmediate(_agent.GetComponent<SteeringBasics>());

        }
    }
}
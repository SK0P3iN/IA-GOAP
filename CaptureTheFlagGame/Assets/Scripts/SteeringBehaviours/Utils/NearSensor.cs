using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Utils
{
    public class NearSensor : MonoBehaviour
    {
        /// <summary>
        /// List with references for current targets close by
        /// </summary>
        public List<Rigidbody> Targets = new List<Rigidbody>();

        /// <summary>
        /// List with all the tags that are going to be searched
        /// </summary>
        public List<string> SensorTags = new List<string>();

        private void Start()
        {
            SensorTags.Add("Props");
            SensorTags.Add("Fleer");
        }


        private void OnTriggerEnter(Collider other)
        {
            if (SensorTags.Contains(other.tag) == false || 
                other.TryGetComponent<Rigidbody>(out var rb) == false ||
                Targets.Contains(rb))
                return;

            Targets.Add(rb);
        }

        private void OnTriggerExit(Collider other)
        {
            if (SensorTags.Contains(other.tag) == false || 
                other.TryGetComponent<Rigidbody>(out var rb) == false || 
                Targets.Contains(rb))
                return;

            Targets.Remove(rb);
        }
    }
}
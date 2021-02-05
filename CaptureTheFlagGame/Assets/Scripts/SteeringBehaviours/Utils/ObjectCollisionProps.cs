using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours.Utils
{
    public class ObjectCollisionProps : MonoBehaviour
    {
        /// <summary>
        /// Represents the Body Radius in Unity units, which is the same scale used for the Vector 3.
        /// </summary>
        [Tooltip("Represents the Body Radius in Unity units, which is the same scale used for the Vector 3.")]
        public float BodyRadius = .5f;
    }
}
using UnityEngine;

namespace Assets.Scripts
{
    public class AnimationHandler : MonoBehaviour
    {
        /// <summary>
        /// The Character Animator
        /// </summary>
        [Tooltip("The Character Animator")]
        public Animator Animator;

        /// <summary>
        /// Private reference to the RigidBody.
        /// </summary>
        private Rigidbody _modelRb;
        
        /// <summary>
        /// Cached variable id for performance. stores the id of the Move bool in the animator
        /// </summary>
        private static readonly int Move = Animator.StringToHash(("Move"));
        /// <summary>
        /// Cached variable id for performance. stores the id of the GoIdle bool in the animator
        /// </summary>
        private static readonly int GoIdle = Animator.StringToHash("GoIdle");

        private void Awake()
        {
            _modelRb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Animator.SetBool(Move, _modelRb.velocity.magnitude > .1f);
            Animator.SetBool(GoIdle, _modelRb.velocity.magnitude < .1f);
        }
    }
}

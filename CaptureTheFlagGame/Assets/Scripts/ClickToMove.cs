using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class ClickToMove : MonoBehaviour
    {
        public NavMeshAgent Agent;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            if (Input.GetMouseButton(0) == false) return;

            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit)) Agent.SetDestination(hit.point);
        }
    }
}

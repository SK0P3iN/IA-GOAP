using System.Collections;
using Assets.General_Scripts;
using UnityEngine;

namespace Assets.EOTS
{
    public class FlagComponent : MonoBehaviour
    {

        public ISoldier Carrier;

        public bool BeingCarried;

        public bool CanBeCarried = true;

        private MeshRenderer _meshRenderer;
        public GameManager Gm;
        private Transform _myTransform;


        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _myTransform = transform;
        }

        public void PickUp(ISoldier runner)
        {
            Carrier = runner;
            _myTransform.SetParent(runner.MyTransform);
            _myTransform.localPosition  = Vector3.up * 2;
            
            BeingCarried = true;

            //print("Assigned runner: " + Carrier.name);
            //transform.SetParent(Carrier.transform);
        }

        public void Score(Base scoringTeam)
        {
            Gm.ScoreFlag(scoringTeam.MyTeam);
            _meshRenderer.enabled = false;
            CanBeCarried = false;
            StopCoroutine(Reset());
            StartCoroutine(Reset(true));
        }

        public void Drop()
        {
            transform.SetParent(null);
            BeingCarried = false;
            CanBeCarried = true;
            StopCoroutine(Reset());
            StartCoroutine(Reset());
        }


       private IEnumerator Reset(bool reset = false)
        {
            _myTransform.parent = null;

            if (Carrier == null)
                yield break;

            BeingCarried = false;
            //print("removed Runner: " + Carrier.name);
            Carrier = null;


            yield return new WaitForSeconds(10f);
            CanBeCarried = true;
            _meshRenderer.enabled = true;

            if (reset)
            {
                _myTransform.position = Vector3.up;
                yield break;
            }


            yield return new WaitForSeconds(20f);

            //if after 10 seconds no1 got it, return to middle

            _myTransform.position = Vector3.up;
        }

    }
}
using System.Linq;
using Assets.Scripts.HSM.Abstracts;
using UnityEngine;

namespace Assets.Scripts.HSM.Concretes.Conditions
{
    public class NoHidingSpot : ICondition
    {
        public bool Test(object watch)
        {
            var go = (GameObject)watch;

            var result = Physics.SphereCastAll(go.transform.position, Constants.HIDE_THRESHOLD, go.transform.forward,
                2);


            return result.Any(hit => hit.transform.CompareTag("HidingSpot")) == false;
        }
    }
}
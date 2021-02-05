using System.Linq;
using Assets.Scripts.HSM.Abstracts;
using UnityEngine;

namespace Assets.Scripts.HSM.Concretes.Conditions
{
    public class ChaserNearby : ICondition
    {
        
        public bool Test(object watch)
        {
            var go = (GameObject) watch;

            var result = Physics.SphereCastAll(
                                                    go.transform.position,
                                                    Constants.CHASER_THRESHOLD, 
                                                    go.transform.forward, 
                                                    2);
            
            return result.Any(hit => hit.transform.CompareTag("Chaser"));

        }
    }
}
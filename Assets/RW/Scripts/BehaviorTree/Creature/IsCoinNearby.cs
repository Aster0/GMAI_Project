using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Returns success if the creature is tamed.")]
    public class IsCoinNearBy : Conditional
    {
        



        public SharedGameObject coinObject;
        private CreatureInfo creatureInfo;


        public override void OnStart()
        {
            base.OnStart();

            creatureInfo = GetComponent<CreatureInfo>();
        }

        public override TaskStatus OnUpdate()
        {
            bool coinNearby = false;
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5); // collection radius of 5


            foreach (Collider collider in colliders)
            {
           


                if (collider.CompareTag("Coin"))
                {


                    coinNearby = true; // coin is nearby.
                    coinObject.Value = collider.gameObject;
                    break; // break out of iteration as we do not need to iterate anymore.
                }
            }

            
            return coinNearby ? TaskStatus.Success : TaskStatus.Failure;
            // if coinNearby == true, we'll return a success. else a failure.

        }
    }
}
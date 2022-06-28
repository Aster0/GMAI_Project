using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Returns success if an enemy is close to the owner")]
    public class IsEnemyNearBy : Conditional
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
            bool enemyNearby = false;
            
            Collider[] colliders = Physics.OverlapSphere(creatureInfo.owner.transform.position, 5); // detect enemy in radius of 5
            // around the owner, not the creature.


            foreach (Collider collider in colliders) // iterate the raycasted colliders
            {
           


                if (collider.CompareTag("Enemy"))
                {

                    enemyNearby = true;

                }
            }

            
            return enemyNearby ? TaskStatus.Success : TaskStatus.Failure;
            // if coinNearby == true, we'll return a success. else a failure.

        }
    }
}
using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class WanderState : State
    {

        private float time;
        

        private Vector3 previousWanderDestination;
        public WanderState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            //enemy.aStarPathFinding.SetDestination(new Vector3(2.67f, 0, 1.91f));
            enemy.StartCoroutine(enemy.aStarPathFinding.StartPathFind(enemy.characterObject));
        }
        



         public override void LogicUpdate()
        {
            base.LogicUpdate();

            /*if (time < 0)
            {
                

                float randomX = Random.Range(-10, 10);
                float randomZ = Random.Range(-10, 10);
                Vector3 randomDirection = Random.insideUnitSphere * 15;

                Vector3 newWanderDestination = new Vector3(randomDirection.x, 0, randomDirection.z);

                if (newWanderDestination != previousWanderDestination)
                {
                    previousWanderDestination = newWanderDestination;
                
                
                    enemy.aStarPathFinding.SetDestination(enemy.transform.position + newWanderDestination);
                
                    time = 1;
                }
       
            }

            time -= Time.deltaTime;*/
            
            // random wander using A*


           
            
            
            
          
            
        }
         

    }
    
    
}
using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class SeekPlayerState : AlertState
    {

        private float time;
        

        private Vector3 previousWanderDestination;
        public SeekPlayerState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Right);
            
            //enemy.aStarPathFinding.SetDestination(new Vector3(2.67f, 0, 1.91f));
      
            enemy.StartCoroutine(enemy.aStarPathFinding.StartPathFind(enemy.characterObject));
            
            time = 1;
        }
        



         public override void LogicUpdate()
        {
            base.LogicUpdate();



            if (time < 0)
            {
                Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, 2);

                foreach (Collider collider in colliders)
                {
                    if (collider.name.Equals("Character"))
                    {
                        enemy.animator.SetFloat("Forward", 0); // stop walk animation

                        int chance = Random.Range(0, 100);

           
                        if (chance <= 80) // 80% chance
                        {
                            // melee attack
                            stateMachine.ChangeState(enemy.meleeAttackState);
                        }
                        else // last 20% chance
                        {
                            // grapple
                            stateMachine.ChangeState(enemy.grappleAttackState);
                        }



                        time = 1;
              
                        break; // break out of iteration. since we found plyaer.
                    
                    }
                }
            }

            time -= Time.deltaTime;


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
using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class SeekPlayerState : AlertState
    {

        private float time;
        private Coroutine seekCoroutine;
        

        private Vector3 previousWanderDestination;
        public SeekPlayerState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Right);
            
            //enemy.aStarPathFinding.SetDestination(new Vector3(2.67f, 0, 1.91f));
      
            seekCoroutine = enemy.StartCoroutine(enemy.aStarPathFinding.StartPathFind(enemy.characterObject));
            
            time = 1;
        }


        public override void Exit()
        {
            base.Exit();
            
            enemy.StopCoroutine(seekCoroutine); // stop because we're exiting out of seek.
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
       


            if (time < 0)
            {
                Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, 3);

                foreach (Collider collider in colliders)
                {
                    if (collider.name.Equals("Character"))
                    {
                        Character character = collider.GetComponent<Character>();

                        if (character.movementSM.CurrentState != character.getThrown) // if its not currently in the get thrown state
                        // because we don't want to hit while the player is down, we want a short cool down.
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
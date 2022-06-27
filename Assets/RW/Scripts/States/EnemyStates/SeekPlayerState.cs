using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class SeekPlayerState : AlertState
    {

        private float time;
  
        private bool justSmashed; // to see if the player was just thrown by the enemy.
        

        private Vector3 previousWanderDestination;
        public SeekPlayerState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Right);
            

      
          
           
            
            time = 1;
        }




        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Vector3.Distance(enemy.characterObject.transform.position, enemy.transform.position) > 2) // if its more than 2 radius away, we go to the player.
            {
                enemy.aStarPathFinding.aStarManager.BuildPath(enemy.characterObject.transform.position); // build a path to player using A*
            }
        
                
            enemy.stamina -= Time.deltaTime; // lose stamina while seeking

            if (time < 0)
            {
                Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, 2);

                foreach (Collider collider in colliders)
                {
                    if (collider.name.Equals("Character"))
                    {
                        Character character = collider.GetComponent<Character>();

                        if (character.movementSM.CurrentState != character.falldown) // if its not currently in the fall down state
                        // because we don't want to hit while the player is down, we want a short cool down.
                        {
                            enemy.animator.SetFloat("Forward", 0); // stop walk animation

                            int chance = Random.Range(0, 100);

           
                            if (chance <= 80) // 80% chance
                            {
                                // melee attack
                                stateMachine.ChangeState(enemy.meleeAttackState);
                                justSmashed = false; // we can safely say that just thrown is false
                                // because we're entering a melee attack instead of the grappling attack.
                                // so no throwing here.
                                // this makes it so it doesn't grapple twice,
                                // might be unfair to the player.
                            }
                            else // last 20% chance
                            {
                                if(!justSmashed) // if not just thrown
                                // grapple
                                    stateMachine.ChangeState(enemy.slamGroundState);
                            }



                            time = 1;
              
                            break; // break out of iteration. since we found plyaer.
                        }
                        else
                        {
                            justSmashed = true; // as the player now is being
                            // thrown, we can say just thrown is true.
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
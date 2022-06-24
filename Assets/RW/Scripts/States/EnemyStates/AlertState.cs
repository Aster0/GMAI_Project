using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class AlertState : State
    {
       

        private float time;
        
        public AlertState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }
        
        
        public override void Enter()
        {
          
            // we removed the base.Enter();
            // this is essential because i'm overriding the enter to not run the base
            // so the UI wont update like the character as it's the enemy and
            // we don't want our character UI state to update to show the enemy's
            
            
            //enemy.animator.SetTrigger("Point"); // point because it found a player to seek.
        }
        
        public override void LogicUpdate()
        {
     

            // we check if player is nearby within a set radius.
            // if it is, we interrupt the whole idlestand (the enemy might be sitting or standing)
            // but it doesn't matter because it'll go to the alert hierarchy state



            bool isPlayerDead = false;
            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, enemy.seekRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.name.Equals("Character"))
                {
                    Character character = collider.GetComponent<Character>();
                    if (character.movementSM.CurrentState == character.die) // if player isn't dead
                    {
                  
                        isPlayerDead = true; // if we found the player dead,
                        break; // we break straight away so we proceed down.
                    }

            

                }
            }


            if (isPlayerDead || enemy.Stamina <= 20)// if player is dead OR stamina below 20, we go back to stand state.
            {
                
                
                stateMachine.ChangeState(enemy.standState); // switch back to idle state.
            }
            
            
        }
        
    }
}
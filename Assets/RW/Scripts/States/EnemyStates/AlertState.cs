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



 


            if (enemy.Stamina < 20) // if stamina is low
            {
           
                
                stateMachine.ChangeState(enemy.standState); // switch back to idle state.
            }
            
            
        }
        
    }
}
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    // super class IdleState manages if it senses a player player. and will interrupt the sub states if it does
    public class IdleState: State 
    {

        private float time;
        
        public IdleState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
          
            // we removed the base.Enter();
            // this is essential because i'm overriding the enter to not run the base
            // so the UI wont update like the character as it's the enemy and
            // we don't want our character UI state to update to show the enemy's
        }

        public override void LogicUpdate()
        {
     

            // we check if player is nearby within a set radius.
            // if it is, we interrupt the whole idlestand (the enemy might be sitting or standing)
            // but it doesn't matter because it'll go to the alert hierarchy state



            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, 7);

            foreach (Collider collider in colliders)
            {
                if (collider.name.Equals("Character"))
                {
                    Character character = collider.GetComponent<Character>();
                    if (character.movementSM.CurrentState != character.die) // if player isn't dead
                    {
                        stateMachine.ChangeState(enemy.seekPlayerState); // , we seek
                        break;
                    }
             
                    
                }
            }
            
            
        }
    }
}
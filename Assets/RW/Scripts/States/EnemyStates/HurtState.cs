using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    // hurt state only inherits state because we don't want it to do anything like 
    // hitting the player while it's hurt.
    public class HurtState: State 
    {
        private int hurtParam = Animator.StringToHash("Hurt");
        private float time;
        
        public HurtState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
          
            // we removed the base.Enter();
            // this is essential because i'm overriding the enter to not run the base
            // so the UI wont update like the character as it's the enemy and
            // we don't want our character UI state to update to show the enemy's
            
            
            DisplayOnUI(UIManager.Alignment.Right);
            
            // this state is changed by the attacking player. 
            // this is why this state can trigger from any states.
            enemy.animator.SetTrigger(hurtParam);

            enemy.health--; // minus one health as its hurt.

            time = 1.5f; // "stunned" (hurted) for 1.5 seconds so the player
            // can retaliate back better

        }

        public override void LogicUpdate()
        {
     

            // we check if player is nearby within a set radius.
            // if it is, we interrupt the whole idlestand (the enemy might be sitting or standing)
            // but it doesn't matter because it'll go to the alert hierarchy state


            if (time < 0) // when cool down is over
            {
                // then we swap back to previous state.
                
                stateMachine.ChangeState(enemy.seekPlayerState);
            }

            time -= Time.deltaTime;
                
            
            
        }
    }
}
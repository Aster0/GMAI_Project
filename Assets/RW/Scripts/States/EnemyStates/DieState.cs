using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    // hurt state only inherits state because we don't want it to do anything like 
    // hitting the player while it's hurt.
    public class DieState: State 
    {
        private int dieParam = Animator.StringToHash("Die");
        private float time;
        
        public DieState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
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
            enemy.animator.SetTrigger(dieParam);

            enemy.GetComponent<CapsuleCollider>().height = 0;// so it'll lay flat on the ground.

    



        }

  
    }
}
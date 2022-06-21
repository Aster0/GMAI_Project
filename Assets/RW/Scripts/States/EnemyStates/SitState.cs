using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class SitState : IdleState // super class IdleState manages if it senses a player player. and will interrupt this state if it does
    {

        private float time;
        
        public SitState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            
            DisplayOnUI(UIManager.Alignment.Right);
            // sit down
            enemy.animator.SetBool("Sit", true);
           
        }

        public override void Exit()
        {
            base.Exit();
            
            enemy.animator.SetBool("Sit", false); // get up 
            Debug.Log("Get up");
        }

        
    }
}
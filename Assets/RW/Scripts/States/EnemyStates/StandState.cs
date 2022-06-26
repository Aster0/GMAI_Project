using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class StandState: IdleState  // super class IdleState manages if it senses a player player. and will interrupt this state if it does
    {

        private float time;
        
        public StandState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            DisplayOnUI(UIManager.Alignment.Right);
    
        }


        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            // idle state animation using animation blend.
            enemy.animator.SetFloat("Forward", 0);
            enemy.animator.SetFloat("Side", 0);
            // make sure that nothing will override and stay idle.

            if (time < 0)
            {
                time = 3; 
                
                // every 3 seconds, there's a chance for the enemy to be tired
                // and enter the sit idle state.


                if (Random.Range(0, 100) < 30) // 30% chance around there
                {
                    stateMachine.ChangeState(enemy.sitState);
                }

           
            }

            time -= Time.deltaTime;
        }
        
    }
}
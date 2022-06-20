using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class MeleeAttackState : AlertState
    {

        private float time;
        

        private Vector3 previousWanderDestination;
        public MeleeAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
         
            
            enemy.animator.SetTrigger("Melee");
            
            stateMachine.ChangeState(enemy.seekPlayerState);
 
        }
        



         public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            
            


           
            
            
            
          
            
        }
         

    }
    
    
}
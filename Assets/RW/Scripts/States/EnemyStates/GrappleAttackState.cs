using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class GrappleAttackState : AlertState
    {

        private float time;
        private GameObject player;
        

        private Vector3 previousWanderDestination;
        public GrappleAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
         
            DisplayOnUI(UIManager.Alignment.Right);
            
            enemy.animator.SetTrigger("GrappleAttack");

            player = enemy.characterObject;

            time = 4;


        }
        
       
        

         public override void LogicUpdate()
        {
            base.LogicUpdate();





            player.transform.position = enemy.rightShoulder.transform.position;




            if (time < 0)
            {
                // many ways of transiting back. we can also check the current animation clip
                // by doing animator.GetCurrentAnimatorClipInfo(0).
                // or check the animation timing and set based off it.
                
                stateMachine.ChangeState(enemy.seekPlayerState);
                
                
            }
            time -= Time.deltaTime;




        }
         

    }
    
    
}
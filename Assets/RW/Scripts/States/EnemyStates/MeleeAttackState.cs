using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class MeleeAttackState : AlertState
    {

        private float time;
        private bool hit;
        

        private Vector3 previousWanderDestination;
        public MeleeAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            
            DisplayOnUI(UIManager.Alignment.Right);
            
            enemy.animator.SetTrigger("Melee");


            hit = false; // have not hit yet

            
            


            time = 2;

        }
        



         public override void LogicUpdate()
        {
            base.LogicUpdate();


            if (time < 1 && !hit) // under 1 second left, to sync up the animation timing. and if it's not already hit.
            {
                Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, 3); // hit radius of 3


                foreach (Collider collider in colliders)
                {
                    // this iteration is so we can scale to hitting
                    // other entities in the future.
                    // of course, we can just call the character object in the enemy instance
                    // but there's no future proof in that.
                    // we can also hit multiple "characters" like this then.


                    if (collider.name.Equals("Character"))
                    {

                     
                        Character character = collider.GetComponent<Character>();
                     
                        character.movementSM.ChangeState(character.hurt);
                    }
                }

                hit = true;
            }
            
            else if (time < 0)
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
using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class GrappleAttackState : AlertState
    {

        private float time;
        private GameObject player;
        private bool grappled; 
        

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

            grappled = false;


        }
        
       
        

         public override void LogicUpdate()
        {
            base.LogicUpdate();





            player.transform.position = enemy.rightShoulder.transform.position +(enemy.rightShoulder.transform.forward);



            if (time < 0)
            {
                Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, 5); // hit radius of 3

        

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
                     
                        
                        character.movementSM.ChangeState(character.getThrown);
                 
                    }
                }
                
                // many ways of transiting back. we can also check the current animation clip
                // by doing animator.GetCurrentAnimatorClipInfo(0).
                // or check the animation timing and set based off it.
                
                stateMachine.ChangeState(enemy.seekPlayerState);
                
                
            }
            time -= Time.deltaTime;




        }
         

    }
    
    
}
using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class SlamGroundState : AlertState
    {

        private float time;
        private GameObject player;

        

        private Vector3 previousWanderDestination;
        public SlamGroundState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
         
            DisplayOnUI(UIManager.Alignment.Right); // display the state on the right side for enemy.
            
            enemy.animator.SetBool("Slam", true); // turn the animation on

            player = enemy.characterObject;

            time = 1.5f; // set time to start from 1.5 seconds. to count down later.
            enemy.slamFloor.SetActive(true); // slam floor visual on

            



        }
        
       
        

         public override void LogicUpdate()
        {
            base.LogicUpdate();

            enemy.stamina -= Time.deltaTime; // lose stamina while seeking



            //player.transform.position = enemy.rightShoulder.transform.position +(enemy.rightShoulder.transform.forward);

            if (time <= 0.5f) // if time is under 0.5 seconds alreaedy
            {
                enemy.GetComponent<CapsuleCollider>().center = new Vector3(0, 1.4f, 0);
                // so the animation will be on the ground as it needs to kneel
                
              
            }


            if (time < 0) // if time is up
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
                     
                        
                        character.movementSM.ChangeState(character.falldown);
                        
                        
                 
                    }
                }
                
                enemy.slamFloor.SetActive(false); // slam floor visual off
                
                // many ways of transiting back. we can also check the current animation clip
                // by doing animator.GetCurrentAnimatorClipInfo(0).
                // or check the animation timing and set based off it.
                
                stateMachine.ChangeState(enemy.seekPlayerState);
                enemy.animator.SetBool("Slam", false); // turn the animation off.
                
                
                
            }
            time -= Time.deltaTime;




        }

         public override void Exit()
         {
             base.Exit();
             enemy.GetComponent<CapsuleCollider>().center = new Vector3(0, 1f, 0);
             // reset the center
         }
    }
    
    
}
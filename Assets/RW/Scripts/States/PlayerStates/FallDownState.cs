using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class FallDownState : State // can't move during get thrown. so inherit just state.
    {
        private int thrownParam = Animator.StringToHash("Fall");

        private CapsuleCollider capsuleCollider;
        private float time; 
        // constructor receive and to fill in the base class' constructor values.
        public FallDownState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        // on state enter
        public override void Enter()
        {
            base.Enter();

                
            // remove the movement speed, so the player can't walk away while being throw.

            character.StateMovementSpeed = 0;
      
            
            
            // this state is changed by the attacking enemy. 
            // this is why this state can trigger from any states.
            character.SetAnimationBool(thrownParam, true);

            time = 3; // give it a  3 seconds cooldown

            capsuleCollider = character.GetComponent<CapsuleCollider>();
            capsuleCollider.height = character.FallColliderHeight; // this is so, when
            // the fall animation plays, the collider is small enough that it doesn't levitate the player
            // when its lying down.
            // this makes it so the player lays on the ground and not floating above it.
            // we don't want to use the character.ColliderSize because it changes the center too.
            // we just want to change the height, that's all.

            character.HurtPlayer();
        }

        public override void Exit()
        {
            base.Exit();
            character.ColliderSize = character.NormalColliderHeight ; // set back to normal collider height
        }


        public override void LogicUpdate()
        {
            base.LogicUpdate();


            

            

            if (time < 0.3f) // when cool down is over (altered for the animation timing)
            {
                // then we swap back to previous state.




                if (!character.GetCurrentAnimation().Equals("FallDown")) // if its not the fall down animation anymore
                {
                    
              
                    stateMachine.ChangeState(stateMachine.PreviousState);
                }
                   
            }
            else if (time < 0.7f)
            {
                capsuleCollider.height = character.NormalColliderHeight;
                // swap back to normal collider height.
            }
            else if (time < 1f)
            {
                character.SetAnimationBool(thrownParam, false); // set the animation off early.
                
              
            }

            time -= Time.deltaTime;
        }
    }
}
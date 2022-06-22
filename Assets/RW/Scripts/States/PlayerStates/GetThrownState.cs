using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class GetThrownState : State // can't move during get thrown. so inherit just state.
    {
        private int thrownParam = Animator.StringToHash("Fall");

        private float time; 
        // constructor receive and to fill in the base class' constructor values.
        public GetThrownState(Character character, StateMachine stateMachine) : base(character, stateMachine)
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
            
            character.GetComponent<CapsuleCollider>().height = character.FallColliderHeight; // this is so, when
            // the fall animation plays, the collider is small enough that it doesn't levitate the player
            // when its lying down.
            // this makes it so the player lays on the ground and not floating above it.
            // we don't want to use the character.ColliderSize because it changes the center too.
            // we just want to change the height, that's all.


        }

        public override void Exit()
        {
            base.Exit();
            character.ColliderSize = character.NormalColliderHeight ; // set back to normal collider height
        }


        public override void LogicUpdate()
        {
            base.LogicUpdate();


            if (time < 0) // when cool down is over
            {
                // then we swap back to previous state.
                
                character.SetAnimationBool(thrownParam, false);
                stateMachine.ChangeState(stateMachine.PreviousState);
            }

            time -= Time.deltaTime;
        }
    }
}
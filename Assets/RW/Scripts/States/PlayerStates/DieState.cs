using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class DieState : State // can't move during get thrown. so inherit just state.
    {
        private int dieParam = Animator.StringToHash("Die");

        private float time; 
        // constructor receive and to fill in the base class' constructor values.
        public DieState(Character character, StateMachine stateMachine) : base(character, stateMachine)
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
            character.TriggerAnimation(dieParam);

            time = 3; // give it a  3 seconds cooldown
            
            character.GetComponent<CapsuleCollider>().height = character.FallColliderHeight; // this is so, when
            // the fall animation plays, the collider is small enough that it doesn't levitate the player
            // when its lying down.
            // this makes it so the player lays on the ground and not floating above it.
            // we don't want to use the character.ColliderSize because it changes the center too.
            // we just want to change the height, that's all.

  

        }
        


    }
}
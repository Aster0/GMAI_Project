using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class HurtState : GroundedState // so still can move during unarmed state
    {
        private int hurtParam = Animator.StringToHash("Hurt");

        private float time; 
        // constructor receive and to fill in the base class' constructor values.
        public HurtState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        // on state enter
        public override void Enter()
        {
            base.Enter();

            // set the movement speed. this way, we can make it slower when it's armed as well.
            character.StateMovementSpeed = character.MovementSpeedSword; // when equipped with a sword, the down side is that u move slower./
            // but the incentive as explained in the SwingSwordState is that you have more attack speed.
            rotationSpeed = character.RotationSpeed;  
            
            
            // this state is changed by the attacking enemy. 
            // this is why this state can trigger from any states.
            character.TriggerAnimation(hurtParam);

            time = 1; // give it a second cooldown


            character.HurtPlayer();
         
              
        }
        
        


        public override void LogicUpdate()
        {
            base.LogicUpdate();


            if (time < 0) // when cool down is over
            {
                // then we swap back to previous state.
                
                stateMachine.ChangeState(stateMachine.PreviousState);
            }

            time -= Time.deltaTime;
        }
    }
}
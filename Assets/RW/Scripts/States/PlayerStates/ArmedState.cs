using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class ArmedState : GroundedState // so still can move during unarmed state
    {
        private bool sheathSwordKey; // if the player pressed to punch.
        
        // constructor receive and to fill in the base class' constructor values.
        public ArmedState(Character character, StateMachine stateMachine) : base(character, stateMachine)
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
            
     
       
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            sheathSwordKey = Input.GetButtonDown("Fire2"); // GetButtonDown because we only want the input
            // to detect ONCE. GetButton detects as it's holding down, might not be very good for sheathing and drawing.
        }
        


        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (sheathSwordKey)
            {
                stateMachine.ChangeState(character.sheathSword); // change to sheath sword state.
            }
        }
    }
}
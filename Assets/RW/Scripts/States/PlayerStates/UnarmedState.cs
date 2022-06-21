using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class UnarmedState : GroundedState // so still can move during unarmed state
    {
        private bool drawSwordKey; // if the player pressed to punch.
        
        // constructor receive and to fill in the base class' constructor values.
        public UnarmedState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        
        // on state enter
        public override void Enter()
        {
            base.Enter();

            // set the movement speed. this way, we can make it slower when it's armed as well.
            character.StateMovementSpeed = character.MovementSpeed; // in unarmed, you move faster. however
            // as explained in the SwingSword state, unarmed have lower attack speed than when fighting with a sword.
            rotationSpeed = character.RotationSpeed;  
            
            // to draw sword
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            drawSwordKey = Input.GetButtonDown("Fire2"); // GetButtonDown because we only want the input
            // to detect ONCE. GetButton detects as it's holding down, might not be very good for sheathing and drawing.
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (drawSwordKey)
            {
                stateMachine.ChangeState(character.drawSword); // change to draw sword state.
            }
        }
    }
}
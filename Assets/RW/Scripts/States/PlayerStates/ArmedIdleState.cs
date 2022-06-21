using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class ArmedIdleState : ArmedState
    {
        private bool swingSwordKeyPress; // if the player pressed to swing the sword.
        
        // constructor receive and to fill in the base class' constructor values.
        public ArmedIdleState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        
        // override the virtual method on specific inputs we want to handle for this state. 
        public override void HandleInput()
        {
            base.HandleInput();
            
            swingSwordKeyPress = Input.GetButton("Fire1");
        }

        // on frame update
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (swingSwordKeyPress)
            {
                stateMachine.ChangeState(character.swingSword);
            }
        }
    }
}
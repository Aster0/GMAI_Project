using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class UnarmedIdleState : UnarmedState
    {
        private bool punchKeyPress; // if the player pressed to punch.
        
        // constructor receive and to fill in the base class' constructor values.
        public UnarmedIdleState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        
        // override the virtual method on specific inputs we want to handle for this state. 
        public override void HandleInput()
        {
            base.HandleInput();
            
            punchKeyPress = Input.GetButton("Fire1");
        }

        // on frame update
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (punchKeyPress)
            {
                stateMachine.ChangeState(character.punch);
            }
        }
    }
}
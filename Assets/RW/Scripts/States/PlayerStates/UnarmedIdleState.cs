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

        public override void Enter()
        {
            base.Enter(); 
            stateMachine.PreviousState = this; // set this as previous state
            // in enter() because when we swing the sword,
            // and while punching, the player gets hurt,
            // the previous state should be unarmed idle and not punching state.
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
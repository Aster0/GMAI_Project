using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class PunchState : UnarmedState
    {
        private int rightPunch = Animator.StringToHash("RightPunch");
        private int leftPunch = Animator.StringToHash("LeftPunch");


        private bool hit;
        private float time;
        
        // constructor receive and to fill in the base class' constructor values.
        public PunchState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }


        // on state enter
        public override void Enter()
        {
            base.Enter();



            character.TriggerSwingOrPunchAnimation(leftPunch, rightPunch);
            // parameter allows multiple indexes.
            // this allows for a more dynamic coding for in the future when we have more animations.


            time = 1; // set cooldown as 1


        }


        // on frame update
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            character.FinishHurting(time, character.unarmedIdle);
            // handles what happens when the enemy is finished hurting ^
            // Hit is handled in HitBox.cs - this is because the default project provided that script
            // so to try to work with the project, I have used that script.

            time -= Time.deltaTime; // count down time
            

        }
    }
}
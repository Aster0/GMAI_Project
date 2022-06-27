using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class SwingSwordState : ArmedState
    {
        private int swingSword1 = Animator.StringToHash("SwingMelee");
        private int swingSword2 = Animator.StringToHash("SwingMelee2");

        private bool hit;

        private float time;
        
        // constructor receive and to fill in the base class' constructor values.
        public SwingSwordState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }


        // on state enter
        public override void Enter()
        {
            base.Enter();

           

            hit = false; // not yet hit.
            
            time = 0.5f; // set cooldown as 0.5 - swing sword is faster as we want a sword to have faster attack speed.
            // incentive of equipping a sword.
            
            
            character.TriggerSwingOrPunchAnimation(swingSword1, swingSword2);
            // parameter allows multiple indexes.
            // this allows for a more dynamic coding for in the future when we have more animations.


        }


        // on frame update
        public override void LogicUpdate()
        {
            base.LogicUpdate();


            character.FinishHurting(time, character.armedIdle);

            time -= Time.deltaTime;

        }
    }
}
using System;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class TameNPCState : State // not connected to groundeed or anything because we dont want movement or anything during
    // this state.
    {
        private int tame = Animator.StringToHash("Tame");



        private bool hit;
        private float time;
        
        // constructor receive and to fill in the base class' constructor values.
        public TameNPCState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }


        // on state enter
        public override void Enter()
        {
            base.Enter();



            character.TriggerAnimation(tame);


            



            
          

            time = 3; // 2 seconds to tame




        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (time < 0)
            {
                if (character.TamedCreatureCollider != null) // null check just to make sure.
                {
                    CreatureInfo creatureInfo = character.TamedCreatureCollider.GetComponent<CreatureInfo>();
                    // get the creature's info instance so we can tame that specific creature.
                    
                    creatureInfo.TameCreature(character.gameObject); // tame the creature
                 
                }
                
                stateMachine.ChangeState(stateMachine.PreviousState); // change back to previous state.
            }

            time -= Time.deltaTime;
        }
    }
}
using System;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class TameNPCState : UnarmedState
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



            float nearestDistance = Mathf.Infinity;
            Collider nearestCollider = null;
            
            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, 3); // tame radius of 3


            foreach (Collider collider in colliders)
            {
                // this iteration is so we can scale to hitting
                // other entities in the future.
                // of course, we can just call the character object in the enemy instance
                // but there's no future proof in that.
                // we can also hit multiple "characters" like this then.


                if (collider.CompareTag("Creature"))
                {

                    float distance = Vector3.Distance(character.transform.position, collider.transform.position);
                    if (distance < nearestDistance)
                        // meaning this new one is the nearest between the player and the creature
                    {
                        nearestDistance = distance;
                        nearestCollider = collider;
                        // update the nearest colliders
                    }
                }
            }
            
            // now, we only tame the closest. so we don't tame all the creatures in the vicinity if there are more than 1.


            if (nearestCollider != null)
            {
                CreatureInfo creatureInfo = nearestCollider.GetComponent<CreatureInfo>();

                creatureInfo.isTamed = true;
                creatureInfo.owner = character.gameObject;
            }
            
            stateMachine.ChangeState(stateMachine.PreviousState); // change back to previous state.
            



        }


    }
}
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Idle for the creature behavior")]
    public class Idle : Action
    {

    
        private CreatureInfo creatureInfo;
        private Animator animator;
        public SharedVector3 restingSpot;

        public override void OnStart()
        {
            base.OnStart();
            creatureInfo = GetComponent<CreatureInfo>();
            animator = GetComponent<Animator>();
            
            animator.SetFloat("Forward", 0); // walking aniamtion is off.

            restingSpot.Value = transform.position; // this position is the resting spot. 
            // as idle is the first state, so wherever the creature is at first marks its resting spot.



        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
            // do nothing just return running
        }


    }
}
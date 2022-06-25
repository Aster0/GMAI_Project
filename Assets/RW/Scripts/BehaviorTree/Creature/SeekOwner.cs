using BehaviorDesigner.Runtime.Tasks;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Seeks the owner")]
    public class SeekOwner : Action
    {
        private AStarManager aStarManager;
        private CreatureInfo creatureInfo;

        private Animator animator;

   
        
        public override void OnStart()
        {
            base.OnStart();

            aStarManager = GetComponent<AStarManager>();
            creatureInfo = GetComponent<CreatureInfo>();
            animator = GetComponent<Animator>();
            
            animator.SetBool("Run", true); // run animation on

        }

        public override TaskStatus OnUpdate()
        {

            if (creatureInfo.owner != null) // null check
            {
                if(Vector3.Distance(transform.position, creatureInfo.owner.transform.position) > 4) // if above 4 radius then keep following.
                    aStarManager.aStarManager.BuildPath(creatureInfo.owner.transform.position); // then we follow the owner.
            }
              
            
            return TaskStatus.Running;
            // just return running 
        }


        public override void OnEnd()
        {
            base.OnEnd();
            
            animator.SetBool("Run", false); // run animation off when this node returns a failure.
        }
    }
}
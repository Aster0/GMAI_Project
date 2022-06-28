using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Seeks the a coin")]
    public class SeekToCoin : Action
    {
        private AStarManager aStarManager;
        private CreatureInfo creatureInfo;

        private Animator animator;
        public SharedGameObject coinObject;

        private bool collected ;

   
        
        public override void OnStart()
        {
            base.OnStart();
            collected = false;

            aStarManager = GetComponent<AStarManager>();
            creatureInfo = GetComponent<CreatureInfo>();
            animator = GetComponent<Animator>();


        }

        public override TaskStatus OnUpdate()
        {

            if (coinObject != null) // null check
            {
                if(Vector3.Distance(transform.position, coinObject.Value.transform.position) > 1) // if above 1 radius then keep following.
                    aStarManager.aStarPathFinding.BuildPath(coinObject.Value.transform.position); // then we follow the coin.
                
                
                if(Vector3.Distance(transform.position, coinObject.Value.transform.position) < 3)
                {
                    if (!collected)
                    {
                   
                        animator.SetTrigger("Collect"); // trigger collect coin anim
                        collected = true;
                    }
                  
                }
            }
       
              
            
            return TaskStatus.Running;
            // just return running 
        }


  
    }
}
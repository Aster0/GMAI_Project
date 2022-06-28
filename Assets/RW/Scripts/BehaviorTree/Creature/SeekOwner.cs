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



   
        
        public override void OnStart()
        {
            base.OnStart();

            aStarManager = GetComponent<AStarManager>();
            creatureInfo = GetComponent<CreatureInfo>();

    

        }

        public override TaskStatus OnUpdate()
        {

            if (creatureInfo.owner != null) // null check
            {
                if(Vector3.Distance(transform.position, creatureInfo.owner.transform.position) > 5) // if above 5 radius then keep following.
                    aStarManager.aStarPathFinding.BuildPath(creatureInfo.owner.transform.position); // then we follow the owner.
            }
              
            
            return TaskStatus.Running;
            // just return running 
        }


 
    }
}
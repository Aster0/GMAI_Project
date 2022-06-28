using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Goes back to old resting spot.")]
    public class GoBackOldSpot : Action
    {
        private AStarManager aStarManager;
        private CreatureInfo creatureInfo;

        public SharedVector3 restingSpot;



   
        
        public override void OnStart()
        {
            base.OnStart();

            aStarManager = GetComponent<AStarManager>();
            creatureInfo = GetComponent<CreatureInfo>();

            creatureInfo.isTamed = false; // idling means tamed is not true.

        }

        public override TaskStatus OnUpdate()
        {

            if (Vector3.Distance(transform.position, restingSpot.Value) > 2) // if distance is more than 2
            {
                aStarManager.aStarPathFinding.BuildPath(restingSpot.Value); // we continue path finding to the resting spot.
              
            
                return TaskStatus.Running;
                // just return running 
            }
            else 
            {
                
                
                return TaskStatus.Success; // if under distance of 2, return successful as it's at the resting spot now.
            }
        
        }


 
    }
}
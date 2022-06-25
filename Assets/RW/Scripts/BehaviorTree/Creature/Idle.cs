using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Idle for the creature behavior")]
    public class Idle : Action
    {

        private Animation animation;

        public override void OnStart()
        {
            base.OnStart();

     
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
            // do nothing just return running
        }


    }
}
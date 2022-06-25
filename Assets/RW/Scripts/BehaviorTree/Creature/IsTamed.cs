using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Returns success if the creature is tamed.")]
    public class IsTamed : Conditional
    {



        private CreatureInfo creatureInfo;


        public override void OnStart()
        {
            base.OnStart();

            creatureInfo = GetComponent<CreatureInfo>();
        }

        public override TaskStatus OnUpdate()
        {
            return creatureInfo.isTamed ? TaskStatus.Success : TaskStatus.Failure;
            // if tamed == true, we'll return a success. else a failure.

        }
    }
}
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Returns success if the creature's shield cooldown is over.")]
    public class IsShieldCoolDownOver : Conditional
    {
        


        
        private CreatureInfo creatureInfo;


        public override void OnStart()
        {
            base.OnStart();

            creatureInfo = GetComponent<CreatureInfo>();
        }

        public override TaskStatus OnUpdate()
        {
            creatureInfo.shieldCooldown -= Time.deltaTime; // reduce the cooldown
            
            return creatureInfo.shieldCooldown <= 0 ? TaskStatus.Success : TaskStatus.Failure;
            // if shield cooldown hit 0 or below, then success. if not, fail.

        }
    }
}
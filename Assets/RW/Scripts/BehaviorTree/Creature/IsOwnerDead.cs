using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Returns success if the creature's owner is dead")]
    public class IsOwnerDead : Conditional
    {



        private CreatureInfo creatureInfo;
        private Character character;


        public override void OnStart()
        {
            base.OnStart();

            creatureInfo = GetComponent<CreatureInfo>();

            character = creatureInfo.owner.GetComponent<Character>();
            
            
        }

        public override TaskStatus OnUpdate()
        {

            return character.movementSM.CurrentState == character.die ? TaskStatus.Success : TaskStatus.Failure;
            // if player is in die state, return success. else failure.

        }
    }
}
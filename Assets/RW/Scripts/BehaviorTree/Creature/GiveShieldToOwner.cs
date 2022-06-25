using BehaviorDesigner.Runtime.Tasks;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.BehaviorTree.Creature
{
    [TaskCategory("Creature")]
    [TaskDescription("Returns success if it's able to give the owner a shield.")]
    public class GiveShieldToOwner : Action
    {
 
        private CreatureInfo creatureInfo;

    

        private float time;
        private Character character;

   
        
        public override void OnStart()
        {
            base.OnStart();

 
            creatureInfo = GetComponent<CreatureInfo>();



            int chance = Random.Range(0, 100);


            if (chance <= 90) // if chance is 1%
            {
                character = creatureInfo.owner.GetComponent<Character>();

                character.ToggleShield(true); // turn on the shield

                time = 2; // 2 second shield
                
                
            }


        }


        public override TaskStatus OnUpdate()
        {
         
            
            // the creature will maintain the shield with its powers
            if (character != null) // character != null means the chance hit,
            {
                if (time < 0)
                {
                
                    character.ToggleShield(false); // turn off the shield
                    creatureInfo.shieldCooldown = 10; // 5 seconds for shield cooldown, so the
                    // next shield wont come too fast.
                    return TaskStatus.Success; // finished its powers to maintain the shield, so go out of the node with a success. 
                }

                time -= Time.deltaTime;

                return TaskStatus.Running; // as long as it's still maintaining t he shield, its still running.
            }
            
            // if not, we just return a failure as it did not give a shield.
            return TaskStatus.Failure;
      
        }
    }
}
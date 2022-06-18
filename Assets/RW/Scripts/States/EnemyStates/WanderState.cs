using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.EnemyStates
{
    public class WanderState : State
    {

        private float time;
        public WanderState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            enemy.aStarPathFinding.SetDestination(new Vector3(4.67f, 0, -4.7f));
        }


         public override void LogicUpdate()
        {
            base.LogicUpdate();

            /*if (time < 0)
            {
                time = 3;

                int randomX = Random.Range(-10, 10);
                int randomZ = Random.Range(-10, 10);
                enemy.aStarPathFinding.SetDestination(new Vector3(randomX,0, randomZ));
            }

            time -= Time.deltaTime;*/
            
            // random wander using A*
            
            enemy.aStarPathFinding.Move();
            
            
          
            
        }
    }
}
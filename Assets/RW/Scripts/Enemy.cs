using RayWenderlich.Unity.StatePatternInUnity.EnemyStates;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class Enemy : MonoBehaviour
    {
        public StateMachine stateMachine ;

        public WanderState wanderState;

        public AStarPathFinding aStarPathFinding;
        
        private void Start()
        {
            aStarPathFinding = GetComponent<AStarPathFinding>();
            stateMachine = new StateMachine();

            wanderState = new WanderState(this, stateMachine);
            
            stateMachine.Initialize(wanderState);
        }
        private void Update()
        {
            stateMachine.CurrentState.HandleInput();

            stateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            stateMachine.CurrentState.PhysicsUpdate ();
        }

    }
}
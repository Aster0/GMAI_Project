using RayWenderlich.Unity.StatePatternInUnity.EnemyStates;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class Enemy : MonoBehaviour
    {
        public StateMachine stateMachine ;

        public WanderState wanderState;

        public AStarManager aStarPathFinding;

        public GameObject characterObject;
        
        private void Start()
        {
            characterObject = GameObject.Find("Character");
            aStarPathFinding = GetComponent<AStarManager>();
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
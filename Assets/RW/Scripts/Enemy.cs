using RayWenderlich.Unity.StatePatternInUnity.EnemyStates;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class Enemy : MonoBehaviour
    {
        public StateMachine stateMachine ;

        public SeekPlayerState seekPlayerState;
        public StandState standState;
        public SitState sitState;
        public MeleeAttackState meleeAttackState;
        public GrappleAttackState grappleAttackState;

        public AStarManager aStarPathFinding;

        public GameObject characterObject;

        public Animator animator;

        public GameObject rightShoulder; 
        private void Start()
        {
            animator = GetComponent<Animator>();
            characterObject = GameObject.Find("Character");
            aStarPathFinding = GetComponent<AStarManager>();
            stateMachine = new StateMachine();

            meleeAttackState = new MeleeAttackState(this, stateMachine);
            grappleAttackState = new GrappleAttackState(this, stateMachine);

            seekPlayerState = new SeekPlayerState(this, stateMachine);
            sitState = new SitState(this, stateMachine);
            standState = new StandState(this, stateMachine);
            
            stateMachine.Initialize(standState);
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
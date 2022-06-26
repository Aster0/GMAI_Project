using RayWenderlich.Unity.StatePatternInUnity.EnemyStates;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;
using UnityEngine.UI;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class Enemy : MonoBehaviour
    {
        public StateMachine stateMachine ;

        // we can assign the start health in the inspector.
        public int health; 

        public float Stamina { get; set; }
        public SeekPlayerState seekPlayerState;
        public StandState standState;
        public SitState sitState;
        public MeleeAttackState meleeAttackState;
        public SlamGroundState slamGroundState;
        public EnemyStates.HurtState hurtState;
        public EnemyStates.DieState dieState;

        public AStarManager aStarPathFinding;

        public GameObject characterObject;

        public Animator animator;

        public GameObject rightShoulder;
        public Rigidbody rb;

        public float seekRadius = 10;

        [SerializeField]
        private Text enemyHealthText;


        public GameObject slamFloor;
        
        private void Start()
        {
            Stamina = 100; // start at 100.
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            characterObject = GameObject.Find("Character");
            aStarPathFinding = GetComponent<AStarManager>();
            stateMachine = new StateMachine();

            meleeAttackState = new MeleeAttackState(this, stateMachine);
            slamGroundState = new SlamGroundState(this, stateMachine);

            seekPlayerState = new SeekPlayerState(this, stateMachine);
            sitState = new SitState(this, stateMachine);
            standState = new StandState(this, stateMachine);
            dieState = new EnemyStates.DieState(this, stateMachine); // EnemyStates.DieState because its a different 
            // namespace from the player's die state class.
            hurtState = new EnemyStates.HurtState(this, stateMachine); // EnemyStates.HurtState because its a different 
            // namespace from the player's hurt state class.
            
            stateMachine.Initialize(standState);
            
            SetEnemyHealth(health); // set the UI
        }
        private void Update()
        {
            stateMachine.CurrentState.HandleInput();

            stateMachine.CurrentState.LogicUpdate();
            
            // any state

            if (health <= 0 && stateMachine.CurrentState != dieState) // less than equal 0 (meaning player should change to die state)
                // and player isn't already dead.
            {
                stateMachine.ChangeState(dieState);
            }
        }

        private void FixedUpdate()
        {
            stateMachine.CurrentState.PhysicsUpdate ();
        }
        
        public void SetEnemyHealth(int value)
        {
            enemyHealthText.text = "ENEMY HEALTH: " + value;
        }

    }
}
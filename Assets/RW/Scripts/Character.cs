﻿/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Character : MonoBehaviour
    {
        #region Variables

        public StateMachine movementSM ;
        public StandingState standing;
        public DuckingState ducking; 
        public JumpingState jumping;
        public DrawSwordState drawSword;
        public UnarmedIdleState unarmedIdle;
        public ArmedIdleState armedIdle;
        public PunchState punch;
        public SwingSwordState swingSword;
        public ShealthSwordState sheathSword;
        public HurtState hurt;
        public FallDownState falldown;
        public DieState die;

        

#pragma warning disable 0649
        [SerializeField]
        private Transform handTransform;
        [SerializeField]
        private Transform sheathTransform;
        [SerializeField]
        private Transform shootTransform;
        [SerializeField]
        private CharacterData data;
        [SerializeField]
        private LayerMask whatIsGround;
        [SerializeField]
        private Collider hitBox;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private ParticleSystem shockWave;
#pragma warning restore 0649
        [SerializeField]
        private float meleeRestThreshold = 10f;
        [SerializeField]
        private float diveThreshold = 1f;
        [SerializeField]
        private float collisionOverlapRadius = 0.1f;

        private GameObject currentWeapon;
        private Quaternion currentRotation;
        private int horizonalMoveParam = Animator.StringToHash("H_Speed");
        private int verticalMoveParam = Animator.StringToHash("V_Speed");
        private int shootParam = Animator.StringToHash("Shoot");
        private int hardLanding = Animator.StringToHash("HardLand");
     

        public GameObject weaponPrefab;
        #endregion

        #region Properties

        public int Health { get; set; } // player's health.
        public bool Jumped { get; set; } // if player has jumped
        public float NormalColliderHeight => data.normalColliderHeight;
        public float CrouchColliderHeight => data.crouchColliderHeight;
        public float FallColliderHeight => data.fallColliderHeight;
        public float DiveForce => data.diveForce;
        public float JumpForce => data.jumpForce;
        public float MovementSpeed => data.movementSpeed;

        public float StateMovementSpeed; // stores the current super state's movement speed (armed/idle)
        public float CrouchSpeed => data.crouchSpeed;
        
        public float MovementSpeedSword => data.movementSpeedSword; // when equipped with a sword
        public float RotationSpeed => data.rotationSpeed;
        public float CrouchRotationSpeed => data.crouchRotationSpeed;
        public GameObject MeleeWeapon => data.meleeWeapon;
        public GameObject ShootableWeapon => data.staticShootable;
        public float DiveCooldownTimer => data.diveCooldownTimer;
        public float CollisionOverlapRadius => collisionOverlapRadius;
        public float DiveThreshold => diveThreshold;
        public float MeleeRestThreshold => meleeRestThreshold;
        public int isMelee => Animator.StringToHash("IsMelee");
        public int crouchParam => Animator.StringToHash("Crouch");
        public int drawSwordParam => Animator.StringToHash("DrawMelee");
        public int sheathSwordParam => Animator.StringToHash("SheathMelee");

        public float ColliderSize
        {
            get => GetComponent<CapsuleCollider>().height;

            set
            {
                GetComponent<CapsuleCollider>().height = value;
                Vector3 center = GetComponent<CapsuleCollider>().center;
                center.y = value / 2f;
                GetComponent<CapsuleCollider>().center = center;
            }
        }

        #endregion

        #region Methods

        public void Move(float speed, float rotationSpeed)
        {
            Vector3 targetVelocity = speed * transform.forward * Time.deltaTime;
            targetVelocity.y = GetComponent<Rigidbody>().velocity.y;
            GetComponent<Rigidbody>().velocity = targetVelocity;

            GetComponent<Rigidbody>().angularVelocity = rotationSpeed * Vector3.up * Time.deltaTime;

            if (targetVelocity.magnitude > 0.01f || GetComponent<Rigidbody>().angularVelocity.magnitude > 0.01f)
            {
                SoundManager.Instance.PlayFootSteps(Mathf.Abs(speed));
            }

            anim.SetFloat(horizonalMoveParam, GetComponent<Rigidbody>().angularVelocity.y);
            anim.SetFloat(verticalMoveParam, speed * Time.deltaTime);
        }

        public void ResetMoveParams()
        {
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            anim.SetFloat(horizonalMoveParam, 0f);
            anim.SetFloat(verticalMoveParam, 0f);
        }

        public void ApplyImpulse(Vector3 force)
        {
            GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }

        public void SetAnimationBool(int param, bool value)
        {
            anim.SetBool(param, value);
        }

        public string GetCurrentAnimation() // method added by me, to get the correct animation the player is in.
        {
            return anim.GetCurrentAnimatorStateInfo(0).ToString();
        }

        public void TriggerAnimation(int param)
        {
            anim.SetTrigger(param);
        }

        public void Shoot()
        {
            TriggerAnimation(shootParam);
            GameObject shootable = Instantiate(data.shootableObject, shootTransform.position, shootTransform.rotation);
            shootable.GetComponent<Rigidbody>().velocity = shootable.transform.forward * data.bulletInitialSpeed;
            SoundManager.Instance.PlaySound(SoundManager.Instance.shoot, true);
        }

        public bool CheckCollisionOverlap(Vector3 point)
        {
            return Physics.OverlapSphere(point, CollisionOverlapRadius, whatIsGround).Length > 0;
        }

        public void Equip(GameObject weapon = null)
        {
            if (weapon != null)
            {
                currentWeapon = Instantiate(weapon, handTransform.position, handTransform.rotation, handTransform);
            }
            else
            {
                ParentCurrentWeapon(handTransform);
            }
        }

        public void DiveBomb()
        {
            TriggerAnimation(hardLanding);
            SoundManager.Instance.PlaySound(SoundManager.Instance.hardLanding);
            shockWave.Play();
        }

        public void SheathWeapon()
        {
            ParentCurrentWeapon(sheathTransform);
        }

        public void Unequip()
        {
            Destroy(currentWeapon);
        }

        public void ActivateHitBox()
        {
            hitBox.enabled = true;
        }

        public void DeactivateHitBox()
        {
            hitBox.enabled = false;
        }

        private void ParentCurrentWeapon(Transform parent)
        {
            if (currentWeapon.transform.parent == parent)
            {
                return;
            }

            currentWeapon.transform.SetParent(parent);
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
        }
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            Health = data.startHealth; // initialize start health of player.
          
            movementSM = new StateMachine();

            standing = new StandingState (this , movementSM);

            ducking = new DuckingState(this, movementSM);

            jumping = new JumpingState(this, movementSM);
            drawSword = new DrawSwordState(this, movementSM);
            unarmedIdle = new UnarmedIdleState(this, movementSM);
            punch = new PunchState(this, movementSM);
            armedIdle = new ArmedIdleState(this, movementSM);

            swingSword = new SwingSwordState(this, movementSM);

            sheathSword = new ShealthSwordState(this, movementSM);
            hurt = new HurtState(this, movementSM);
            falldown = new FallDownState(this, movementSM);
            die = new DieState(this, movementSM);

            Equip(weaponPrefab); // equip
            SheathWeapon(); // then sheath the weapon behind the back
            movementSM.Initialize(unarmedIdle);
        }
        
        private void Update()
        {
            movementSM.CurrentState.HandleInput();

            movementSM.CurrentState.LogicUpdate();
            
            
            // any state

            if (Health <= 0 && movementSM.CurrentState != die) // less than equal 0 (meaning player should change to die state)
            // and player isn't already dead.
            {
                movementSM.ChangeState(die);
            }
        }

        private void FixedUpdate()
        {
            movementSM.CurrentState.PhysicsUpdate ();
        }





        
        #endregion


        #region State Methods

        // these input methods are so it could be used in several states and not needing to repeat slowly. 
        public bool CheckJumpInput()
        {
        
            return Input.GetButtonDown("Jump"); 
        }
        
        public bool CheckCrouchInput()
        {
   
            return Input.GetButtonDown("Fire3"); 
        }

        public void TriggerSwingOrPunchAnimation(params int[] animations) // to trigger the animations of swing or punch.
        // parameter means we can parse as many animation index as we want when using this method.
        // this allows for a more dynamic coding for in the future when we have more animations.
        {
            
            int chance = Random.Range(0, 100);


            int animationToTrigger;
            
            // basically have a 50/50 chance to have a right or left punch animation.
            if (chance <= 50)
            {
                animationToTrigger = animations[0];
            }
            else
            {
                animationToTrigger = animations[1];
            }
            
            TriggerAnimation(animationToTrigger);
            // trigger the animation.

           
        }

        public bool HurtEnemy(float time, bool hit, State state) // method to hurt an enemy
        {

           
            if (time <= 0.5f && !hit)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 3); // hit radius of 3


                foreach (Collider collider in colliders)
                {
                    // this iteration is so we can scale to hitting
                    // other entities in the future.
                    // of course, we can just call the character object in the enemy instance
                    // but there's no future proof in that.
                    // we can also hit multiple "characters" like this then.


                    if (collider.CompareTag("Enemy")) // if its an enemy
                    {

                     
                        Enemy enemy = collider.GetComponent<Enemy>();
                    
                        if(enemy.stateMachine.CurrentState != enemy.dieState) // not dead,
                            enemy.stateMachine.ChangeState(enemy.hurtState); // we hurt!
                    }
                }
                
                Debug.Log("Hit");
                hit = true; // hit = true cause we just hit.

            }
            else if (time < 0)
            {
                // when cooldown is over, we swap back to the unarmed idle state.
                // where we are ready to punch again
                
                
                movementSM.ChangeState(state);
            }

            return hit;

        }

        public void CheckPlayerJumpAndCrouch(bool jump, bool crouch, StateMachine stateMachine, State previousState)
        {
           
            
            if (crouch)
            {
                stateMachine.ChangeState(ducking);
            }
            else if (jump)
            {
                stateMachine.ChangeState(jumping);
                
                
            }
            
            // since jumping and crouching can be from multiple states, we need to save the previousState.
            stateMachine.PreviousState = previousState;
        }

        #endregion
    }
}

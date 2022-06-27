/*
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
    public class GroundedState : State
    {
        protected float speed;
        protected float rotationSpeed;

        private float horizontalInput;
        private float verticalInput;
        



        private bool belowCeiling;
        private bool crouchHeld, jumpHeld, tamePress;
        
        private bool grounded;
        private int jumpParam = Animator.StringToHash("Jump");
        private int landParam = Animator.StringToHash("Land");
        
        public GroundedState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base .Enter();
            horizontalInput = verticalInput = 0.0f;
            
  
        }
        
        public override void Exit()
        {
            base.Exit();
            character.ResetMoveParams();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");
            
            
            crouchHeld = Input.GetButton("Fire3"); // we use t his so we can sense if the player is holding shift.
            jumpHeld = character.CheckJumpInput();

            tamePress = Input.GetButton("Cancel");

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();



      

            if (crouchHeld) // if crouch is pressed,
            {
                // ducking logic have been shifted to grounded state 
                // this is for a more efficient use of my hierarchical state.
                // this is so when you're in unarmed or armed super states,
                // u can still move, jump & crouch. 
                // also u can swing your weapon while jumping & crouching. 
                character.SetAnimationBool (character.crouchParam , true);
                speed = character.CrouchSpeed ;
                rotationSpeed = character.CrouchRotationSpeed;
                character.ColliderSize = character.CrouchColliderHeight;
                belowCeiling = false;
            }
            else if (!(crouchHeld || belowCeiling))
            {
                
           
                // this is changed to fit the new grounded state.
                // basically when it's not pressing crouch button or under a ceiling,
                // the collider becomes the normal height
                // and the animation for crouch stops playing.
                character.SetAnimationBool(character.crouchParam, false);
                character.ColliderSize = character.NormalColliderHeight ;
                
                
                
                speed = character.StateMovementSpeed;
                rotationSpeed = character.RotationSpeed; 
                // we also remember to reset the character's speeds as it's uncrouched now.
                
            }
            
            // separate ifs from the crouch because we want the player to be able to jump while crouching.
            if (jumpHeld && grounded) // also make sure if its grounded then jump.
            {
                grounded = false;
                // set the player to not grounded as they just jumped.
                character.Jumped = true;
                
                Jump(); // call the jump method to jump.
                
            }
            
            
            if (grounded && character.Jumped) // checks if a player has landed look @PhysicsUpdate. also check jumped because we only want this if clause to run after jump and not everytime.
            {
                // if yes, we use the land animation and play a sound.
                character.TriggerAnimation(landParam);
                SoundManager.Instance.PlaySound(SoundManager.Instance.landing);
                
                character.Jumped = false; // landed, so we reset to false.

            }


            if (tamePress)
            {
                float nearestDistance = Mathf.Infinity;
                Collider nearestCollider = null;
            
                Collider[] colliders = Physics.OverlapSphere(character.transform.position, 3); // tame radius of 3


                foreach (Collider collider in colliders)
                {
                    // this iteration is so we can scale to hitting
                    // other entities in the future.
                    // of course, we can just call the character object in the enemy instance
                    // but there's no future proof in that.
                    // we can also hit multiple "characters" like this then.


                    if (collider.CompareTag("Creature"))
                    {

                        float distance = Vector3.Distance(character.transform.position, collider.transform.position);
                        if (distance < nearestDistance)
                            // meaning this new one is the nearest between the player and the creature
                        {
                            nearestDistance = distance;
                            nearestCollider = collider;
                            // update the nearest colliders
                        }
                    }
                }
            
                // now, we only tame the closest. so we don't tame all the creatures in the vicinity if there are more than 1.

                character.TamedCreatureCollider = nearestCollider; // save a reference to it, so
                // we can use it in the tamed state.
                
                if(nearestCollider != null)
                    stateMachine.ChangeState(character.tameNPC);
            }
            
            

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
           
            character.Move(verticalInput * speed, 
                horizontalInput * rotationSpeed);
            
            
            // to check if player is below a ceiling.
            belowCeiling = character.CheckCollisionOverlap(
                character.transform.position +
                Vector3.up * character.NormalColliderHeight);


            if (!grounded)
            {
                // we only want to check when the player is in the sky, so !grounded
                grounded = 
                    character.CheckCollisionOverlap(character.transform.position); // constantly check if the player is grounded.

                // grounded will be set back to true eventually and stay there until the player jumps again.
                
            }
            
        }
        
        // jumping logic have been shifted to grounded state 
        // this is for a more efficient use of my hierarchical state.
        // this is so when you're in unarmed or armed super states,
        // u can still move, jump & crouch. 
        // also u can swing your weapon while jumping & crouching. 
        private void Jump()
        {
            character.transform.Translate(Vector3.up * 
                                          (character.CollisionOverlapRadius + 0.1f));

            character.ApplyImpulse(Vector3.up * character.JumpForce);
            character.TriggerAnimation(jumpParam);
        }
        




    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.PathFinding
{
    public class AStarManager : MonoBehaviour
    {
 

        private Animator animator;


        private List<Vector3> destinationGridIndexes = new List<Vector3>();

        private GameObject character;

        public AStarPathFinding aStarManager;

        private Coroutine movementCoroutine;
        private Rigidbody rb;

        private Vector3 lastPosition;

        private void Awake()
        {
            character = GameObject.Find("Character");
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {


    
            
            //if(movementCoroutine != null)
              //  StopCoroutine(movementCoroutine);
                
            //movementCoroutine = StartCoroutine(MovementCoroutine());
            
                                        
            aStarManager = new AStarPathFinding();

            aStarManager.animator = animator;
            aStarManager.transform = this.transform;
            aStarManager.rb = rb;
        }


        public IEnumerator StartPathFind(GameObject gameObject)
        {
                            
            aStarManager = new AStarPathFinding();

            aStarManager.animator = animator;
            aStarManager.transform = this.transform;
            aStarManager.rb = rb;
            
            while (true)
            {


                // SEE THE EXPLANATION ON THE CODE FOR COROUTINE BELOW.

                if (lastPosition != gameObject.transform.position)
                {
                    aStarManager.SetDestination(gameObject.transform.position);
                    lastPosition = gameObject.transform.position;
                }
          
      
                
                
            
                yield return new WaitForSeconds(0.2f);

                
       
            }
      

        }
        
        public IEnumerator MovementCoroutine()
        {
            // This here needs a coroutine because so we can set a new destination (above's coroutine) while moving to the old's
            // location. Coroutine lets us do both, something like asynchronous. 
            // So while waiting for the StartPathFind() method to run to set a new destination on the new
            // player's position,
            // this coroutine here is moving to the old player's position. So it never stops and keeps updating.
            // once we have the new destination, the next frame (yield return null) runs the Move() function so
            // we build the new set of A* grid points to move to the new player's position.
            while(true)
            {
                if(aStarManager != null)
                    aStarManager.Move();
                yield return null;
            }
        }


    }
}
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


        
        
 


    }
}
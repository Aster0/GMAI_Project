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

        private AStarPathFinding aStarManager;

        private Coroutine movementCoroutine;
        private Rigidbody rb;

        private Vector3 lastPosition;

        private void Start()
        {
            character = GameObject.Find("Character");
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            
            
            if(movementCoroutine != null)
                StopCoroutine(movementCoroutine);
                
            movementCoroutine = StartCoroutine(AiTick());
        }


        public IEnumerator StartPathFind(GameObject gameObject)
        {
                            
            aStarManager = new AStarPathFinding();

            aStarManager.animator = animator;
            aStarManager.transform = this.transform;
            aStarManager.rb = rb;
            
            while (true)
            {




                if (lastPosition != gameObject.transform.position)
                {
                    aStarManager.SetDestination(gameObject.transform.position);
                    lastPosition = gameObject.transform.position;
                }
          
      
                
                
            
                yield return new WaitForSeconds(0.5f);

                
       
            }
      

        }
        
        public IEnumerator AiTick()
        {
            while(true)
            {
                if(aStarManager != null)
                    aStarManager.Move();
                yield return null;
            }
        }


    }
}
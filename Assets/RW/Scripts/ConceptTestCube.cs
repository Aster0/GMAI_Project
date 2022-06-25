using System;
using System.Collections;
using System.Collections.Generic;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;

public class ConceptTestCube : MonoBehaviour
{

    private AStarManager aStarManager;

    private GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        aStarManager = GetComponent<AStarManager>();
        //StartCoroutine(GetComponent<AStarManager>().StartPathFind(GameObject.Find("Character")));

        character = GameObject.Find("Character");

    }

    private void Update()
    {
  
        aStarManager.aStarManager.BuildPath(character.transform.position);
        
       
    }
}





using System;
using System.Collections;
using System.Collections.Generic;
using RayWenderlich.Unity.StatePatternInUnity.PathFinding;
using UnityEngine;

public class ConceptTestCube : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetComponent<AStarManager>().StartPathFind(GameObject.Find("Character")));
    }


}





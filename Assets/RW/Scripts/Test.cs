using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Test2 test;
    // Start is called before the first frame update
    void Start()
    {
        test = new Test2();

        StartCoroutine(PlayTest());
    }


    public IEnumerator PlayTest()
    {
        while (true)
        {
            test.Test();
            yield return new WaitForSeconds(5f);
        }
        
    }

    
}


[Serializable]
public class Test2 
{

    public bool hey;
    
    public void Test()
    {
        if (hey)
        {
            Debug.Log("Hi");
        }
        else
        {
            Debug.Log("Ho");
        }
        
    }
}


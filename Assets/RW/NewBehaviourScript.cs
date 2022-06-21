using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Testing
{
    public int index;

    public Testing(int i)
    {
        index = i;
    }
}
public class NewBehaviourScript : MonoBehaviour
{

    public List<Testing> test;
    // Start is called before the first frame update
    void Start()
    {
        test.Add(new Testing(4));
        test.Add(new Testing(3));
        test.Add(new Testing(7));
        test.Add(new Testing(1));

        test.OrderBy(i => i.index);
        test.Reverse();
        Debug.Log("Order");

        foreach (Testing test1 in test)
        {
            Debug.Log(test1.index);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

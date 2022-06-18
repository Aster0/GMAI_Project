using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Test2
{
    // Start is called before the first frame update
    void Start()
    {
        Enter();
    }

    public override void Enter()
    {
        base.Enter();
        
    
    }
}


public class Test2 : TestBase
{
    public override void Enter()
    {
        base.Enter();
        
       
    }
}

public class TestBase : MonoBehaviour
{
    public virtual void Enter()
    {
        Debug.Log("TESTBASE");
    }
}

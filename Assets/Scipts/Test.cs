using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;



public class Test : MonoBehaviour
{

    Action<Transform> action = transform => 
    {
        transform.position += Vector3.up * 10f;
    };

    private async void Func()
    {
        await Task.Run(
            () => action(transform)
        );

        await Func2();

    }


    private Task Func2()
    {   
        return Task.Run(
          () => {

          }  
        );
    }

    delegate void MyTest( int i);

    MyTest test;

    void V( int i)
    {

    }

    void Start()
    {
        Func();
        test = V;
       //test => () => {};
        Debug.Log("Hello");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

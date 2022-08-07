using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FabrikaBalls : MonoBehaviour
{
    private int countBalls;
    private Ball[] _balls;
    private BallMover[] _ballmovers;

    [SerializeField] private BallMover _ballmover;
    [SerializeField] private Ball _ball;
    [SerializeField] private A a;




    private void Awake()
    {
         Pool<A> pool;
         IPoolFactory<A> factory = new PrefabFactory<A>(a, transform, "test");
         factory.Create();
        pool = new Pool<A>(factory, 10);

        float index = 0;
        A p = pool.Get();
        p.transform.position = new Vector3(index, 0f, 0f);
        index+=1.5f;
        A b = pool.Get();
        b.transform.position = new Vector3(index, 0f, 0f);
        index+=1.5f;
        pool.Return(b);
        

        //GameObject ball = Instantiate( _ball.transform.gameObject, _ball.transform.position, Quaternion.identity);
    }


    private void OnEnable()
    {

    }




    
}

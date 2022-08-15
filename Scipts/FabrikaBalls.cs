using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FabrikaBalls : MonoBehaviour
{
    private List<Ball> _balls;
    [SerializeField] private Ball _prefabBall;

    private void Awake()
    {

        Pool<Ball> pool;
        IPoolFactory<Ball> factory = new PrefabFactory<Ball>(_prefabBall, transform, "ball");
        pool = new Pool<Ball>(factory,50);

        _balls = new List<Ball>();

        for( int i = 0 ; i < 0 ; i++ )
        {
            Ball ball = pool.Get();
            ball.RandomStartVector(i);
            _balls.Add( ball );
        }
    
     }
}

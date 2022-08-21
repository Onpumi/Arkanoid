using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class FabrikaBalls : MonoBehaviour
{
    private HashSet<Ball> _balls;
    Pool<Ball> pool;
    IPoolFactory<Ball> factory;
    [SerializeField] private Ball _prefabBall;
    private Vector3 _startDirection;

    public event Action OnDestroyBall;
    public event Action<Vector3> OnReproductionBall;

    private void Awake()
    {

        factory = new PrefabFactory<Ball>(_prefabBall, transform, "ball");
        pool = new Pool<Ball>(factory,5000);

        _balls = new HashSet<Ball>();

        for( int i = 0 ; i < 0 ; i++ )
        {
            Ball ball = pool.Get();
            ball.InitVelocity(i*5);
     //       _balls.Add( ball );
        }

        _startDirection = Vector3.up;
     }



     public void SpawnBall( Ball ballOrigin, int countSpawn )
     {

        for( int i = 0 ; i < countSpawn ; i++ )
        {
          Ball ball = pool.Get();
          ball.transform.position = ballOrigin.transform.position;
          var angleSpawn = UnityEngine.Random.Range(0,360);
          ball.InitVelocity( angleSpawn );
        }
        OnReproductionBall?.Invoke(_startDirection);
     }
     public void DestroyBall( Ball ball )
     {
        pool.Return(ball);
     }




}

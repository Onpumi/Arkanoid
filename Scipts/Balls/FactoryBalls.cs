using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class FactoryBalls : MonoBehaviour
{
    private HashSet<Ball> _balls;
    Pool<Ball> pool;
    IPoolFactory<Ball> factory;
    [SerializeField] private Ball _prefabBall;
    //private Vector3 _startDirection;

    public event Action OnLossAllBalls;
    public event Action OnReproductionBall;
    private int countBalls = 1;
    public int CountBalls => countBalls;

    private void Awake()
    {

        factory = new PrefabFactory<Ball>(_prefabBall, transform, "ball");
        pool = new Pool<Ball>(factory,5000);

        _balls = new HashSet<Ball>();
        //_startDirection = Vector3.up;
     }

     public void SpawnBall( Ball ballOrigin, int countSpawn, Transform startPoint = null, float[] anglesSpawn = null )
     {

        for( int i = 0 ; i < countSpawn ; i++ )
        {
          Ball ball = pool.Get();
          if( startPoint == null )
          {
            ball.transform.position = ballOrigin.transform.position;
          }
          else
          {
            ball.transform.position = startPoint.position + Vector3.up * startPoint.localScale.y;
          }
          float angleSpawn = 0;
          if( anglesSpawn == null )
          {
            angleSpawn = UnityEngine.Random.Range(0,360);
            ball.InitVelocity( angleSpawn );
          }
          else if( anglesSpawn.Length >= countSpawn )
          {
            angleSpawn = anglesSpawn[i];
          }
          else
          {
             throw new Exception("Exception");
          }
          ball.InitVelocity( angleSpawn );
          countBalls++;
        }
        OnReproductionBall?.Invoke();
     }


     public Ball SpawnBall()
     {
         countBalls++;
         return pool.Get();
     }

     public void DestroyBall( Ball ball )
     {
        pool.Return(ball);
        countBalls--;
        if( countBalls <= 0 )
        {
            OnLossAllBalls?.Invoke();
        }
     }


}

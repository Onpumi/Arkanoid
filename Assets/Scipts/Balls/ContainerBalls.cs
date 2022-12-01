using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Burst;

[BurstCompile]
public class ContainerBalls : MonoBehaviour
{
    private Pool<Ball> _pool;
    private IPoolFactory<Ball> _factory;
    [SerializeField] private Ball _ball;
    [SerializeField] private BallMover _ballMover;
    [SerializeField] private Board _board;
    private const int MaxCountBalls = 400;
    private List<Ball> _activeBalls;
    public event Action OnLossAllBalls;
    public event Action OnUpdateCountShowBall;
    public event Action OnMoveBall;
    public int CountBalls { get; private set; } = 1;
    private IEnumerator _coroutineSpawn;

    private void Awake()
    {
        _ball.transform.gameObject.SetActive(false);
    }

    public void SpawnBallsToPool()
    {
       _factory ??= new PrefabFactory<Ball>(_ball, transform, "ball");
       _pool ??= new Pool<Ball>(_factory,500);
       _activeBalls ??= new List<Ball>();
    }
    
    private void OnEnable()
    {
      SpawnBallsToPool();
      CountBalls++;
     _ball = _pool.Get();
      _activeBalls.Add( _ball );
      _board.OnReproductionOne += SpawnAllBalls;
    }

    private void OnDisable()
    {
        CountBalls--;
        _activeBalls.Remove( _ball );
        _pool.Return(_ball);
        _board.OnReproductionOne -= SpawnAllBalls;
    }



     public void SpawnAllBalls()
     {
         _coroutineSpawn = SpawnBalls();
         StartCoroutine( _coroutineSpawn );
     }

     public void ResetBalls()
     {
        CountBalls = 0;
       _activeBalls.Clear();
     }

     private IEnumerator SpawnBalls( )
     {
        Vector2 direction1;
        Vector2 direction2;
        var countMaxSpawnInFrame = 5;
        var countActiveBalls = _activeBalls.Count;
        var countSpawn = 0;
        var spawnNewBalls = new HashSet<Ball>();
        
         foreach( Ball ball in _activeBalls )
         {
             if (CountBalls >= MaxCountBalls - 1) break;

            Ball ball1 = _pool.Get();
            direction1 = Quaternion.Euler(0,0,45) * ball.BallMover.Direction;
            ball1.transform.position = ball.transform.position;
            ball1.BallMover.StartMove( direction1 );
            spawnNewBalls.Add(ball1);
            CountBalls++;
            countSpawn++;

            Ball ball2 = _pool.Get();
            direction2 = Quaternion.Euler(0,0,-45) * ball.BallMover.Direction;
            ball2.transform.position = ball.transform.position;
            ball2.BallMover.StartMove( direction2 );
            spawnNewBalls.Add(ball2);
            CountBalls++;
            countSpawn++;


            if(countSpawn < countMaxSpawnInFrame ) continue;
            yield return new WaitForFixedUpdate();
            countSpawn = 0;
        }
        _activeBalls = _activeBalls.Concat(spawnNewBalls).ToList();
        OnUpdateCountShowBall?.Invoke();
     }

    
     public Ball SpawnBall()
     {
         CountBalls++;
         var ball = _pool.Get();
         _activeBalls.Add(ball);
         return ball;
     }

     public Ball GetFirstBall()
     {
         return _ball;
     }

     public void DespawnBall( Ball ball )
     {
         CountBalls--;
         _activeBalls.Remove(ball);
         _pool.Return( ball );
     }

     public void DestroyBall( Ball ball )
     {
        _pool.Return(ball);
        CountBalls--;
        if( CountBalls <= 0 )
        {
            OnLossAllBalls?.Invoke();
        }
     }

     public void ReturnPoolAllBalls()
     {
         foreach( Ball activeBall in _activeBalls )
         {
           _pool.Return(activeBall);
         }
     }

     
/*
     private void FixedUpdate()
     {
        OnMoveBall?.Invoke();
     }
*/

}

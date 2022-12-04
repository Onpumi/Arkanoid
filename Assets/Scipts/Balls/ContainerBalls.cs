using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ContainerBalls : MonoBehaviour
{
    private Pool<Ball> _pool;
    private IPoolFactory<Ball> _factory;
    [SerializeField] private Ball _ball;
    [SerializeField] private BallMover _ballMover;
    [SerializeField] private Board _board;
    [SerializeField] private SoundsPlayer _soundsPlayer;
    private const int MaxCountBalls = 400;
    private List<Ball> _activeBalls;
    public event Action OnLossAllBalls;
    public event Action OnUpdateCountShowBall;
    public event Action OnMoveBall;
    public int CountBalls { get; private set; } = 1;
    private IEnumerator _coroutineSpawn;
    private SpawnerBall _spawnerBall;
    

    private void Awake()
    {
        _ball.transform.gameObject.SetActive(false);
    }

    private void SpawnBallsToPool()
    {
       _factory ??= new PrefabFactory<Ball>(_ball, transform, "ball");
       _pool ??= new Pool<Ball>(_factory,550);
       _activeBalls ??= new List<Ball>();
       _spawnerBall ??= new SpawnerBall();
    }
    
    private void OnEnable()
    {
      SpawnBallsToPool();
      CountBalls++;
      _ball = _pool.Get();
      _activeBalls.Add( _ball );
    }

    private void OnDisable()
    {
        //CountBalls--;
      // _activeBalls.Remove( _ball );
         ReturnPoolAllBalls();
         ResetBalls(); 
        _pool.Return(_ball);
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
            int[] angles = { -45, 45 };
             var (ball1, ball2) = _spawnerBall.SpawnTwoBall( ball, _pool, angles);
             spawnNewBalls.Add(ball1);
             spawnNewBalls.Add(ball2);
             CountBalls+=2;
             countSpawn+=2;

            if(countSpawn < countMaxSpawnInFrame ) continue;
            yield return new WaitForFixedUpdate();
            countSpawn = 0;
        }
        _activeBalls = _activeBalls.Concat(spawnNewBalls).ToList();
        OnUpdateCountShowBall?.Invoke();
        _soundsPlayer.PlayGetBonus();
     }

     public void SpawnThreeBalls()
     {
         int[] angles = { -30, 0, 30 };
         var spawnNewBalls = new HashSet<Ball>();
         var (ball1, ball2, ball3) = _spawnerBall.SpawnThreeBall( _board.transform, _pool, angles );
         CountBalls += 3;
         spawnNewBalls.Add(ball1);
         spawnNewBalls.Add(ball2);
         spawnNewBalls.Add(ball3);
         _activeBalls = _activeBalls.Concat(spawnNewBalls).ToList();
         OnUpdateCountShowBall?.Invoke();
         _soundsPlayer.PlayGetBonus();
     }

     public List<Ball> GetAllBalls() => _activeBalls;
     
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

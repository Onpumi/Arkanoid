using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
//using UnityEngine.Pool;

public class FactoryBalls : MonoBehaviour
{
    private Pool<Ball> _pool;
    private IPoolFactory<Ball> _factory;
    [SerializeField] private Ball _ball;
    [SerializeField] private BallMover _ballMover;
    [SerializeField] private BallContainer _ballContainer;
    [SerializeField] private Board _board;
    private const int _maxCountBalls = 1500;
    private List<Ball> _activeBalls;
    public event Action OnLossAllBalls;
    public event Action OnUpdateCountShowBall;
    private int _countBalls = 1;
    public int CountBalls => _countBalls;

    private IEnumerator _coroutineSpawn;

    private void Awake()
    {
        _factory = new PrefabFactory<Ball>(_ball, transform, "ball");
        _pool = new Pool<Ball>(_factory,5000);
        _activeBalls = new List<Ball>();
        _activeBalls.Add(_ball);
    }

    private void OnEnable()
    {
      _board.OnReproductionOne += SpawnAllBalls;
    }

    private void OnDisable()
    {
      _board.OnReproductionOne -= SpawnAllBalls;
    }


     private async void SpawnAsyncBalls( Ball ball)
     {

         await Task.Run( () =>
         {
           if( _countBalls >= _maxCountBalls - 1 ) 
           {
            return;
           }
         }
         );

     }

     public void SpawnAllBalls()
     {
        
         _coroutineSpawn = SpawnBalls();
         StartCoroutine( _coroutineSpawn );
     }

     private IEnumerator SpawnBalls( )
     {
        Vector2 direction1;
        Vector2 direction2;
        var countMaxSpawn = 5;
        var countActiveBalls = _activeBalls.Count;
        var countSpawn = 0;
        var spawnNewBalls = new List<Ball>();

         foreach( Ball ball in _activeBalls )
        {
            if( _countBalls >= _maxCountBalls - 1 ) 
          {
            break;
          }

            Ball ball1 = _pool.Get();
            direction1 = Quaternion.Euler(0,0,45) * ball.BallMover.Direction;
            ball1.transform.position = ball.transform.position;
            ball1.BallMover.StartMove( direction1 );
            spawnNewBalls.Add(ball1);
            _countBalls++;
            countSpawn++;

            Ball ball2 = _pool.Get();
            direction2 = Quaternion.Euler(0,0,-45) * ball.BallMover.Direction;
            ball2.transform.position = ball.transform.position;
            ball2.BallMover.StartMove( direction2 );
            spawnNewBalls.Add(ball2);
            _countBalls++;
            countSpawn++;


            if(countSpawn < countMaxSpawn ) continue;
            yield return new WaitForFixedUpdate();
            countSpawn = 0;
        }
        _activeBalls = _activeBalls.Concat(spawnNewBalls).ToList();
        OnUpdateCountShowBall?.Invoke();
     }




    
     public Ball SpawnBall()
     {
         _countBalls++;
         return _pool.Get();
     }

     public void DestroyBall( Ball ball )
     {
        _pool.Return(ball);
        _countBalls--;
        if( _countBalls <= 0 )
        {
            OnLossAllBalls?.Invoke();
        }
     }

}
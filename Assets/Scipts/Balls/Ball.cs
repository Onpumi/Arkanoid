using UnityEngine;
using Unity.Burst;
using System.Diagnostics;

[RequireComponent(typeof(BallMover))]
[RequireComponent(typeof(BallSpawner))]

[BurstCompile]
public class Ball : MonoBehaviour, IPoolable<Ball>
{

  [SerializeField] private FactoryBalls _factoryBalls;
  [SerializeField] private BallMover _ballMover;
  [SerializeField] private BallSpawner _spawnerBall;
  [SerializeField] private Board _board;
  public BallMover BallMover => _ballMover;
  Stopwatch st;
  private void OnEnable()
  {
  //  _board.OnReproductionOne += ActivateBonus;
     st = new Stopwatch();
  }

    private void OnDisable()
  {
 //  _board.OnReproductionOne -= ActivateBonus;
  }

    public void SpawnFrom( IPool<Ball> pool )
  {
    transform.gameObject.SetActive(true);
  }

    public void Despawn()
  {
    transform.gameObject.SetActive(false);
  }

   private void ActivateBonus()
   {
      //st.Start();
      //st.Stop();
      //float ms = st.ElapsedMilliseconds;
      //UnityEngine.Debug.Log(st.ElapsedMilliseconds);
      //UnityEngine.Debug.Log(string.Format("finished: {0:0.000000000000000000000000000000000000000000000000000}ms total",  ms));
   }


 }

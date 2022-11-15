using UnityEngine;
using Unity.Burst;

[RequireComponent(typeof(BallMover))]

[BurstCompile]
public class Ball : MonoBehaviour, IPoolable<Ball>
{
  [SerializeField] private FactoryBalls _factoryBalls;
  [SerializeField] private BallMover _ballMover;
  [SerializeField] private Board _board;
  public BallMover BallMover => _ballMover;

    public void SpawnFrom( IPool<Ball> pool )
  {
    transform.gameObject.SetActive(true);
  }

    public void Despawn()
  {
    transform.gameObject.SetActive(false);
  }


 }

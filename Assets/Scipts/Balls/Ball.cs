using UnityEngine;
using Unity.Burst;
using UnityEngine.Serialization;

[RequireComponent(typeof(BallMover))]
[BurstCompile]
public class Ball : MonoBehaviour, IPoolable<Ball>
{
  [FormerlySerializedAs("_factoryBalls")] [SerializeField] private ContainerBalls _containerBalls;
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

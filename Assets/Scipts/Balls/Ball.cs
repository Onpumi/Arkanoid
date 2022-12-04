using UnityEngine;

[RequireComponent(typeof(BallMover))]
public class Ball : MonoBehaviour, IPoolable<Ball>
{
  [SerializeField] private ContainerBalls _containerBalls;
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

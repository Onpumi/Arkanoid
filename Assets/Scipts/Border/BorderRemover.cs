using UnityEngine;
using UnityEngine.Serialization;

public class BorderRemover : MonoBehaviour 

{ 
    [FormerlySerializedAs("_factoryBalls")] [SerializeField] ContainerBalls _containerBalls;
    private void OnCollisionEnter2D( Collision2D collision )
   {
      if( collision.collider.TryGetComponent(out Ball ball) )
      {
       // _factoryBalls.DestroyBall(ball);
      }
   }

}

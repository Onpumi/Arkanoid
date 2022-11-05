using UnityEngine;

public class BorderRemover : MonoBehaviour 

{ 
    [SerializeField] FactoryBalls _factoryBalls;
    private void OnCollisionEnter2D( Collision2D collision )
   {
      if( collision.collider.TryGetComponent(out Ball ball) )
      {
       // _factoryBalls.DestroyBall(ball);
      }
   }

}

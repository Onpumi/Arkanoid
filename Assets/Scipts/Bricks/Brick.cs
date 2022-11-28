using UnityEngine;

public class Brick : MonoBehaviour
{
   private BonusBall _bonusBall = null;
   private Bricks _bricks;
   public bool IsNull { get => (_bonusBall == null) ? (true) : (false); }
   public BonusBall BonusBall => _bonusBall;

   int count = 0;

   private void OnEnable()
   {
      _bricks = transform.parent.parent.parent.parent.GetComponent<Bricks>();
   }


   private void OnCollisionEnter2D( Collision2D collision )
   {

      //   if( collision.contactCount > 1 ) { Debug.Log(count++); }

      //if( collision.collider.TryGetComponent(out BallMover ballMover) )
      //{
            
         //ballMover.UpdateDirection( collision.contacts[0].normal );
         //transform.gameObject.SetActive(false);
      //}
   }

  

   public void Despawn()
   {
      transform.gameObject.SetActive(false);
   }

     public void SpawnBonus( BonusBall prefabBonus )
   {
      _bonusBall = Instantiate( prefabBonus, transform.position, Quaternion.identity, transform.parent );
      _bonusBall.transform.gameObject.SetActive(false);
   }

   public BonusBall GetBonus() => _bonusBall;

   private void OnDisable()
   {
      OpenBrick();
      _bonusBall = null;
   }


   public void OpenBrick()
   {
          if( _bricks ) 
          {
           _bricks.UpdateBricks();
          }

          if( _bonusBall != null )
          {
            _bonusBall.transform.gameObject.SetActive(true);
            _bonusBall.OpenBonus();
          }
   }


  public void SetColor( Color color )
  {
    transform.GetComponent<SpriteRenderer>().color = color;
  }


}

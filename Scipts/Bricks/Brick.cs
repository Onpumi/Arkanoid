using UnityEngine;

public class Brick : MonoBehaviour
{
   private BonusBall _bonusBall = null;
   private Bricks _bricks;
   public bool IsNull { get => (_bonusBall == null) ? (true) : (false); }
   public BonusBall BonusBall => _bonusBall;

   private void Awake()
   {
      _bricks = transform.parent.GetComponent<Bricks>();
   }

   private void OnCollisionEnter2D( Collision2D collision )
   {

      if( collision.collider.TryGetComponent(out Ball ball) )
      {
         this.transform.gameObject.SetActive(false);
      }
   }

     public void InitBonus( BonusBall prefabBonus )
   {
      _bonusBall = Instantiate( prefabBonus, transform.position, Quaternion.identity, transform.parent );
      _bonusBall.transform.localScale = transform.localScale;
      _bonusBall.transform.gameObject.SetActive(false);
   }

   public BonusBall GetBonus() => _bonusBall;

   private void OnDisable()
   {
      OpenBrick();
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

}

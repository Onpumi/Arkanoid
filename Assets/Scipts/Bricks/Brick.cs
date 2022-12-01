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
//      _bricks = transform.parent.parent.parent.parent.GetComponent<Bricks>();
       _bricks = transform.parent.parent.GetComponent<Bricks>();
   }

   public void DeSpawn()
   {
       if( transform != null )
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

   public void DisableBonus()  
   {
       _bonusBall.DisableBonus();
   }

   public void SetColor( Color color )
  {
    transform.GetComponent<SpriteRenderer>().color = color;
  }


}

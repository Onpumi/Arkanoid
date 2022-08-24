using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Brick : MonoBehaviour
{
   private BonusBall _bonusBall = null;
   public bool IsNull { get => (_bonusBall == null) ? (true) : (false); }
   public BonusBall BonusBall => _bonusBall;


   public void TakeBonus( BonusBall prefabBonus, Transform _parentBalls, Board board )
   {
      _bonusBall = Instantiate( prefabBonus, transform.position, Quaternion.identity, transform.parent );
      _bonusBall.transform.localScale = transform.localScale;
      _bonusBall.transform.gameObject.SetActive(false);
   }

     public BonusBall GetBonus()
   {
      return _bonusBall;
   }

   private void OnDestroy()
   {
         if( _bonusBall != null )
         {
            _bonusBall.transform.gameObject.SetActive(true);
            _bonusBall.OpenBonus();
         }
   }

}

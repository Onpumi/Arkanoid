using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{

    [SerializeField] private BallMover _ballmover;
    [SerializeField] private Ball _ball;
    [SerializeField] private Color _color;
   

  private void Awake()
  {
    // transform.gameObject.SetActive(false);

    foreach( Transform child in transform )
    {
       // child.GetComponent<SpriteRenderer>().color = Color.green;
         
    }

  }

  

   public void Update()
   {

       if(Input.GetMouseButtonDown(0))
       {
            if( _ball.IsMove == false )
          {
           _ball.StartMove();
          }

         var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         var target = position - _ball.transform.position;
         _ballmover.SetDirection(target);
       }


   }


}

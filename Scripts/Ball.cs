using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Ball : MonoBehaviour
{
    [SerializeField] private InputBoard _inputBoard;
    [SerializeField] private Board _board;
    [SerializeField] private Transform _parentBlocks;
    [SerializeField] private LayerMask _maskaRaycast;
    [SerializeField] private GridBricks _gridBriks;
    public bool IsMove { get; private set; }
    public Vector2 Target { get; private set; }
    public event Action<Vector2,int> OnHit;
    public event Action OnHitPlay;
    public event Action<GameObject> OnDestroyBlock;
    public event Action OnFreeze;
    private Vector3 pointHit;
    private Vector3 _prevPosition;

   private void OnEnable()
   {
      _inputBoard.OnPress += StartMove;
   }

   private void OnDisable()
   {
      _inputBoard.OnPress -= StartMove;
   }

   private void OnCollisionEnter2D( Collision2D collision )
   {
        if( IsMove )
        {
            int angleBoard = 0;

            if( _board.transform == collision.transform )
            {
              Vector3 nearPointPositionBoard = collision.collider.ClosestPoint(transform.position);
              Vector3 ballLocalPosition = _board.transform.InverseTransformPoint( nearPointPositionBoard );
              angleBoard = _board.GetAngleReflect(ballLocalPosition.x);
            }

             OnHit?.Invoke(collision.contacts[0].normal,angleBoard);
             OnHitPlay?.Invoke();

            if( _parentBlocks == collision.gameObject.transform )
            {
                _gridBriks.FindCellToDel( collision );
            }
        }
   }
 
   private void OnCollisionStay2D( Collision2D collision )
   {

            if( _parentBlocks == collision.gameObject.transform )
            {
                 OnHit?.Invoke(collision.contacts[0].normal,3);
                _gridBriks.FindCellToDel( collision );
            }
   }

   private void StartMove()
   {
       IsMove = true;
   }

   private bool checkFreezeBall()
   {
      if ( Mathf.Abs(_prevPosition.x-transform.position.x) < 0.0001f && Mathf.Abs(_prevPosition.y-transform.position.y) < 0.0001f )
      {
         return true;
      }
       _prevPosition = transform.position; 
       return false;
   }

   private void Update()
   {

      if( checkFreezeBall() )
      {
        OnFreeze?.Invoke();
      }
    //Debug.DrawLine( (Vector3)Position,((Vector3)Position - (Vector3)Normal), Color.green);
    //Debug.DrawRay( (Vector3)Position, - (Vector3)Normal, Color.green);
   }


  

  
}

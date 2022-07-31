using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _size;
    [SerializeField] private Color _color;
    [SerializeField] private InputBoard _inputBoard;
    [SerializeField] private Board _board;
    [SerializeField] private Transform _parentblocks;
    [SerializeField] private LayerMask _maskaRaycast;
    public bool IsMove { get; private set; }
    public event Action<Vector2,int> OnHit;
    public event Action OnHitPlay;
    public event Action<GameObject> OnDestroyBlock;
    public Vector2 Direction;
    private float _timeDelta = Time.time;


   private void OnEnable()
   {
      _inputBoard.OnMove += Move;
      _inputBoard.OnPress += StartMove;
   }

   private void OnDisable()
   {
      _inputBoard.OnMove -= Move;
      _inputBoard.OnPress -= StartMove;
   }

   private void OnCollisionEnter2D( Collision2D collision )
   {

        if( Time.time-_timeDelta < 0.1f && _parentblocks != collision.gameObject.transform.parent )
        {
          return;
        }
         
        if( IsMove )
        {

            int angleBoard = 0;

            if( _board.transform == collision.transform )
            {
              Vector3 nearPointPositionBoard = collision.collider.ClosestPoint(transform.position);
              Vector3 ballLocalPosition = _board.transform.InverseTransformPoint( nearPointPositionBoard );
              angleBoard = _board.GetAngleReflect(ballLocalPosition.x);
            }

            var point = collision.contacts[0].point;
             Vector2 normal = Vector2.zero;
             for( int i = 0 ; i < collision.contactCount; i++ )
             {
               normal += collision.contacts[i].normal;
             }

               normal /= collision.contactCount;
        

              OnHit?.Invoke(normal,angleBoard);

              if( _parentblocks == collision.gameObject.transform.parent )
             {
                //RaycastHit2D hitDown; 
               //hitDown = Physics2D.Raycast( transform.position, Direction, 10, _maskaRaycast );
               //normal = hitDown.normal;
             }

              OnHitPlay?.Invoke();
              
             if( _parentblocks == collision.gameObject.transform.parent )
             {
                OnDestroyBlock.Invoke(collision.gameObject);
             }
        }

        _timeDelta = Time.time;
   }

   private void Move()
   {
        if( IsMove == false )
        {
            var xCoordinateBoard = _inputBoard.TouchPosition.x;
            transform.position = new Vector3( xCoordinateBoard, transform.position.y, transform.position.y );
        }
   }

   private void StartMove()
   {
     IsMove = true;
   }
  
}

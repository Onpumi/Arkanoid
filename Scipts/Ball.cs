using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Ball : MonoBehaviour, IPoolable<Ball>
{
    [SerializeField] private InputBoard _inputBoard;
    [SerializeField] private Board _board;
    [SerializeField] private Transform _parentBlocks;
    [SerializeField] private LayerMask _maskaRaycast;
    [SerializeField] private Vector2 _startPosition;
    public bool IsMove { get; private set; }
    public Vector2 Target { get; private set; }
    public event Action<Collision2D,int> OnHit;
    public event Action OnHitPlay;
    private Vector3 pointHit;
    private Vector3 _prevPosition;
    public float StartAngle {get; private set; }
    public ContactPoint2D[] ContactsPoint {get; private set; }
    private float deltaTime;
    private float prevDeltaTime;
    public bool IsCollisionEnter {get; set;}




      public void RandomStartVector( float delta )
    {
        StartAngle = delta;
    }



   private void Awake()
   {
     //transform.position = new Vector3( _startPosition.x, _startPosition.y, transform.position.z );

     transform.position = new Vector3( _board.transform.position.x, _board.transform.position.y + _board.transform.localScale.y, transform.position.z );
     ContactsPoint = null;
     IsCollisionEnter = false;
   }
   private void OnEnable()
   {
      _inputBoard.OnPress += StartMove;
   }

   private void OnDisable()
   {
      _inputBoard.OnPress -= StartMove;
   }

  public void SpawnFrom( IPool<Ball> pool )
  {
    transform.gameObject.SetActive(true);
  }

   public void Despawn()
  {
    transform.gameObject.SetActive(false);
  }


   private void OnCollisionEnter2D( Collision2D collision )
   {
      ContactsPoint = collision.contacts;
    deltaTime = Time.time-prevDeltaTime;
     IsCollisionEnter = true;
        if( IsMove )
        {
 
            int angleBoard = 0;
            Vector2 sumNormal = Vector2.zero;
            if( _board.transform == collision.transform )
            {
              Vector3 nearPointPositionBoard = collision.collider.ClosestPoint(transform.position);
              Vector3 ballLocalPosition = _board.transform.InverseTransformPoint( nearPointPositionBoard );
              angleBoard = _board.GetAngleReflect(ballLocalPosition.x);
            }

             

            if( _parentBlocks == collision.gameObject.transform )
            {
             // OnHit?.Invoke(collision,angleBoard);
             //   _gridBriks.FindCellToDel( collision );
            }
        }
        prevDeltaTime = Time.time;
   }

 public void Stop()
 {
   IsMove = false;
 }


   public void StartMove()
   {
       IsMove = true;
   }
 
}

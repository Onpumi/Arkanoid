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
    [SerializeField] private GridBricks _gridBriks;
    [SerializeField] private Vector2 _startPosition;
    public bool IsMove { get; private set; }
    public Vector2 Target { get; private set; }
    public event Action<Collision2D,int> OnHit;
    public event Action<Vector2> OnBounc;
    public event Action OnHitPlay;
    public event Action<GameObject> OnDestroyBlock;
    public event Action OnFreeze;
    private Vector3 pointHit;
    private Vector3 _prevPosition;
    public float StartAngle {get; private set; }
    public ContactPoint2D[] ContactsPoint {get; private set; }
    private float deltaTime;
    private float prevDeltaTime;
    private float maxDeltaTime=0;
    public bool IsCollisionEnter {get; set;}

    public float timeEnter=0;
    public float timeExit=0;
    public float timeStay=0;



      public void RandomStartVector( float delta )
    {
        StartAngle = delta;
    }



   private void Awake()
   {
     transform.position = new Vector3( _startPosition.x, _startPosition.y, transform.position.z );
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
      timeEnter = Time.time;
    deltaTime = Time.time-prevDeltaTime;
    if( maxDeltaTime < deltaTime )
    {
      maxDeltaTime = deltaTime;
    }
     IsCollisionEnter = true;
   // Debug.Log( $"maxDeltaTime={maxDeltaTime} deltaTime={Time.deltaTime}");


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

             OnHit?.Invoke(collision,angleBoard);

            if( _parentBlocks == collision.gameObject.transform )
            {
                _gridBriks.FindCellToDel( collision );
            }
        }
        prevDeltaTime = Time.time;
   }

 public void Stop()
 {
   IsMove = false;
 }




   private void OnCollisionExit2D( Collision2D collision )
   {
      timeExit=Time.time;
      timeStay=timeExit-timeEnter;
      IsCollisionEnter = false;
      ContactsPoint = null;

//      Debug.Log(timeStay);

   }
 
 

   public void StartMove()
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
      
      //Debug.Log(timeStay);
      

      //if( checkFreezeBall() )
      //{
        //OnFreeze?.Invoke();
      //}
    //Debug.DrawLine( (Vector3)Position,((Vector3)Position - (Vector3)Normal), Color.green);
    //Debug.DrawRay( (Vector3)Position, - (Vector3)Normal, Color.green);
   }
  
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMover : MonoBehaviour
{
    [SerializeField] private Transform test;
    [SerializeField] private float _speed;
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _parentBalls;
    [SerializeField] private Board _board;
    [SerializeField] private float _limitAngle;
    [SerializeField] private LayerMask _maskaRaycast;
    [SerializeField] private GridBricks _gridBriks;
    private Vector2 _direction = new Vector2(1f,1f);
    private Rigidbody2D _rigidbody;
    private int countBalls;
    private Vector2 _prevDirection;
    private Vector2 prevPosition;
    public Vector2 Direction => _direction;
    public event Action OnFreezeDirection;
    private float _startTimeFreezeDirection;
    private float _endTimeFreezeDirection;
    private float _freezeTime;
    private Vector2 Normal;
    private Vector2 Contact;
    private bool _isHit = false;
    [SerializeField] private Transform Test;
    private List<RaycastHit2D> hits = new List<RaycastHit2D>();
    private RaycastHit2D hit;
    private Vector2 pointCollision;
    Vector2 targetBall; 
    float enterTime;
    float repeatTime;   
    float stayTime;
   

      public float Speed 
    {
        get => _speed;
        private set {}
    }

 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _direction = GetMoveDirection( _direction, -5f * Mathf.PI / 180f );
    }

    private void Start()
    {
        _direction = GetMoveDirection( _direction, -_ball.StartAngle * Mathf.PI / 180f );
    }

    private void OnEnable()
    {
        _ball.OnHit += HitBlock;
        _ball.OnFreeze += ResetDirection;

    }

    private void OnDisable()
    {
      _ball.OnHit -= HitBlock;
      _ball.OnFreeze -= ResetDirection;
    }

      private void HitBlock( Collision2D collision, int angleBoard )
    {
        Vector2 normal = Vector2.zero;
        Vector2 closestPoint = collision.contacts[0].point;
        var collider = collision.collider;
        var distance = 10f;

         if(collision.contacts.Length > 0 )
         {
            Contact = collision.contacts[0].point;
         }
         else
         {
            Debug.Log($"нет контакта для {_ball.name}");
         }

        if(collision.contacts.Length > 1 )
        {
         Debug.Log(collision.contacts.Length);
         //_direction = -_direction;
        }


             foreach( var contact in collision.contacts)
             {
              //  normal += contact.normal.normalized;
                var delta = _rigidbody.position - collider.ClosestPoint(_rigidbody.position);
                if( delta.magnitude < distance )
                {
                    distance = delta.magnitude;
                    normal = contact.normal.normalized;
                    closestPoint = collider.ClosestPoint(_rigidbody.position);
                }
             }


             //normal /= collision.contacts.Length;
             //normal.Normalize();


        
       // normal.x = Mathf.Round(normal.x);
        //normal.y = Mathf.Round(normal.y);

   


       //  _direction = Vector2.Reflect( _direction.normalized, normal );


        Normal = normal;



      _prevDirection = _direction;
    }
int K = 0;



private Vector2 SetNormal( ContactPoint2D[] points )
{

   Normal = Vector2.zero;

   foreach( var point in points )
   {
      Normal += point.normal;
   }
    Normal /= points.Length;
    Normal.Normalize();
  return Normal;
}




  private void OnCollisionEnter2D( Collision2D collision )
  {
    
   
    repeatTime = Time.time-enterTime;
    enterTime = Time.time;

   
    if(repeatTime > 0.02 )
    {
      //Debug.Log($"{_ball.name}");
      //_direction = Vector2.zero;
    }
    

      if(collision.contacts.Length > 1 )
        {
         _direction = -_direction;
        }

    
    if(_isHit == false)
      ReflectBall();
     _isHit = true;
    
  }

  private void OnCollisionStay2D( Collision2D collision )
  {

    

      stayTime = Time.time-enterTime;

      if(stayTime > 0.1)
      {
        //Debug.Log($"{_ball.name}");
       // _direction -= _direction;
      }


         
     if(repeatTime > 0.02 )
     {
     //   ReflectBall();
     }



  }


  private void OnCollisionExit2D( Collision2D collision )
  {
    _isHit = false;
    stayTime = Time.time-enterTime;
//    Debug.Log(stayTime);
  }


    private void TestMoveBall()
    {
      if( targetBall.magnitude < 0.5 * transform.localScale.x )
      {
       // Debug.Log($"Застрял {transform.name}");
       //_direction = -_direction;
       //_direction = Vector2.Reflect( _direction.normalized, Normal.normalized);
      }
    }



    private void ReflectBall()
    {
        Vector2 normal;
        hit = Physics2D.CircleCast(transform.position, 0.6f*transform.localScale.x,_direction, 5f * transform.localScale.x, _maskaRaycast );
        pointCollision = hit.point;
        _isHit = true;
        //test.transform.position = hit.point;
        //test.transform.localScale = new Vector3( 0.04f, 0.04f, 0.04f);
//        Debug.Log($"hit{K++}");
        targetBall = pointCollision - (Vector2)transform.position;
        
          if( hit.collider == null ) 
          {
            return;
          }
          normal = (Vector2)transform.position - hit.collider.ClosestPoint(transform.position);

          
          Normal = normal * 25;

         _direction = Vector2.Reflect( _direction.normalized, normal.normalized);

         

         if( Vector3.Angle(_direction.normalized,normal.normalized) > 90 )
         {
         Debug.Log(Vector3.Angle(_direction.normalized,normal.normalized)); 
          _direction -= _direction;
         }
if( Vector3.Angle(_direction.normalized,normal.normalized) < 0 )
         Debug.Log(Vector3.Angle(_direction.normalized,normal.normalized)); 
       //  _isHit = false;
       //  _rigidbody.velocity = Vector2.zero;
    }



    private Vector2 GetHeatingVector( Vector2 normal )
    {
       Vector2 VectorHeating = _direction - 2 * Vector2.Dot(_direction,normal) * normal;
       return (Vector2)VectorHeating;
    }

      public Vector2 GetMoveDirection( Vector2 vectorMove, float delta )
    {
       return new Vector2( vectorMove.x * Mathf.Cos(delta) - vectorMove.y * Mathf.Sin(delta), vectorMove.x * Mathf.Sin(delta) + vectorMove.y * Mathf.Cos(delta) ) ;
    }

     public void SetMoveDirection( float delta )
    {
       _direction = new Vector2( _direction.x * Mathf.Cos(delta) - _direction.y * Mathf.Sin(delta), _direction.x * Mathf.Sin(delta) + _direction.y * Mathf.Cos(delta) ) ;
    }




    private void Update()
    {
      if( _ball.IsMove )
      {
        //if(targetBall.magnitude > 0.5 * transform.localScale.x)
        //_rigidbody.velocity = _direction.normalized * Time.fixedDeltaTime * _speed;  
      }
        
        if( Input.GetMouseButtonDown(1))
        {
            _direction = (Vector2)Contact-(Vector2)_ball.transform.position;
            _speed = 5;
        }

    }

    private void FixedUpdate()
    {   

        if( _ball.IsMove )
        {
            //_rigidbody.velocity = _direction.normalized * Time.deltaTime * GetSpeed() * 50;
            //_rigidbody.velocity = _direction.normalized * Time.deltaTime * 30;

           // Debug.Log(_direction.normalized * Time.deltaTime );
            //var target = _direction.normalized * Time.deltaTime * 0.5f;
           //_rigidbody.MovePosition(_rigidbody.position + target);

        _rigidbody.velocity = _direction.normalized * Time.fixedDeltaTime * _speed;  

        //Debug.Log(_rigidbody.velocity.magnitude);
           // _rigidbody.velocity = _direction.normalized * Time.deltaTime * 70;


        }
    
      // ReflectBall();
        

        Debug.DrawLine(_ball.transform.position, _ball.transform.position + (Vector3)Normal.normalized, Color.green );
        Debug.DrawLine(_ball.transform.position, _ball.transform.position + (Vector3)_direction.normalized, Color.red );
       // Debug.DrawLine(_ball.transform.position, (Vector3)targetBall, Color.magenta );
        
        var delta = _rigidbody.position - prevPosition;
        if( _rigidbody.velocity.magnitude < 0.01f && _ball.IsMove )
       { 
          //_direction -= _direction;
       }

        //if( _rigidbody.velocity.magnitude == 0 )
        //{
//            Debug.Log(_ball.IsCollisionEnter);
        //}

        prevPosition = _rigidbody.position;
    }



    public void SetDirection( Vector2 direction )
    {
        _direction = direction;
    }


    public void ResetDirection()
    {
      //  _direction=-_direction;
    }

    private float GetSpeed()
    {
        var speed = _direction.magnitude;
        return Mathf.Max(speed, Speed);
    }
}

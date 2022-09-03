using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IPoolable<Ball>
{
  [SerializeField] RayBall _rayBall;
  [SerializeField] private float _speed;
  [SerializeField] private Transform _parentRemoveBricks;
  [SerializeField] private FabrikaBalls _factoryBalls;
  [SerializeField] private Transform _destroyerBorder;
  [SerializeField] private LayerMask _maskaRaycast;
  [SerializeField] private Board _board;
  [SerializeField] private Vector3 _startPosition;
  private Vector2 _velocity;
  private Vector2 _prevDirection;
  public bool IsMove  { get; private set; }
  public float StartAngle { get; private set; }
  private Rigidbody2D _rigidbody;
  public float Radius;
  public Vector2 ReflectDirection;
  public float AnglelNormalReflect;
  private float _timeDelay = 5f;
  private float _timePrev;
  private float _sizeBall;
  private float _radiusBall;
  private float _prevTime = 0;
  public Vector3 StartPosition => _startPosition;

   private void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
    InvokeRepeating("TestCollinearity", 0, _timeDelay);
    _radiusBall = GetComponent<CircleCollider2D>().radius;
    _sizeBall = _radiusBall * transform.localScale.x;
    transform.position = _startPosition;
  }

  public void StateStart()
  {
     IsMove = false;
     _velocity = Vector3.zero;
     _rigidbody.velocity = _velocity;
     transform.position = _startPosition;
  }

   private void TestCollinearity()
   {
      if( IsMove )
      {
        if( Vector3.Angle(_velocity, _prevDirection) == 0 || Vector3.Angle(_velocity, -_prevDirection) == 0)
        {
           _velocity = (Vector2)(Quaternion.Euler( 0f, 0f, 5f) * (Vector3)_velocity);
        }
      }
        _prevDirection = _velocity;
   }

   public void InitVelocity( float angle )
  {
      StartAngle = angle;
  }

   public void SpawnFrom( IPool<Ball> pool )
  {
    transform.gameObject.SetActive(true);
  }

   public void Despawn()
  {
    transform.position = StartPosition;
    transform.gameObject.SetActive(false);
  }

  private void OnEnable()
  {
    _rayBall.OnStartDirection += StartMove; 
    _factoryBalls.OnReproductionBall += StartMove; 
    _board.OnReproductionOne += ActivateBonus;
  }

  private void OnDisable()
  {
    _rayBall.OnStartDirection -= StartMove; 
    _factoryBalls.OnReproductionBall -= StartMove; 
    _board.OnReproductionOne -= ActivateBonus;
  }
  private void StartMove( Vector3 startDirection )
  {
     if( _velocity == Vector2.zero )
     {
        _velocity = startDirection * _speed * Time.fixedDeltaTime;
       RotateDirection( StartAngle );
     }
     IsMove = true;
  }

  private void RotateDirection( float angle )
  {
     _velocity = Quaternion.Euler(0,0,angle) * _velocity;
  }

  private void OnCollisionEnter2D( Collision2D collision )
  {

    Vector2 normal = collision.contacts[0].normal;
    ReflectDirection = Vector3.Reflect(_velocity.normalized, normal.normalized);
    AnglelNormalReflect = Vector3.Angle(ReflectDirection.normalized, collision.contacts[0].normal.normalized );
    _velocity = ReflectDirection;
    
    if( _parentRemoveBricks && collision.collider.transform.parent == _parentRemoveBricks  ) 
    {
       if( _prevTime != 0 && Time.time-_prevTime >= 0.02 )
       {
         Destroy(collision.collider.transform.gameObject);
         _prevTime = Time.time;
       }
       else if( _prevTime == 0 )
       {
         Destroy(collision.collider.transform.gameObject);
         _prevTime = Time.time;
       }
    }

     if( _destroyerBorder && collision.collider.transform == _destroyerBorder )
     {
         _factoryBalls.DestroyBall(this);
     }

  }

  private void OnCollisionStay2D( Collision2D collision )
  {
 
    if( AnglelNormalReflect > 90 )
    {
      _velocity = Quaternion.Euler(0,0,AnglelNormalReflect) * _velocity;
    }
    AnglelNormalReflect = Vector3.Angle(_velocity.normalized, collision.contacts[0].normal.normalized );
    if( AnglelNormalReflect == 0 )
    {
      _velocity = Quaternion.Euler(0,0, 15) * _velocity;
    }
  }

  public void ActivateBonus()
  {
     _factoryBalls.SpawnBall( this, 2);
  }


  private void FixedUpdate()
  {   
      _rigidbody.velocity = _velocity.normalized * _speed * Time.fixedDeltaTime ;
  }

}

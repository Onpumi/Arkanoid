using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IPoolable<Ball>
{
  [SerializeField] RayBall _rayBall;
  [SerializeField] private float _speed;
  [SerializeField] private FabrikaBalls _factoryBalls;
  [SerializeField] private Board _board;
  [SerializeField] private Vector3 _startPosition;
  [SerializeField] private BallCollision _ballCollision;
  private Vector2 _velocity;
  private Vector2 _prevDirection;
  private Vector3 _prevPosition;
  public bool IsMove  { get; private set; }
  public float StartAngle { get; private set; }
  private Rigidbody2D _rigidbody;
  private float _timeDelay = 5f;
  public Vector3 StartPosition => _startPosition;

  public float Magnitude;
  public Vector2 Velocity;

   private void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
    InvokeRepeating("TestCollinearity", 0, _timeDelay);
    transform.position = _startPosition;
    _velocity = Vector2.zero;
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
    _velocity = Vector2.zero;
    IsMove = false;
  }

  private void OnEnable()
  {
    _rayBall.OnStartDirection += StartMove; 
    _factoryBalls.OnReproductionBall += StartMove; 
    _board.OnReproductionOne += ActivateBonus;
    _ballCollision.OnDestroyBall += DestroyThisBall;
    _ballCollision.OnCollisionBall += UpdateVelocity;
  }

  private void OnDisable()
  {
    _rayBall.OnStartDirection -= StartMove; 
    _factoryBalls.OnReproductionBall -= StartMove; 
    _board.OnReproductionOne -= ActivateBonus;
    _ballCollision.OnDestroyBall -= DestroyThisBall;
    _ballCollision.OnCollisionBall -= UpdateVelocity;
  }
  private void StartMove( Vector3 startDirection )
  {
     if( _velocity == Vector2.zero || IsMove == false )
     {
        _velocity = startDirection * _speed * Time.fixedDeltaTime;
        _ballCollision.SetVelocity( _velocity );
        RotateDirection( StartAngle );
     }
     IsMove = true;
  }

  private void RotateDirection( float angle )
  {
     _velocity = Quaternion.Euler(0,0,angle) * _velocity;
  }

  public void ActivateBonus()
  {
     _factoryBalls.SpawnBall( this, 2);
  }

  private void DestroyThisBall()
  {
    _factoryBalls.DestroyBall(this);
  }

  private void UpdateVelocity( Vector2 velocity )
  {
    _velocity = velocity;
  }

  private void FixedUpdate()
  {   
    if( IsMove )
    {
      _rigidbody.velocity = _velocity.normalized * _speed * Time.fixedDeltaTime ;

        if( _prevPosition == transform.position )
      {
       // _velocity = -_velocity;
      }
       _prevPosition = transform.position;
    }

    Magnitude = _rigidbody.velocity.magnitude;
    Velocity =  _rigidbody.velocity;
  }

}

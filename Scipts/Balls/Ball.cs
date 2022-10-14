using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Ball : MonoBehaviour, IPoolable<Ball>
{
  [SerializeField] RayBall _rayBall;
  [SerializeField] private float _speed;
  [SerializeField] private FactoryBalls _factoryBalls;
  [SerializeField] private Board _board;
  [SerializeField] private Vector3 _startPosition;
  [SerializeField] private BallCollision _ballCollision;
  [SerializeField] private SpawnerBall _spawnerBall;
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

  public float time = 0;
  public  float prevTime = 0;
  public int count = 0;

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
      if( _spawnerBall )
     {
      _spawnerBall.OnDoSpawn += ActivateBonus;
     }
    _ballCollision.OnDestroyBall += DestroyThisBall;
    _ballCollision.OnCollisionBall += UpdateVelocity;
  }

  private void OnDisable()
  {
    _rayBall.OnStartDirection -= StartMove; 
    _factoryBalls.OnReproductionBall -= StartMove; 
    _board.OnReproductionOne -= ActivateBonus;
    if( _spawnerBall )
    {
      _spawnerBall.OnDoSpawn -= ActivateBonus;
    }
    _ballCollision.OnDestroyBall -= DestroyThisBall;
    _ballCollision.OnCollisionBall -= UpdateVelocity;
  }
  private void StartMove( )
  {
     var startDirection = Vector3.up;
     if( _rayBall.enabled == true )
     {
       startDirection = _rayBall.DirectionRay;
     }
     if( _velocity == Vector2.zero || IsMove == false )
     {
        _velocity = startDirection * _speed * Time.fixedDeltaTime;
        _ballCollision.SetVelocity( _velocity );
        RotateDirection( StartAngle );
        _rigidbody.velocity = _velocity.normalized * _speed * Time.fixedDeltaTime ;
     }
     IsMove = true;
  }

  private void RotateDirection( float angle )
  {
     _velocity = Quaternion.Euler(0,0,angle) * _velocity;
  }

  public void ActivateBonus()
  {
      if( IsMove )
    {
     _factoryBalls.SpawnBall( this, 2);
    }
  }

  private void DestroyThisBall()
  {
    _factoryBalls.DestroyBall(this);
  }

  private void UpdateVelocity( Vector2 velocity )
  {
      if( IsMove )
    {
      _rigidbody.velocity = velocity.normalized * _speed * Time.fixedDeltaTime ;
    }
  }

  public void StopBall()
  {
    _velocity = Vector2.zero;
    IsMove = false;
  }

  public void SpawnBall( int countSpawn )
  {
    _factoryBalls.SpawnBall( this, countSpawn );
  }


  private void SetVelocity()
  {
      //_rigidbody.velocity = _velocity.normalized * _speed * Time.fixedDeltaTime ;    
    _rigidbody.velocity = Vector2.zero;
      //_velocity.Normalize();
     //var a = _rigidbody.velocity;
      //_rigidbody.velocity = Vector2.zero;
      //Instantiate( transform, transform.position, Quaternion.identity);
  }

  private void TestFunction()
  {
      if( time-prevTime >= 1)
      {
       UnityEngine.Debug.Log( count++ );
        prevTime = time;
      }

      time = Time.time;
      //UnityEngine.Debug.Log(time);
  }



/*
  private void FixedUpdate()
  {   
    if( IsMove )
    {
      _rigidbody.velocity = _velocity.normalized * _speed * Time.fixedDeltaTime ;
       _prevPosition = transform.position;
    }

    Magnitude = _rigidbody.velocity.magnitude;
    Velocity =  _rigidbody.velocity;

//    TestFunction();
  }

  */
/*
  #if UNITY_EDITOR
  private void Update()
  {

        if( Input.GetKeyDown(KeyCode.Space))
       {
         int numTests = 250000;
         using(new CustomTimer("Controlled Test", numTests))
         {
             for( int i = 0; i < numTests ; ++i)
             {

               SetVelocity();

             }
         }
       }

  }
  #endif
*/
}

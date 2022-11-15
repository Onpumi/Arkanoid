using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Profiling;
using System.Diagnostics;

[BurstCompile]
public class BallMover : MonoBehaviour
{
 // static readonly ProfilerMarker marker = new ProfilerMarker("MySystem.Prepare");
   [SerializeField] private SoundsPlayer _soundPlayer;
   [SerializeField] private FactoryBalls _factoryBalls;
   private float _speed;
   private bool _isMove = false;
   private Vector2 _direction;
   private Rigidbody2D _rigidBody;
   public bool IsMove { get => _isMove; }
   public float Speed { get => _speed; }
   public Vector2 Direction => _direction;
   private Vector2 _normal;
   private float _angleNormalReflect;
   private Rigidbody2D _rigidbody;
   private Vector2 _reflectDirection;
   private BallTime _ballTime;
   Stopwatch st;

   private Quaternion[] _anglesRotateBall;


   private void OnEnable()
   {
       _factoryBalls.OnMoveBall += Translate;
   }

   private void OnDisable()
   {
       _factoryBalls.OnMoveBall += Translate;
   }


   private void Awake()
   {
      _direction = Vector3.up;
      _rigidBody = GetComponent<Rigidbody2D>();
      //_speed = Time.fixedDeltaTime * 2f * 70f * 2f;
      _speed = 0.02f * 2f * 70f;
      _rigidbody = GetComponent<Rigidbody2D>();
      st = new Stopwatch();
      _ballTime = new BallTime();
      _anglesRotateBall = new Quaternion[3];
      _anglesRotateBall[0] = Quaternion.Euler(0,0,30);
      _anglesRotateBall[1]= Quaternion.Euler(0,0, 15);
   }


  private void OnTriggerEnter2D( Collider2D collider )
  {
       if( _soundPlayer.isCanPlay )
       {
        _soundPlayer.PlayHitBall();
       }

      var closestPoint = collider.ClosestPoint( transform.position  );
      var normal = (Vector2)transform.position - closestPoint;
      normal.Normalize();
      UpdateDirectionEnter( normal );    
       if( collider.TryGetComponent(out Brick brick) )
      {
        brick.Despawn();
      }
  }
  
  private void OnCollisionEnter2D( Collision2D collision )
   {
      // if( _ballTime.isNeedTime() )
     {
       if( _soundPlayer.isCanPlay )
       {
        _soundPlayer.PlayHitBall();
       }

      var closestPoint = collision.collider.ClosestPoint( transform.position  );
      var normal = ((Vector2)transform.position - closestPoint).normalized;

      UpdateDirectionEnter( normal );

      if( collision.collider.TryGetComponent(out Brick brick) )
      {
        if( _ballTime.isNeedTime() )
        {
           brick.Despawn();
           _ballTime.FixedTime();
        }
      }
    }
      _rigidbody.velocity = _direction.normalized * _speed;
      // _ballTime.FixedTime( Time.time );
    //UnityEngine.Debug.Log(_normal);
   }


  private void OnCollisionStay2D( Collision2D collision )
  {
  //   if( _ballTime.isNeedTime() )
  //  {
      float angleRandom = UnityEngine.Random.Range(5,10);
      _direction = Quaternion.Euler(0,0,angleRandom) * _direction;

      if( _angleNormalReflect > 90 )
      {
        //_direction = Quaternion.Euler(0,0,_angleNormalReflect) * _direction;
        _direction = Quaternion.Euler(0,0,_angleNormalReflect-60) * _direction;
      }
      _angleNormalReflect = Vector2.Angle(_direction.normalized, _normal.normalized );
      //if( _angleNormalReflect == 0 )
      //if( _angleNormalReflect >= 0 && _angleNormalReflect <= 5 )
      if( _angleNormalReflect >= 0 && _angleNormalReflect <= 30 )
      {
          //_direction = Quaternion.Euler(0,0, 15) * _direction;
          _direction = Quaternion.Euler(0,0, 30-_angleNormalReflect) * _direction;
      }
      _rigidbody.velocity = _direction.normalized * _speed;
  //   }
   //   _rigidbody.velocity = _direction.normalized * _speed;
   

  }
  
    public void StartMove( Vector2 direction )
   {
      _isMove = true;
      _direction = (Vector3)direction.normalized * _speed;
      _rigidbody.velocity = _direction;
   }

   
    private void UpdateDirectionEnter( Vector2 normal )
   {
      _normal = normal;
      _direction = Vector2.Reflect(_direction,_normal).normalized * _speed;
       var angleRandom = UnityEngine.Random.Range(5,10);
       _direction = Quaternion.Euler(0,0,angleRandom) * _direction;
      _angleNormalReflect = (int) Vector2.Angle(_direction, _normal );
      _rigidbody.velocity = _direction.normalized * _speed;
    // _direction = _direction.normalized * _speed;
   }

    private void UpdateDirectionStay()
   {
       var angleRandom = UnityEngine.Random.Range(5,10);
      _direction = Quaternion.Euler(0,0,angleRandom) * _direction;

      if( _angleNormalReflect > 90 )
      {
        _direction = Quaternion.Euler(0,0,_angleNormalReflect) * _direction;
      }
      _angleNormalReflect = (int)Vector2.Angle(_direction.normalized, _normal.normalized );
      if( _angleNormalReflect == 0 )
      {
          _direction = Quaternion.Euler(0,0, 15) * _direction;
      }
      _rigidbody.velocity = _direction.normalized * _speed;
   }


   public void Translate()
   {
      if( IsMove == true )
      {
         //transform.Translate(_direction);
         transform.position += (Vector3)_direction;
         //collision.collider.TryGetComponent(out Brick brick)
         //UnityEngine.Debug.Log(_direction.magnitude);
      }
   }


   public void Func()
   {
       float angleRandom = UnityEngine.Random.Range(5,10);
      _direction = Quaternion.Euler(0,0,angleRandom) * _direction;

      if( _angleNormalReflect > 90 )
      {
        _direction = _anglesRotateBall[0] * _direction;
        //_direction = Quaternion.Euler(0,0,_angleNormalReflect-60) * _direction;
      }
      _angleNormalReflect = Vector2.Angle(_direction.normalized, _normal.normalized );
          _direction = _anglesRotateBall[1] * _direction;
          //_direction = Quaternion.Euler(0,0, 30-_angleNormalReflect) * _direction;
      _rigidbody.velocity = _direction.normalized * _speed;
   }

  /* 
   private void FixedUpdate()
  {

         if( Input.GetKeyDown(KeyCode.Space))
       {
         int numTests = 1000 * 20;
         using(new CustomTimer("Controlled Test", numTests))
         {
             for( int i = 0; i < numTests ; ++i)
             {
               // Func();            
             }
         }
       }

      if( IsMove == true )
      {
   //     transform.Translate(_direction);
      }
#if UNITY_EDITOR
     // UnityEngine.Debug.DrawRay( transform.position, _direction.normalized, Color.red);
      //UnityEngine.Debug.DrawRay( transform.position, _normal, Color.green);
     // UnityEngine.Debug.Log(_normal);
#endif
  }
*/
   

  
}

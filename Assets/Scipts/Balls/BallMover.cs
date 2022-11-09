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
   Stopwatch st;


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
      _speed = Time.fixedDeltaTime * 2f * 60f;
      _rigidbody = GetComponent<Rigidbody2D>();
      st = new Stopwatch();
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
        brick.Despawn();
      }
    }
   }


  private void OnCollisionStay2D( Collision2D collision )
  {
      float angleRandom = UnityEngine.Random.Range(5,10);
      _direction = Quaternion.Euler(0,0,angleRandom) * _direction;

      if( _angleNormalReflect > 90 )
      {
        _direction = Quaternion.Euler(0,0,_angleNormalReflect) * _direction;
      }
      _angleNormalReflect = Vector2.Angle(_direction.normalized, _normal.normalized );
      //if( _angleNormalReflect == 0 )
      if( _angleNormalReflect >= 0 && _angleNormalReflect <= 3 )
      {
          _direction = Quaternion.Euler(0,0, 15) * _direction;
      }
      _rigidbody.velocity = _direction.normalized * _speed;
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
         //UnityEngine.Debug.Log(_direction.magnitude);
      }
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
              
               _angleNormalReflect = Vector2.Angle(_direction, _normal );
             }
         }
       }

      if( IsMove == true )
      {
        transform.Translate(_direction);
      }
#if UNITY_EDITOR
     // UnityEngine.Debug.DrawRay( transform.position, _direction.normalized, Color.red);
      //UnityEngine.Debug.DrawRay( transform.position, _normal, Color.green);
     // UnityEngine.Debug.Log(_normal);
#endif
  }

  */ 

  
}

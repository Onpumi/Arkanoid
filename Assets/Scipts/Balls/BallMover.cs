using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Burst;
using Unity.Profiling;
using System.Diagnostics;

[BurstCompile]
public class BallMover : MonoBehaviour
{
 // static readonly ProfilerMarker marker = new ProfilerMarker("MySystem.Prepare");
   [SerializeField] private SoundsPlayer _soundPlayer;
   [SerializeField] private Board _board;
   [SerializeField] private LayerMask _maskaRaycast;
   private float _speed;
   private bool _isMove = false;
   private Vector2 _direction;
   private Rigidbody2D _rigidBody;
   public bool IsMove { get => _isMove; }
   public float Speed { get => _speed; }
   public Vector2 Direction => _direction;
   private Vector3 _newPosition;
   private Collider2D _collider;
   private CircleCollider2D _circleCollider;
   private RaycastHit2D[] _hitNewPoint;
   private RaycastHit2D contactHitPoint;
   private Vector2 _normal;
   private float _angleNormalReflect;
   private Rigidbody2D _rigidbody;
   private BallTime _ballTime;
   private Vector2 _reflectDirection;
   Stopwatch st;

   private void Awake()
   {
      _direction = Vector3.up;
      _rigidBody = GetComponent<Rigidbody2D>();
      _collider = GetComponent<Collider2D>();
      _circleCollider = GetComponent<CircleCollider2D>();
      _hitNewPoint = new RaycastHit2D[1];
      _speed = Time.fixedDeltaTime * 2f * 50f ;
      _rigidbody = GetComponent<Rigidbody2D>();
      _ballTime = new BallTime();
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
      var normal = (Vector2)transform.position - closestPoint;
      normal.Normalize();

      //normal = collision.contacts[0].normal;

      UpdateDirectionEnter( normal );
      if( collision.collider.TryGetComponent(out Brick brick) )
      {
        brick.Despawn();
      }
    }
   }


  private void OnCollisionStay2D( Collision2D collision )
  {
    UpdateDirectionStay();
  }
  
    public void StartMove( Vector2 direction )
   {
      _isMove = true;
      _direction = direction;
      _direction = (Vector3)_direction.normalized * _speed;
      _rigidbody.velocity = _direction;
   }

   
    private void UpdateDirectionEnter( Vector2 normal )
   {
      _normal = normal;
      _direction = Vector2.Reflect(_direction,_normal).normalized * _speed;
       var angleRandom = UnityEngine.Random.Range(5,10);
       _direction = Quaternion.Euler(0,0,angleRandom) * _direction;
      _angleNormalReflect = Vector2.Angle(_direction, _normal );
      _rigidbody.velocity = _direction.normalized * _speed;
   }

    private void UpdateDirectionStay()
   {
    //  if( _ballTime.isNeedTime() )
    {
       var angleRandom = UnityEngine.Random.Range(5,10);
      _direction = Quaternion.Euler(0,0,angleRandom) * _direction;

      if( _angleNormalReflect > 90 )
      {
        _direction = Quaternion.Euler(0,0,_angleNormalReflect) * _direction;
      }
      _angleNormalReflect = Vector2.Angle(_direction.normalized, _normal.normalized );
      if( _angleNormalReflect == 0 )
      {
          _direction = Quaternion.Euler(0,0, 15) * _direction;
      }
      _rigidbody.velocity = _direction.normalized * _speed;
    }
   }

   
  // private void Update()
  //{
//      if( IsMove == true )
    //  {
      //  transform.Translate(_direction);
      //}
#if UNITY_EDITOR
     // UnityEngine.Debug.DrawRay( transform.position, _direction.normalized, Color.red);
      //UnityEngine.Debug.DrawRay( transform.position, _normal, Color.green);
     // UnityEngine.Debug.Log(_normal);
#endif
//  }

   

  
}

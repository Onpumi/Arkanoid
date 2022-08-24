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
  private Vector2 _velocity;
  private Vector2 _prevDirection;
  public bool IsMove  { get; private set; }
  public float StartAngle { get; private set; }
  private Rigidbody2D _rigidbody;
  public float Radius;
  public Vector2 Normal;
  public Vector2 ReflectDirection;
  public bool IsContact; 
  public float AnglelNormalReflect;
  private float[] _normals;
  private float _timeDelay = 5f;
  private float _timePrev;
  private float _sizeBall;
  private float _radiusBall;
  private Vector2 _prevPosition;
  private float _prevTime;
  private float _countCollision = 0;
  private HashSet<Collision2D> _collisions;

   private void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
    IsContact = false;
    _normals = new float[2];
    _prevDirection = _velocity = Vector2.zero;
    InvokeRepeating("TestCollinearity", 0, _timeDelay);
    _radiusBall = GetComponent<CircleCollider2D>().radius;
    _sizeBall = _radiusBall * transform.localScale.x;
    _collisions = new HashSet<Collision2D>();

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
    Normal = collision.contacts[0].normal;
    AnglelNormalReflect = Vector3.Angle(ReflectDirection.normalized, collision.contacts[0].normal.normalized );
  //  AngelReflectDirection = Vector3.Angle(ReflectDirection.normalized, _velocity.normalized);
    _velocity = ReflectDirection;
    

     _collisions.Add(collision);

    var deltaPosition = (_rigidbody.position-_prevPosition).magnitude;
    var deltaTime = Time.time - _prevTime;

    
    if( _parentRemoveBricks && collision.collider.transform.parent == _parentRemoveBricks  ) 
    {
       Destroy(collision.collider.transform.gameObject);
       _prevPosition = _rigidbody.position;
       _prevTime = Time.time;
    }
    
  IsContact = true;

     if( _destroyerBorder && collision.collider.transform == _destroyerBorder )
     {
         _factoryBalls.DestroyBall(this);
     }

     _timePrev = Time.time;
  }

  private void OnCollisionStay2D( Collision2D collision )
  {
   //  _rigidbody.velocity = Vector2.zero;
    var buffDirection = _velocity;

    Normal = collision.contacts[0].normal;
    if( _parentRemoveBricks && collision.collider.transform.parent == _parentRemoveBricks) 
    {
     // return;
    }

   //IsContact = false;

    if( AnglelNormalReflect > 90 )
    {
      _velocity = Quaternion.Euler(0,0,AnglelNormalReflect) * _velocity;
    }
    AnglelNormalReflect = Vector3.Angle(_velocity.normalized, collision.contacts[0].normal.normalized );
    if( AnglelNormalReflect == 0 )
    {
      _velocity = Quaternion.Euler(0,0, 15) * _velocity;
    }
    //IsContact = true;
  }

  private void OnCollisionExit2D( Collision2D collision )
  {
    Normal = Vector2.zero;
      _countCollision=0;

    _collisions.Remove(collision);

      IsContact = false;
      _countCollision--;
  }

  public void ActivateBonus()
  {
     _factoryBalls.SpawnBall( this, 2);
  }


  private void FixedUpdate()
  {   
      _rigidbody.velocity = _velocity.normalized * _speed * Time.fixedDeltaTime ;
  }

  private void Update()
  {
    //Debug.DrawRay( transform.position, (Vector3)_velocity * 0.01f, Color.red );
    //Debug.DrawRay( transform.position, (Vector3)Normal * 0.05f, Color.green );
    //_rigidbody.velocity = _velocity;
  //  Debug.Log(_countCollision);
     if( Input.GetKeyDown(KeyCode.Space)) 
   {
    _factoryBalls.SpawnBall( this, 2);
   }

    if(Input.GetMouseButtonDown(0))
   {
      if( Input.mousePosition.y > 150)
      {
        //_factoryBalls.SpawnBall(this,2);
      }
   }
  }
}

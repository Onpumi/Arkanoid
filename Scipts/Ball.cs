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
  private Vector2 _velocity;
  private Vector2 _prevDirection;
  public bool IsMove  { get; private set; }
  public float StartAngle { get; private set; }
  private Rigidbody2D _rigidbody;
  public float Radius;
  public Vector2 Normal;
  public Vector2 ReflectDirection;
  public bool IsContact; 
  public float AnglelNormalDirection;
  public float AnglelNormalReflect;
  public float AngelReflectDirection;
  private float[] _normals;
  private float _timeDelay = 5f;

  
  

   private void Awake()
  {

//     Array<Vector2> normals = new Array<Vector2>();

    //normals.Add(new Vector2(1,3));

     //for( int i = 0 ; i < normals.Count ; i++ )
     //Debug.Log(normals[i]);
      Vector3 v1 = Vector3.up;
      Vector3 v2 = -v1;
     //Debug.Log( Vector3.Angle(v1,v2) + " " + Vector3.Angle(v1,-v2) );

    _rigidbody = GetComponent<Rigidbody2D>();
    IsContact = true;
    _normals = new float[2];
    _prevDirection = _velocity = Vector2.zero;

    InvokeRepeating("TestCollinearity", 0, _timeDelay);
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
  }

  private void OnDisable()
  {
    _rayBall.OnStartDirection -= StartMove; 
    _factoryBalls.OnReproductionBall -= StartMove; 
  }


  private void StartMove( Vector3 startDirection )
  {
     if( _velocity == Vector2.zero )
     {
        _velocity = startDirection * _speed * Time.fixedDeltaTime;
        if( _velocity.magnitude < 1 )
       {
        _velocity.Normalize();
       }
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


    ReflectDirection = Vector3.Reflect(_velocity,collision.contacts[0].normal);
    Normal = collision.contacts[0].normal;
    AnglelNormalDirection = Vector3.Angle(_velocity.normalized, collision.contacts[0].normal.normalized );
    AnglelNormalReflect = Vector3.Angle(ReflectDirection.normalized, collision.contacts[0].normal.normalized );
    AngelReflectDirection = Vector3.Angle(ReflectDirection.normalized, _velocity.normalized);
    _velocity = ReflectDirection;
    IsContact = true;


    if( _parentRemoveBricks && collision.collider.transform.parent == _parentRemoveBricks) 
    {
      Destroy(collision.collider.transform.gameObject);
    }

     if( _destroyerBorder && collision.collider.transform == _destroyerBorder )
     {
      _factoryBalls.DestroyBall(this);
     }

  }

  private void OnCollisionStay2D( Collision2D collision )
  {

    Normal = collision.contacts[0].normal;

    if( AnglelNormalReflect > 90 )
    {
     // _velocity = Quaternion.Euler(0,0,15) * _velocity;
      _velocity = Quaternion.Euler(0,0,AnglelNormalReflect) * _velocity;
    }
    AnglelNormalReflect = Vector3.Angle(_velocity.normalized, collision.contacts[0].normal.normalized );
    if( AnglelNormalReflect == 0 )
    {
      _velocity = Quaternion.Euler(0,0, 15) * _velocity;
    }
    IsContact = true;
  }

  private void OnCollisionExit2D( Collision2D collision )
  {
    Normal = Vector2.zero;
    IsContact = false;
  }


  private void FixedUpdate()
  {
      _rigidbody.velocity = _velocity;

      if( IsNotSpeed() )
      {
        GetComponent<SpriteRenderer>().color=Color.red;
      }
  }


  private bool IsNotSpeed() => ( _velocity.x == 0 && _velocity.y == 0 && IsMove );


  private void Update()
  {
    //Debug.DrawRay( transform.position, (Vector3)_velocity * 0.01f, Color.red );
    //Debug.DrawRay( transform.position, (Vector3)Normal * 0.05f, Color.green );

            if( Input.GetKeyDown(KeyCode.Space)) 
        {
            _factoryBalls.SpawnBall( this, 2);
        }

  }


}

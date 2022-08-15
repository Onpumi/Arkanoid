using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _parentBalls;
    [SerializeField] private Transform _parentBricks;
    [SerializeField] private Board _board;
    [SerializeField] private LayerMask _maskaRaycast;
    public Vector2 _direction = new Vector2(1f,1f);
    private Rigidbody2D _rigidbody;
    public Vector2 _prevDirection;
    private Vector2 prevPosition;
    public Vector2 Direction => _direction;
    public Vector2 Normal;
    private Vector2 Contact;
    public bool _isHit = false;
    private List<RaycastHit2D> hits = new List<RaycastHit2D>();
    private RaycastHit2D hit;
    private Vector2 pointCollision;
    private float _angleRotate = 90f;
    Vector2 targetBall; 
    float enterTime;
    float repeatTime;   
    float stayTime;
    public int countPointContacts;
    public string colliderName;
    [SerializeField] private Transform circle;
    [SerializeField] private int countColliders;
    [SerializeField] private int countCollisions;
    private List<Collider2D> _colliders;   
    private List<Vector2> _normals;
    private List<ContactPoint2D[]> _contacts;
    private List<Collision2D> _collisions;
    private Dictionary<Transform,Vector2>  _points;
    private HashSet<Transform> _transformsContact;
    

      public float Speed 
    {
        get => _speed;
        private set {}
    }

 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _direction = GetMoveDirection( _direction, -5f * Mathf.PI / 180f );
        _colliders = new List<Collider2D>();
        _normals = new List<Vector2>();
        _contacts = new List<ContactPoint2D[]>();
        _collisions = new List<Collision2D>();
        _points = new Dictionary<Transform, Vector2>();
        _transformsContact = new HashSet<Transform>();

    }

    private void Start()
    {
        _direction = GetMoveDirection( _direction, -_ball.StartAngle * Mathf.PI / 180f );
    }



  private void OnCollisionEnter2D( Collision2D collision )
  {
   
    repeatTime = Time.time-enterTime;
    enterTime = Time.time;

    _colliders.Add(collision.collider);
    countColliders = _colliders.Count;


    countPointContacts = collision.contacts.Length;
    colliderName = collision.collider.transform.name;
    
    //Instantiate(circle, (Vector3)collision.contacts[0].point, Quaternion.identity);

            Vector2 normal = Vector2.zero;



        foreach( var contact in collision.contacts )
        {
           normal += contact.normal;
        }

        normal /= collision.contacts.Length;


          if( _points.ContainsKey(collision.collider.transform) == false )
          {
           //  _points[collision.collider.transform] = collision.contacts[0].normal;
             _points[collision.collider.transform] = normal;
          }
        

          

          _transformsContact.Add(collision.collider.transform);

        countCollisions = _collisions.Count;


        foreach( var normalCurrent in _points.Values )
        {
            normal += normalCurrent;
        }

        normal /= _points.Count;

        //if( normal.x != 0 ) normal.x = Mathf.Sign(normal.x) * 1;
        //if( normal.y != 0 ) normal.y = Mathf.Sign(normal.y) * 1;

        //if( Mathf.Abs(normal.x) > Mathf.Abs(normal.y) ) normal.y = 0;
        //if( Mathf.Abs(normal.x) < Mathf.Abs(normal.y) ) normal.x = 0;

        if( _points.Count > 1 )
        {
          _direction = normal;


          Normal = normal;
          return;
        }

  //  if(_isHit == false)
//    {
          _direction = Vector3.Reflect(_direction.normalized, normal.normalized);
          Normal = normal * 25;
    
        //  ReflectBall();
        

    

        if( collision.collider.transform.parent == _parentBricks ) 
     {
        Destroy(collision.collider.transform.gameObject);
     }

  //  }
     _isHit = true;
  }

  private void OnCollisionStay2D( Collision2D collision )
  {
      stayTime = Time.time-enterTime;
      Vector2 normal = Vector2.zero;
//if( _rigidbody.velocity.x < 1 && _ball.name == "ball 49")
//Debug.Log(collision.contacts.Length);

   

      if(stayTime > Time.fixedDeltaTime)
      {
         foreach( var contact in collision.contacts )
         {
           normal += contact.normal;
         }
         normal /= collision.contacts.Length;
      //  normal = collision.contacts[0].normal;
        // Normal = normal * 25;
       // _direction = Vector3.Reflect(_direction.normalized, normal.normalized);
      }
      
  }


  private void OnCollisionExit2D( Collision2D collision )
  {

    _isHit = false;
    stayTime = Time.time-enterTime;
    _colliders.Remove(collision.collider);

    _points.Remove(collision.collider.transform);
    _transformsContact.Remove(collision.collider.transform);

    countColliders = _colliders.Count;
    countCollisions = _collisions.Count;
//    Debug.Log(_points.Count);
  }

    private void ReflectBall()
    {
        Vector2 normal;
        hit = Physics2D.CircleCast(transform.position, 0.5f,_direction, 5f, _maskaRaycast );
        pointCollision = hit.point;
        targetBall = pointCollision - (Vector2)transform.position;
        
          if( hit.collider == null ) 
          {
            Debug.Log("NULL");
            return;
          }
       normal = (Vector2)transform.position - hit.collider.ClosestPoint(transform.position);
       normal = hit.normal;
       //_direction = Vector2.Reflect( _direction.normalized, normal.normalized);
       _direction = Vector2.Reflect( _direction, normal.normalized);
       Normal = normal * 25;
       Contact = hit.point;

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
      // _direction = new Vector2( _direction.x * Mathf.Cos(delta) - _direction.y * Mathf.Sin(delta), _direction.x * Mathf.Sin(delta) + _direction.y * Mathf.Cos(delta) ) ;
    }



    private void FixedUpdate()
    {   

        if( _ball.IsMove )
        {
          //_rigidbody.velocity = _direction.normalized * Time.fixedDeltaTime * GetSpeed();  
          _rigidbody.velocity = _direction.normalized * Time.fixedDeltaTime * GetSpeed();  
        }
    
        

        Debug.DrawLine(_ball.transform.position, _ball.transform.position + (Vector3)Normal.normalized, Color.green );
        Debug.DrawLine(_ball.transform.position, _ball.transform.position + (Vector3)_direction.normalized, Color.red );
        Debug.DrawLine(_ball.transform.position, (Vector3)Contact , Color.magenta );
        
        prevPosition = _rigidbody.position;
    }


    private float GetSpeed()
    {
        var speed = _direction.magnitude;
        return Mathf.Max(speed, Speed);
    }


    private void Update()
    {
      if( Input.GetMouseButtonDown(0))
      {
        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //_direction = target-transform.position;
      }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Ball _ball;
    private Vector2 _direction = new Vector2(0f,1f);
    private Rigidbody2D rigidbody;

      public float Speed 
    {
        get => _speed;
        private set {}
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        _direction = GetMoveDirection( _direction, -5f * Mathf.PI / 180f );
        _ball.Direction = _direction;

        //rigidbody.collisionDetectMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void OnEnable()
    {
        _ball.OnHit += HitBlock;
    }

    private void OnDisable()
    {
      _ball.OnHit -= HitBlock;
    }

    private void HitBlock( Vector2 normal, int angleBoard )
    {
        _direction = Vector2.Reflect( _direction.normalized, normal );
        Vector2 rotateVector = Vector2.zero;
        if( angleBoard != 0 )
        {
           var angleToRadian = angleBoard * Mathf.PI / 180f;
           rotateVector = GetMoveDirection( new Vector2(0,1), -angleToRadian );
        }
        if( rotateVector != Vector2.zero )
        {
            rotateVector.Normalize();
            _direction = rotateVector;
            _direction.Normalize();
        }
        _ball.Direction = _direction;
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


    private void FixedUpdate()
    {
        if( _ball.IsMove )
        {
           // rigidbody.MovePosition( transform.position + (Vector3)_direction * Time.fixedDeltaTime * Speed );
            rigidbody.velocity = (Vector3)_direction * Time.fixedDeltaTime * GetSpeed() * 50f;
           // rigidbody.velocity = rigidbody.velocity + _direction * Time.fixedDeltaTime * Speed;
            //rigidbody.AddForce((Vector3)_direction * Time.fixedDeltaTime * Speed,ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if( _ball.IsMove )
        {
           // transform.Translate((Vector3)_direction * Time.deltaTime * Speed);
           //rigidbody.position = Vector2.MoveTowards(rigidbody.position, rigidbody.position+_direction, Time.deltaTime * Speed );
        }

        //Debug.DrawLine(transform.position, transform.position+(Vector3)_direction * 10, Color.cyan);
    }

    private float GetSpeed()
    {
        var speed = _direction.magnitude;
        return Mathf.Max(speed, Speed);
    }


}

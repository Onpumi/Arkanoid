using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _parentBalls;
    [SerializeField] private Board _board;
    [SerializeField] private float _limitAngle;
    private Vector2 _direction = new Vector2(0f,1f);
    private Rigidbody2D _rigidbody;
    private int countBalls;

      public float Speed 
    {
        get => _speed;
        private set {}
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _direction = GetMoveDirection( _direction, -5f * Mathf.PI / 180f );
    }

    private void OnEnable()
    {
        _ball.OnHit += HitBlock;
        _ball.OnFreeze += ResetDirection;

    }

    private void OnDisable()
    {
      _ball.OnHit -= HitBlock;
      _ball.OnFreeze -= ResetDirection;
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

        if( Vector3.Angle(_direction, new Vector3(0,1,0)) < _limitAngle || Vector3.Angle(_direction, new Vector3(0,-1,0)) < _limitAngle  )
        {
            _direction = GetMoveDirection( _direction, _limitAngle );
        }
         else if( Vector3.Angle(_direction, new Vector3(1,0,0)) < _limitAngle || Vector3.Angle(_direction, new Vector3(-1,0,0)) < _limitAngle  )
        {
            _direction = GetMoveDirection( _direction, _limitAngle );
        }
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
            _rigidbody.velocity = (Vector3)_direction * Time.fixedDeltaTime * GetSpeed() * 50f;
        }
        else
        {
             var target = new Vector2( _board.transform.position.x, transform.position.y);
            _rigidbody.position = new Vector2( target.x, _rigidbody.position.y );
        }
            
    }

    public void ResetDirection()
    {
        _direction=-_direction;
    }

    private float GetSpeed()
    {
        var speed = _direction.magnitude;
        return Mathf.Max(speed, Speed);
    }
}

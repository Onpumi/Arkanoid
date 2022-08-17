using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
  [SerializeField] private float _width;
  [SerializeField] private Color _color;
  [SerializeField] private Transform _frameParent;
  [SerializeField] private Border _border;
  [SerializeField] private int[] _anglesMove;
  [SerializeField] private float _speed;
  [SerializeField] private RayBall _rayBall;
  private Rigidbody2D _rigidbody;
  public float Speed => _speed;
  private List<float[]> _segmentsBoard = new List<float[]>();
  public event Action OnMove;

    private void Awake()
    {
      int countAngles = _anglesMove.Length * 2 - 1;
      int countAnglesRight = _anglesMove.Length - 1;
      float lengthSegment = 1f / (float)countAngles;
      float[] segment = { 0f, lengthSegment };
      _rigidbody = GetComponent<Rigidbody2D>();


      for( int i = 1 ; i < countAnglesRight + 2 ; i++ )
     {
        _segmentsBoard.Add(new float[2]{ lengthSegment * (float)(i-1), lengthSegment * (float)(i) }); 
     }

    }


    private void Move()
    {
        var directionX = Input.GetAxis("Mouse X");
        var positionX = _rigidbody.position.x + directionX;
          
        var target = _rigidbody.position + new Vector2( directionX, 0) * Time.deltaTime * _speed;
        if( 
            target.x < _border.MaxHorizontalPosition - transform.localScale.x / 2f - _border.FrameWidth / 2f && 
            target.x > _border.MinHorizontalPosition + transform.localScale.x / 2f + _border.FrameWidth / 2f 
          )
       {
         _rigidbody.MovePosition( target );
         if( directionX != 0 )
         {
           OnMove?.Invoke();
         }
       }
    }

    public int GetAngleReflect( float localPositionX )
    {
      int countAngles = _anglesMove.Length * 2 - 1;
      int countAnglesRight = _anglesMove.Length - 1;
      float lengthSegment = 1f / (float)countAngles;
      int angleResult = 0;
      foreach( var segment in _segmentsBoard )
      for( int i = 0 ; i < _segmentsBoard.Count; i++ )
      {
          if( localPositionX > _segmentsBoard[i][0] && localPositionX <= _segmentsBoard[i][1] )
          {
            angleResult = _anglesMove[i];
          }
          else if( localPositionX < -_segmentsBoard[i][0] && localPositionX >= -_segmentsBoard[i][1] )
          {
            angleResult = -_anglesMove[i];
          }
      }
        return angleResult;
    }

    private void Update()
    {
      if( _rayBall.IsDraw == false )
      {
         Move();
      }
    }
  
}


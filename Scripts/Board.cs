using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
  [SerializeField] private float _width;
  [SerializeField] private Color _color;
  [SerializeField] private InputBoard _inputBoard;
  [SerializeField] private Transform _frameParent;
  [SerializeField] private Border _border;
  [SerializeField] private int[] AnglesMove;
  [SerializeField] private float _speed;
  public float Speed => _speed;
  private List<float[]> segmentsBoard = new List<float[]>();
  



    private void Awake()
    {
      int countAngles = AnglesMove.Length * 2 - 1;
      int countAnglesRight = AnglesMove.Length - 1;
      float lengthSegment = 1f / (float)countAngles;
      float[] segment = { 0f, lengthSegment };

      for( int i = 1 ; i < countAnglesRight + 2 ; i++ )
     {
        segmentsBoard.Add(new float[2]{ lengthSegment * (float)(i-1), lengthSegment * (float)(i) }); 
     }

    }

    private void OnEnable()
    {
      _inputBoard.OnMove += Move;
    }

    private void OnDisable()
    {
      _inputBoard.OnMove -= Move;
    }

    private void Move()
    {
        //transform.position = Vector3.Lerp(transform.position,_inputBoard.TouchPosition,Time.deltaTime * 10);
        var touchPosition = _inputBoard.TouchPosition;
        var targetPositionX = transform.position.x  + touchPosition.x;
        if( targetPositionX < _border.MaxHorizontalPosition + transform.localScale.x && targetPositionX > _border.MinHorizontalPosition - transform.localScale.x )
        {
          transform.position = Vector3.MoveTowards(transform.position, touchPosition, Time.deltaTime * Speed);
          //transform.position = new Vector3 (touchPosition.x, transform.position.y,transform.position.z);
        }
    }



    public int GetAngleReflect( float localPositionX )
    {
      int countAngles = AnglesMove.Length * 2 - 1;
      int countAnglesRight = AnglesMove.Length - 1;
      float lengthSegment = 1f / (float)countAngles;
      int angleResult = 0;
      foreach( var segment in segmentsBoard )
      for( int i = 0 ; i < segmentsBoard.Count; i++ )
      {
          if( localPositionX > segmentsBoard[i][0] && localPositionX <= segmentsBoard[i][1] )
          {
            angleResult = AnglesMove[i];
          }
          else if( localPositionX < -segmentsBoard[i][0] && localPositionX >= -segmentsBoard[i][1] )
          {
            angleResult = -AnglesMove[i];
          }

      }
     
        return angleResult;
       
    }

  
}


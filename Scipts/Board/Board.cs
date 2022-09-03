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
  [SerializeField] private FabrikaBalls _factoryBalls;
  [SerializeField] private HealthView _healthView;
  [SerializeField] private MenuEndView _lossView;
  private Rigidbody2D _rigidbody;
  public Health Health { get; private set; }
  public float Speed => _speed;
  private List<float[]> _segmentsBoard = new List<float[]>();
  public event Action OnMove;
  public event Action OnReproductionOne;
  public event Action<MenuEndView,Transform> OnLostAll;

  
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
      
      Health = new Health( 3, 5, _healthView );
      _healthView.DisplayItems( Health.CurrentValue );
    }

    private void OnEnable()
    {
        _factoryBalls.OnLossAllBalls += TakeDamage;
        Health.OnLossHealth += FinishLevel;
    }

    private void OnDisable()
    {
       _factoryBalls.OnLossAllBalls -= TakeDamage;
       Health.OnLossHealth -= FinishLevel;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {

       if( collision.collider.TryGetComponent(out BonusBall bonusBall) )
       {

          if( bonusBall.ReturnBonus() != null )
          {
             bonusBall.ActivateBonus( OnReproductionOne );
          } 
          Destroy( bonusBall.transform.gameObject );
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

    private void TakeDamage()
    {
       Health.TakeDamage();
    }

    private void FinishLevel()
    {
        Debug.Log("ПРОИГРЫШ!");
        OnLostAll?.Invoke( _lossView, this.transform );
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


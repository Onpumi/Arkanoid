using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : MonoBehaviour
{
  [SerializeField] private float _width;
  [SerializeField] private Color _color;
  [SerializeField] private Transform _frameParent;
  [SerializeField] private Border _border;
  [SerializeField] private int[] _anglesMove;
  [SerializeField] private float _speed;
  [SerializeField] private RayBall _rayBall;
  [FormerlySerializedAs("_factoryBalls")] [SerializeField] private ContainerBalls _containerBalls;
  [SerializeField] private HealthView _healthView;
  [SerializeField] private MenuEndView _lossView;
  [SerializeField] private SoundsPlayer _soundPlayer;
  private Rigidbody2D _rigidbody;
  public Health Health { get; private set; }
  public float Speed => _speed;
  private List<float[]> _segmentsBoard = new List<float[]>();
  public event Action OnMove;
  public event Action OnReproductionOne;
  public event Action OnReproductionTwo;
  public event Action<MenuEndView> OnLostAll;

  
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
        _containerBalls.OnLossAllBalls += TakeDamage;
        Health.OnLossHealth += FinishLevel;
    }

    private void OnDisable()
    {
       _containerBalls.OnLossAllBalls -= TakeDamage;
       Health.OnLossHealth -= FinishLevel;
    }

    private void InvokeBonus( Action actionBonus )
    {
        actionBonus?.Invoke();
        _soundPlayer.PlayGetBonus();
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {

       if( collision.collider.TryGetComponent(out BonusBall bonusBall) )
       {

          if( bonusBall.Type == TypeBonus.ReproductionOne ) 
              InvokeBonus(OnReproductionOne);
          else if( bonusBall.Type == TypeBonus.ReproductionTwo )
              InvokeBonus(OnReproductionTwo);
          bonusBall.transform.gameObject.SetActive(false);
       }
    }

    private void Move()
    {
        var directionX = Input.GetAxis("Mouse X");
        var target = transform.position + new Vector3( directionX, 0) * Time.deltaTime * _speed;
        if( 
            target.x < _border.MaxHorizontalPosition - transform.localScale.x / 2f - _border.FrameWidth / 2f && 
            target.x > _border.MinHorizontalPosition + transform.localScale.x / 2f + _border.FrameWidth / 2f 
          )
       {
       transform.position = target;
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
        OnLostAll?.Invoke( _lossView );
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

/*
    #if UNITY_EDITOR
Vector2 v;
    private void FixedUpdate()
    {
           if( Input.GetButtonDown("Fire2"))
       {
          UnityEngine.Debug.Log("запуск теста");
         int numTests = 500 * 20;
         using(new CustomTimer("Controlled Test", numTests))
         {
             for( int i = 0; i < numTests ; ++i)
             {

             }
         }
         UnityEngine.Debug.Log("окончание теста");
       }
    }
    #endif
*/  
}


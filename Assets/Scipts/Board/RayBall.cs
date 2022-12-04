using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RayBall : MonoBehaviour
{
    [SerializeField] private Transform _prefabBall;
    [SerializeField] private Transform _prefabHitBall;
    [SerializeField] private Transform _prefabNormalBall;
    [SerializeField] private float _step;
    [SerializeField] private int _countStep;
    [SerializeField] private LayerMask _maskaRaycast;
    [SerializeField] private Ball _ball;
    [SerializeField] private ContainerBalls _containerBalls;
    [SerializeField] private PlayingScene _playingScene;
    private Vector3 _target;
    public bool IsDraw { get; private set; }
    private List<Transform> _balls;
    private List<Transform> _hitBalls;
    private List<Transform[]> _hitNormals;

    private const int SizeNormal = 5;
    private float _scaleBall;
    private const float RadiusBall = 0.02f;
    private float SizeBall => _scaleBall * RadiusBall;
    private Vector3 _direction;
    private bool _isReady = false;
    private  readonly Vector3 _startBallPosition = new Vector3(-1f,-2.5f);

    private void Awake()
    {
        IsDraw = false;
        _balls = new List<Transform>();
        _hitBalls = new List<Transform>();
        _hitNormals = new List<Transform[]>();
      if( _ball )
      {
           _scaleBall = _ball.transform.localScale.x;
      }
    }

    private void OnEnable()
    {
      _ball = _containerBalls.GetFirstBall();
      _ball.transform.position = _startBallPosition;
      _ball.BallMover.StopMove();
      _isReady = false;
    }

    private void OnDisable()
    {
        _isReady = false;
        // if( _ball.BallMover.IsMove == false )
        // _factoryBalls.DespawnBall( _ball );
    }

    private void ClearBalls()
    {
           if( _balls.Count > 0  )
           {
            foreach( var ball in _balls )
            {
               Destroy(ball.transform.gameObject);
            }
            _balls.Clear();
           }
    }

    private void ClearHitBalls()
    {
         if( _hitBalls.Count > 0 )
         {
            foreach( var hitball in _hitBalls)
            {
                Destroy( hitball.transform.gameObject );
            }
            _hitBalls.Clear();
         }
    }

    private void ClearHitNormals()
    {
        if( _hitNormals.Count > 0) 
        {
          foreach( var hitNormal in _hitNormals )
          {
              for( int i = 0 ; i < SizeNormal ; i++ )
              {
                Destroy( hitNormal[i].gameObject );
              }
          }
        }
        _hitNormals.Clear();
    }

    private void DrawNormal( Vector3 startPosition, Vector3 direction )
    {

       Transform[] normalBalls = new Transform[SizeNormal];

       for( int i = 0 ; i < SizeNormal ; i++ )
       {
          normalBalls[i] = Instantiate(_prefabNormalBall, startPosition + (i * _step * 0.5f) * direction, Quaternion.identity);
       }
       _hitNormals.Add(normalBalls);
    }

    private void ClearRay()
    {
         ClearBalls();
         ClearHitBalls();
         ClearHitNormals();
    }

    private void DrawRay()
    {
        var startPosition = _ball.transform.position;
        var currentPosition = startPosition;
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var targetDirection = _target-startPosition;
        IsDraw = true;
        ClearRay();
        var startRayPosition = startPosition;
        float startStep = 0;
        for( int i = 0; i < 2 ; i++ )
        {
        
         var hit = Physics2D.CircleCast( startRayPosition + targetDirection.normalized * startStep, SizeBall, targetDirection, Mathf.Infinity, ~_maskaRaycast );

         Vector3 hitPoint; // = Vector3.zero;

         if( hit.collider )
         {
             hitPoint = (Vector3)hit.point;
            _hitBalls.Add(Instantiate(_prefabHitBall, (Vector3)hitPoint, Quaternion.identity));
           // DrawNormal( hitPoint, hit.normal );
         }
         else
         {
            throw new Exception("NULL collider");
            break;
         }

         var prevMagnitude = (hitPoint-currentPosition).magnitude;
         Vector3 directionRay = (hitPoint-currentPosition);

          for( int j = 0 ; j < 1000 && directionRay.magnitude > 0.1f ; j++ )
         {
            directionRay = (hitPoint-currentPosition);
           _balls.Add( Instantiate(_prefabBall, currentPosition, Quaternion.identity) );
           currentPosition += (directionRay).normalized * _step;
           if( directionRay.magnitude > prevMagnitude ) 
           {
             break;
           }
         }

         startRayPosition = hitPoint;
         targetDirection = Vector3.Reflect(targetDirection.normalized * 0.02f,hit.normal.normalized);
         startStep = _step + 1;
        }
    }

    private void StartRay()
    {
          if( Input.GetMouseButton(0) )
          {
           DrawRay();
           _isReady = true;
          }

          if( Input.GetMouseButtonUp(0) && _playingScene.PlayMode == PlayMode.Play )
          {
           if( _isReady )
           { 
              ClearRay();
              IsDraw = false;
              _direction = _target - _startBallPosition;
              _ball.BallMover.StartMove( _direction );
              enabled = false;
              _isReady = false;
           }
          }
    }

    private void Update()
    {
        StartRay();
    }

}

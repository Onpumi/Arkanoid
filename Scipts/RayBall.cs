using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayBall : MonoBehaviour
{
    [SerializeField] private Transform _prefabBall;
    [SerializeField] private Transform _prefabHitBall;
    [SerializeField] private float _step;
    [SerializeField] private int _countStep;
    [SerializeField] private LayerMask _maskaRaycast;
    [SerializeField] private Transform _ball;
    private Vector3 _prevTarget;
    private Vector3 _target;
    public bool IsDraw { get; private set; }
    private List<Transform> _balls;
    private List<Transform> _hitBalls;
    private float _scaleBall;
    private float _radiusBall;
    private float SizeBall => _scaleBall * _radiusBall;
    private Vector3 _direction;
    public event Action<Vector3> OnStartDirection;

    private void Awake()
    {
        IsDraw = false;
        _balls = new List<Transform>();
        _hitBalls = new List<Transform>();
        var collider = _ball.GetComponent<CircleCollider2D>();
        _radiusBall = collider.radius;

         if( _ball )
         {
           _scaleBall = _ball.transform.localScale.x;
         }

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

    private void DrawRay()
    {
        var startPosition = transform.position;
        var currentPosition = startPosition;
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var targetDirection = _target-startPosition;

        IsDraw = true;

         ClearBalls();
         ClearHitBalls();


       Vector3 startRayPosition = transform.position;
       float startStep = 0;

       int k = 0;

       for( int i = 0; i < 4 ; i++ )
     {
        
         //var hit = Physics2D.CircleCast( startRayPosition + Vector3.one * startStep, SizeBall, targetDirection, Mathf.Infinity, ~_maskaRaycast );
         var hit = Physics2D.CircleCast( startRayPosition + targetDirection.normalized * startStep, SizeBall, targetDirection, Mathf.Infinity, ~_maskaRaycast );

         Vector3 hitPoint = Vector3.zero;

         if( hit.collider )
         {
             hitPoint = (Vector3)hit.point;
            _hitBalls.Add(Instantiate(_prefabHitBall, (Vector3)hitPoint, Quaternion.identity));
         }
         else
         {
            Debug.Log("NULL collider");
            break;
         }

           var prevMagnitude = (hitPoint-currentPosition).magnitude;
            Vector3 directionRay = (hitPoint-currentPosition);

          while( directionRay.magnitude > 0.1f )
        {
            directionRay = (hitPoint-currentPosition);
           _balls.Add( Instantiate(_prefabBall, currentPosition, Quaternion.identity) );
           currentPosition += (directionRay).normalized * _step;
           if( directionRay.magnitude > prevMagnitude ) 
           {
             break;
           }
           if( k > 100000) { Debug.Log($"зациклено  normal:{(hitPoint-currentPosition).magnitude}"); break;}
           k++;
        }

        startRayPosition = hitPoint;
        targetDirection = Vector3.Reflect(targetDirection.normalized * 0.05f,hit.normal.normalized);
        startStep = _step + 1;
        
     }

        _prevTarget = _target;
    }

    private void Update()
    {
        if( Input.GetMouseButton(0) )
        {
            DrawRay();
        }

        if( Input.GetMouseButtonUp(0))
        {
           ClearBalls();
           ClearHitBalls(); 
           IsDraw = false;
           _direction = _target - transform.position;
           OnStartDirection?.Invoke(_direction);
           enabled = false;
        }

        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayBall : MonoBehaviour
{
    [SerializeField] private Transform _prefabBall;
    [SerializeField] private float _step;
    [SerializeField] private int _countStep;
    private Vector3 _prevTarget;
    private Vector3 _target;
    public bool IsDraw { get; private set; }
    private Transform[] _balls;



    private void Awake()
    {
        IsDraw = false;
    }

    private void DrawRay()
    {
        var boardPosition = transform.position;
        var startPosition = boardPosition;
        var currentPosition = startPosition;
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        IsDraw = true;

         if( _balls == null )
         {
          _balls = new Transform[_countStep];
         }

         if( _balls != null  )
         {
          for( int i = 0 ; i < _countStep; i++ )
          {
              if( _balls[i] != null ) 
             {
               Destroy(_balls[i].transform.gameObject);
             }
          }
         }

        for( int i = 0 ; i < _countStep ; i++ )
        {
          _balls[i] = Instantiate(_prefabBall, currentPosition, Quaternion.identity);
          currentPosition += (_target-boardPosition) * _step;
        }

        _prevTarget = _target;
    }

    private void Update()
    {
        if( Input.GetMouseButton(0) )
        {
            DrawRay();
        }

        

    }
}

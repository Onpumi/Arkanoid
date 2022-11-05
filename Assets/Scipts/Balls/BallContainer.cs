using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallContainer : MonoBehaviour 
{
    private Ball[] _balls;
    private int _maxContainerCount = 1500;
    private int _currentMaxIndex = 0;
    public int Count => _currentMaxIndex;
    public Ball this[int i] => _balls[i];


    private void Awake()
    {
        _balls = new Ball[_maxContainerCount];
    }

    public void AddBall( Ball ball )
    {
       _balls[_currentMaxIndex++] = ball;
    }

    public void DellBall( int i )
    {
    }

}

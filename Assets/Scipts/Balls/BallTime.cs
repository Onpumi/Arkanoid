using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTime 
{
    private float _prevTime;
    private float _deltaTime;

    public BallTime()
    {
        _prevTime = Time.time;
        _deltaTime = Time.fixedDeltaTime * 2;
        
    }
    public void FixedTime()
    {
        _prevTime = Time.time;
    }
    public bool isNeedTime() => ((_prevTime != 0 && Time.time-_prevTime >= _deltaTime) || _prevTime == 0 );

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTime 
{
    private float _prevTime;

    public BallTime()
    {
        _prevTime = Time.time;
    }
    public void FixedTime( float currentTime )
    {
        _prevTime = currentTime;
    }
    public bool isNeedTime() => ((_prevTime != 0 && Time.time-_prevTime >= 0.02) || _prevTime == 0 );

}

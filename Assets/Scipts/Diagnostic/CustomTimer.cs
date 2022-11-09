using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class CustomTimer : IDisposable
{

    private string m_timerName;
    private float m_numTests;
    private Stopwatch m_watch;

    public CustomTimer( string timerName, int numTests )
    {
        m_timerName = timerName;
        m_numTests = numTests;
        if( m_numTests <= 0 )
        {
            m_numTests = 1;
        }
        m_watch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        m_watch.Stop();
        float ms = m_watch.ElapsedMilliseconds;
        UnityEngine.Debug.Log(string.Format("{0} finished: {1:0.00}ms total, {2:0.000000}ms per test for {3} tests", m_timerName, ms, ms/ m_numTests, m_numTests));
        float m_countFrame = ms / (Time.fixedDeltaTime * 1000f);
        UnityEngine.Debug.Log(string.Format($"Count frames: {m_countFrame}"));
    }

}

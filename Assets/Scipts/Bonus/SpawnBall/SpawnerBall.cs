using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnerBall
{
    public SpawnerBall(  )
    {
    }
   
    public (Ball,Ball) SpawnTwoBall( Ball ball, Pool<Ball> pool, int[] angles  ) 
    {
       var ball1 = pool.Get();
       var direction1 = Quaternion.Euler(0,0,angles[0]) * ball.BallMover.Direction;
       ball1.transform.position = ball.transform.position;
       ball1.BallMover.StartMove( direction1 );
       var ball2 = pool.Get();
       var direction2 = Quaternion.Euler(0,0, angles[1]) * ball.BallMover.Direction;
       ball2.transform.position = ball.transform.position;
       ball2.BallMover.StartMove( direction2 );
       return (ball1,ball2);
    }

    public (Ball, Ball, Ball) SpawnThreeBall(Transform transform, Pool<Ball> pool, int[] angles)
    {
        var ball1 = pool.Get();
        var direction1 = Quaternion.Euler(0,0,angles[0]) * Vector2.up;
        ball1.transform.position = transform.position + transform.localScale.y * Vector3.up * 0.1f;
        ball1.BallMover.StartMove( direction1 );
        
        var ball2 = pool.Get();
        var direction2 = Quaternion.Euler(0,0,angles[1]) * Vector2.up;
        ball2.transform.position = transform.position + transform.localScale.y * Vector3.up * 0.1f;
        ball2.BallMover.StartMove( direction2 );
        
        var ball3 = pool.Get();
        var direction3 = Quaternion.Euler(0,0,angles[2]) * Vector2.up;
        ball3.transform.position = transform.position + transform.localScale.y * Vector3.up * 0.1f;
        ball3.BallMover.StartMove( direction3 );

        return (ball1, ball2, ball3);
    }
    
}

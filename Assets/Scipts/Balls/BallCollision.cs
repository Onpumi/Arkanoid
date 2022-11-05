using System;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    [SerializeField] private BallMover _ballMover;
    [SerializeField] private SoundsPlayer _soundPlayer;
    private BallTime _ballTime;
    private Vector2 _normal;

    private void Awake()
    {
        _ballTime = new BallTime();
    }
//    private void OnCollisionEnter2D( Collision2D collision )
//    {
//       if( _soundPlayer.isCanPlay )
//       {
//        _soundPlayer.PlayHitBall();
//       }
//        _normal = collision.contacts[0].normal;
//        _ballMover.Reflect( _normal );
//    }





}

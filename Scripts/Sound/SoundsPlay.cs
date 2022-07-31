using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPlay : MonoBehaviour
{
   // [SerializeField] Transform _parentBrick;
    [SerializeField] Ball _ball;
    [SerializeField] AudioClip _audioClip;
    private AudioSource _audio;
    private float _time;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        _ball.OnHitPlay += PlayHitBall;
    }

    private void OnDisable()
    {
        _ball.OnHitPlay -= PlayHitBall;
    }


    private void PlayHitBall( ) 
    {
        _audio.time = 0.3f;

       if(Time.time -_time > 0.07f)
       {
        _audio.PlayOneShot(_audioClip);
       }

       _time = Time.time;
        
    }


}

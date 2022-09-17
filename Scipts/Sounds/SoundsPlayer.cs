using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] AudioClip _audioClip;
    public static AudioSource _audio;
    private float _time;
    public bool isCanPlay => Time.time -_time > 0.03f;

  
    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.clip = _audioClip;
    }

       public void PlayHitBall() 
    {
       _audio.PlayOneShot(_audioClip);
       _time = Time.time;
    }

}

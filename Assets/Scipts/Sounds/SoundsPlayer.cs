using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] AudioClip _audioBall;
    [SerializeField] AudioClip _audioBonus;
    private AudioSource _audio;
    private float _time;
    public bool isCanPlay => Time.time -_time > 0.07f;
    
    

  
      private void Start()
    {
        _audio = GetComponent<AudioSource>();
        
    }
     public void PlayHitBall() 
    {
       _audio.PlayOneShot(_audioBall);
       _time = Time.time;
    }

     public void PlayGetBonus()
    {
      _audio.PlayOneShot(_audioBonus, 0.2f);
    }

}

using System;
using UnityEngine;

public class BuilderSound : MonoBehaviour
{
   public event Action OnSoundHit;
   private void OnCollisionEnter2D( Collision2D collision)
   {
      OnSoundHit.Invoke();
   }
}

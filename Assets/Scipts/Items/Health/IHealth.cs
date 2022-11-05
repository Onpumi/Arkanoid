using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
   bool CanTakeDamage { get; }
   void TakeDamage();
  
}

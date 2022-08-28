using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : IHealth
{
   private readonly int _maxValue;
   private readonly int _maxStartValue;
   private const int MinValue = 0;

   public int CurrentValue {get; private set; }
   public event Action OnLossHealth;

   public Health(int maxStartValue, int maxValue)
   {
      if( maxValue < maxStartValue )
      {
        throw new ArgumentException("max must be more than maxStart!");
      }

      if( maxValue < MinValue || maxStartValue < MinValue ) 
      {
        throw new ArgumentException("max and maxStart must be more than min value!");
      }

      _maxStartValue = maxStartValue;
      _maxValue = maxValue;
      CurrentValue = maxStartValue;
   }

   public bool CanTakeDamage => CurrentValue > 0;

   public void TakeDamage()
   {
      if( CurrentValue > MinValue )
      {
        CurrentValue--;
      }

      if( CurrentValue == 0)
      {
        OnLossHealth?.Invoke();
      }
   }

   public void AddValue()
   {
      if( CurrentValue >= _maxValue )
      {
        return;
      }
      else 
      {
        CurrentValue++;
      }
   }
}

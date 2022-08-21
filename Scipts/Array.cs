using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Array<T>
{   
    public T[] Normal { get; private set; } 
    public T this[int i] => (T) Normal[i];
    private int _lastIndex = -1;
    public int Count { get; private set; }


   public Array()
   {
        Normal = null;
        Count = 0;
   }
   public void Add( T normal )
   {
       _lastIndex++;
       if( Count > 0 )
       {
         T[] Out = new T[Count+1];
         for( int i = 0 ; i < Count ; i++ )
         {
            Out[i] = Normal[i];
         }
         Out[Count] = normal;
         Normal = Out;
       }
       else
       {
         Normal = new T[_lastIndex+1];
         Normal[_lastIndex] = normal;
       }
       Count++;
   }

   public void Remove( int index )
   {
      if( Normal.Length == 0 ) return;
      if( Normal.Length <= index ) return;
      var newSize = Normal.Length-1;
      var outArray = new T[newSize];
      int k = 0;
      for( int i = 0 ; i < Normal.Length; i++)
      {
         if( i != index )
         {
           outArray[k++] = Normal[i];
         }
      }
      Normal = outArray;
      Count--;
   }

   public void Clear()
   {
      Normal = null;
   }

}

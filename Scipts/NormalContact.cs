using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalContact
{   
    public Vector2 Normal { get; private set; } 
    public Collider2D Collider { get; private set; }
    
      public NormalContact( Vector2 normal, Collider2D collider )
     {
        Normal = normal;
        Collider = collider;
     }

}

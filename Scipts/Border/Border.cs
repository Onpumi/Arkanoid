using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public float MinHorizontalPosition { get; private set; }
    public float MaxHorizontalPosition { get; private set; }
    public float FrameWidth { get; private set;  }

    
    private void Awake()
    {
        MinHorizontalPosition = 0;
        MaxHorizontalPosition = 0;
        FrameWidth = transform.GetChild(0).localScale.x;
        foreach(Transform childBorder in transform)
        {
            MinHorizontalPosition = (MinHorizontalPosition > childBorder.position.x) ? (childBorder.position.x) : (MinHorizontalPosition);
            MaxHorizontalPosition = (MaxHorizontalPosition < childBorder.position.x) ? (childBorder.position.x) : (MaxHorizontalPosition);
            FrameWidth = (FrameWidth > childBorder.localScale.x) ? (childBorder.localScale.x) : (FrameWidth);
        }
        
    }


    private void Update()
    {

    }
   
}

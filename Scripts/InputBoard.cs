using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputBoard : MonoBehaviour
{
  [SerializeField] private float _speed = 1;
  public event Action OnMove;
  public event Action OnPress;
  public Vector3 TouchPosition { get; private set; }
  private float _minSpeedMouse = 0.01f;


      private void GetNewPosition()
    {
       var xPosition = transform.position.x;
       var deltaXMouse = Input.GetAxis("Mouse X");
       xPosition += deltaXMouse * _speed;
       TouchPosition = new Vector3( xPosition, transform.position.y, transform.position.z);
       OnMove?.Invoke();
    }

    private void Update()
    {
       GetNewPosition();
        if( Input.GetMouseButtonDown(0) )
        {
           OnPress?.Invoke();
        }
    }


}

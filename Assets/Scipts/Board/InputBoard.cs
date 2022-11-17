using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputBoard : MonoBehaviour
{
  private bool isPress = false;
  public event Action OnPress;

    private void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            isPress = true;
        }

        if( Input.GetMouseButtonUp(0) )
        {
           if( isPress )
           {
             OnPress?.Invoke();
           }
        }

     if( Input.GetAxis("Cancel") > 0 )
 	  {
	    Application.Quit();
	  }

    }

}

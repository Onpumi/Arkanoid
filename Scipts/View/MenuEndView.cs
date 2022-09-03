using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScreenToCanvasPositions;


public enum SelectFromLoss
{
    Repeat,
    Win,
    Next,
    Menu,
    Exit
}

public class MenuEndView : MonoBehaviour
{
    void Awake()
    {
        Canvas canvas = transform.parent.GetComponent<Canvas>();
        var positionView = new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, transform.position.z );
        transform.position = CanvasPositions.ScreenToCanvasPosition(canvas, positionView);
        transform.gameObject.SetActive(false);
    }

}

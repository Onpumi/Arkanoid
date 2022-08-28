using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewItems : MonoBehaviour
{
  //  [SerializeField] private Hearth _prefabHearth;
    [SerializeField] private Board _board;
    private Texture texture;
    private float _widthScreen;
    private float _heightScreen;


    private void Awake()
    {
    //    texture = _prefabHearth.GetComponent<SpriteRenderer>().sprite.texture;
        _widthScreen = Screen.width;
        _heightScreen  = Screen.height;
    }

    private void DrawItems()
    {

    }

    private void OnGUI()
    {
        //GUI.Box( new Rect(50,5,25,25), texture);
    }
}

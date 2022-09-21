using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseView : MonoBehaviour
{
   [SerializeField] private Transform _prefabImage;
   private Transform  _parentImage;

    private void Awake()
    {
        DisplayItem();
    }

    public void DisplayItem( )
    {
      _parentImage = this.transform;
       Instantiate( _prefabImage, _parentImage );
    }
}

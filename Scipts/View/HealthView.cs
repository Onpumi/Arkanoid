using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour, IHealthView
{
   [SerializeField] private Transform _prefabImage;
   [SerializeField] private Transform _grid;
   [SerializeField] private Transform _borderUP;
   private Transform  _parentImage;
   private List<Transform> _objectsImage;
   private float _stepDraw;


    public void DisplayItems( int count )
    {
      _parentImage = this.transform;
      _stepDraw = _prefabImage.GetComponent<RectTransform>().rect.width;

      if( _objectsImage == null )
      {
         _objectsImage = new List<Transform>();
      }

      if( _parentImage )
      {
            if( _objectsImage.Count == 0 )
          {
             for( int i = 0 ; i < count; i++)
             {
                _objectsImage.Add( Instantiate( _prefabImage, _parentImage ) );
                var screen = new Vector2 (Screen.width - Screen.width/4f + _stepDraw * i, Screen.height - _stepDraw);
               _objectsImage[i].position = Camera.main.ScreenToWorldPoint(screen);
             }
          }
          else
          {
              for( int i = 0 ; i < _objectsImage.Count ; i++ )
              {
                  _objectsImage[i].transform.gameObject.SetActive( i < count );
              }
          }
      }
    }
}

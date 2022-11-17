using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthView : MonoBehaviour, IHealthView
{
   [SerializeField] private Transform _parentHealth;
   [SerializeField] private Transform _prefabImage;
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
             var beginPositionX = Screen.width - 5 * (_stepDraw + _stepDraw * 0.5f);
             for( int i = 0 ; i < count; i++)
             {
                var hearth = Instantiate( _prefabImage, _parentImage );
                //var screen = new Vector2 (beginPositionX + (_stepDraw + _stepDraw * 0.5f) * (float)i, Screen.height - _stepDraw * 1.5f);
                var screen = new Vector2 (beginPositionX + _stepDraw  * (float)i, Screen.height - _stepDraw * 1.5f);
               var position = Camera.main.ScreenToWorldPoint(screen);
                hearth.position = new Vector3(position.x, position.y,0);
                hearth.transform.localScale = new Vector3(1,1,1);
                _objectsImage.Add( hearth );
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

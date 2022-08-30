using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour, IHealthView
{
   [SerializeField] private Transform _prefabImage;
   [SerializeField] private Transform _grid;
   private Transform  _parentImage;
   private List<Transform> _objectsImage;
   private Vector3 nextPosition( Transform prevItem ) => new Vector3( prevItem.position.x + _grid.localScale.x * 3, prevItem.position.y, prevItem.position.z );

    public void DisplayItems( int count )
    {
      _parentImage = this.transform;

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
                  if( i > 0 )
                  {
                    _objectsImage[i].position = nextPosition( _objectsImage[i-1] );
                  }
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

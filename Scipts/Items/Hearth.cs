using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hearth : MonoBehaviour
{
   [SerializeField] Transform _prefabImage;
   [SerializeField] private Transform _grid;
   private Transform  _parentImage;
   private Transform[] _objectsImage;
   private Vector3 nextPosition( Transform prevItem ) => new Vector3( prevItem.position.x + _grid.localScale.x * 3, prevItem.position.y, prevItem.position.z );

    private void Awake()
    {
      _parentImage = this.transform;
       //var item1 = Instantiate( _prefabImage, _parentImage );
       //var item2 = Instantiate( _prefabImage, _parentImage );
       //item2.position = nextPosition(item1);
       //var item3 = Instantiate( _prefabImage, _parentImage );
       //item3.position = new Vector3( item2.position.x + _grid.localScale.x * 3, item1.position.y, item1.position.z );


    }
    
    public void DisplayItems( int count )
    {

      if( _parentImage )
      {
          if( _objectsImage.Length == 0 )
          {
             for( int i = 0 ; i < count; i++)
             {
                _objectsImage[i] = Instantiate( _prefabImage, _parentImage );
                  if( i > 0 )
                  {
                    _objectsImage[i].position = nextPosition( _objectsImage[i-1] );
                  }
             }
          }
          else
          {
              for( int i = 0 ; i < _objectsImage.Length ; i++ )
              {
                _objectsImage[i].transform.gameObject.SetActive( count < _objectsImage.Length );
              }
          }
      }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewLevels : MonoBehaviour
{
   [SerializeField] int countLevels;
   [SerializeField] LevelButton _prefabButton;
   [SerializeField] float _offset;
   [SerializeField] float _space;
   [SerializeField] float _topPaddingSize;
   private TMP_Text _titleLevel;
   private Vector3 _scaleButton;
   private float _widthButton;
   private float _heightButton;
   private float _maxPositionX;
   private float _minPositionX;
   private float _minPositionY;
   private float _maxPositionY;
   private int _countX;
   private float _offsetX;
   private int _maxLevels = 100;

   private void Awake()
   {
      _scaleButton = _prefabButton.transform.localScale;
       var rect = _prefabButton.GetComponent<RectTransform>().rect;
      var countButtonRow = 3;
      var sizeButton = Screen.width / (countButtonRow+1);
      var scale = sizeButton/rect.width;
      _widthButton = rect.width * scale;
      _heightButton = rect.height * scale;
      _maxPositionX = Screen.width - _widthButton / 2f;
      _minPositionX = _widthButton / 2f;
      _maxPositionY = 0 + _heightButton / 2f + _offset;
      _minPositionY = Screen.height - _heightButton / 2f - _offset;
      var lengthX = Mathf.Round(_maxPositionX - _minPositionX);
      _countX = (int) (lengthX / (_widthButton));
      var spaceStartEnd = (Screen.width - lengthX - (_space*countButtonRow) - _widthButton / 4f  ) ;
      Vector3 position;
      var startXPosition = _minPositionX + spaceStartEnd;
      var startYPosition = _minPositionY - _topPaddingSize;
      int countRows = 0;
      int countButton = 0;
      while( countRows < _maxLevels )
      {
         for( int i = 0 ; i < _countX ; i++ )
        {
          var button = Instantiate(_prefabButton,transform);
          position = new Vector3( startXPosition + (_widthButton +_space) * i, startYPosition  ,0);
          button.transform.position = position;
          button.transform.localScale = new Vector3(scale,scale,scale);
          button.transform.GetChild(0).GetComponent<TextLevel>().InitTitle( countButton + 1 );
          countButton++;
          if( countButton >= countLevels)
          {
            return;
          }
        }
        startYPosition -= _heightButton + _space;
        countRows++;
      }
   }
}

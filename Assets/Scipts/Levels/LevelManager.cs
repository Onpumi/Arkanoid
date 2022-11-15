using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

   [SerializeField] private List<Level> _levels;
   
   private Level _currentLevel;
   private int _currentIndex = 0;

   private void OnEnable()
   {
        _currentLevel = _levels[0];

      if( _levels != null && _levels.Count > 0 )
      {
    
          foreach( Level level in _levels )
          {
            if( level == _currentLevel )
            {
               Instantiate( _currentLevel, transform );
            }
          }
      }
   }

   public Level GetCurrentLevel()
   {
       return _currentLevel;
   }

}

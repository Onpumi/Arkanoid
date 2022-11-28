using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

   [SerializeField] private List<Level> _levels;
   [SerializeField] private RayBall _rayball;
   
   private Level _currentLevel;
   private int _currentIndex = 0;

   private void Awake()
   {
      if( _levels != null && _levels.Count > 0 )
      {
          _currentLevel = Instantiate( _levels[_currentIndex], transform );
      }
   }

   public Level GetCurrentLevel()
   {
       return _currentLevel;
   }

   public void LoadLevel()
   {
     _currentLevel = Instantiate( _levels[_currentIndex], transform );
     _rayball.enabled = true;
   }

   public void NextLevel()
   {
      Destroy(_currentLevel.transform.gameObject);
       _currentIndex++;
      if( _currentIndex < _levels.Count)
      {
         LoadLevel();
      }
   }

   public void RestartLevel()
   {
      Destroy(_currentLevel.transform.gameObject);
      LoadLevel();
   }

}

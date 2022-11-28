using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayingScene : MonoBehaviour
{
    [SerializeField] LevelManager _levelManager;
    [SerializeField] private Ball   _ball;
    [SerializeField] private Transform _gameZone;
    [SerializeField] private Transform _menuZone;
    [SerializeField] private Transform _parentPopupMenuViews;
    private PlayMode _playMode;

   private void Awake()
   {
     transform.gameObject.SetActive(false);
     _playMode = PlayMode.Menu;
     UpdateScene(PlayMode.Menu);
   }


   public void StartLevel()
   {
      _playMode = PlayMode.Play;
      UpdateScene(PlayMode.Play);
   }

   private void UpdateScene( PlayMode playMode)
   {
      if( playMode == PlayMode.Menu )
      {
         _menuZone.gameObject.SetActive(true);
        _gameZone.gameObject.SetActive(false);
      }
      else if( playMode == PlayMode.Play )
      {
        _menuZone.gameObject.SetActive(false);
        _gameZone.gameObject.SetActive(true);
      }
      else
      {

      }

   }

   public void NextLevel()
   {
     _parentPopupMenuViews.gameObject.SetActive(false);
     _playMode = PlayMode.Play;
     _levelManager.NextLevel();
      UpdateScene(PlayMode.Play);
   }

   public void RestartLevel()
   {
      _parentPopupMenuViews.gameObject.SetActive(false);
      _playMode = PlayMode.Play;
      _levelManager.RestartLevel();
      UpdateScene(PlayMode.Play);
   }
}



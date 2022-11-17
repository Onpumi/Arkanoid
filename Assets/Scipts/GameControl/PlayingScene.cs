using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayingScene : MonoBehaviour
{
   [SerializeField] LevelManager _levelManager;
    [SerializeField] private Transform _gameZone;
    [SerializeField] private Transform _menuZone;
    [SerializeField] private Transform _itemsView;
    private PlayMode _playMode;

   private void Awake()
   {
     transform.gameObject.SetActive(false);
     _playMode = PlayMode.Menu;
     UpdateScene(PlayMode.Menu);
   //  DontDestroyOnLoad(this);
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
       //SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
      _itemsView.gameObject.SetActive(false);
     _levelManager.NextLevel();
      UpdateScene(PlayMode.Play);
   }
}



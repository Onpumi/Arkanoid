using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _levels;
    [SerializeField] private FactoryBalls _factoryBalls;
    [SerializeField] private PlayingScene _playingScene;


    private void Awake()
    {
      _factoryBalls.SpawnBallsToPool();
    }

    private void OnEnable()
    {
     _playButton.onClick.AddListener( delegate {OnPlayClicked( PlayMode.Play ); } );
      //_levels.onClick.AddListener( delegate {OnPlayClicked( PlayMode.Levels ); } );
    }

    private void OnDisable()
    {
      _playButton.onClick.AddListener( delegate {OnPlayClicked( PlayMode.Play); } );
      //_levels.onClick.AddListener( delegate {OnPlayClicked( PlayMode.Levels ); } );
    }

    private void OnPlayClicked( PlayMode mode )
    {
        _playingScene.StartLevel();
    }    

}

public enum PlayMode
{
   Menu,
   Play,
   Levels
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _levels;
    [FormerlySerializedAs("_factoryBalls")] [SerializeField] private ContainerBalls _containerBalls;
    [SerializeField] private PlayingScene _playingScene;


    private void Awake()
    {
      _containerBalls.SpawnBallsToPool();
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
   Frozen,
   Levels
}

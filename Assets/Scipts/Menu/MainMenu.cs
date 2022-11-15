using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _levels;
    [SerializeField] private FactoryBalls _factoryBalls;
    [SerializeField] private Transform _gameZone;
    [SerializeField] private Transform _menuZone;


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

        _menuZone.gameObject.SetActive(false);
        _gameZone.gameObject.SetActive(true);
    }    


}

public enum PlayMode
{
   Menu,
   Play,
   Levels
}

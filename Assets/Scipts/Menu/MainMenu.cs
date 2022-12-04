using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _levels;
    [SerializeField] private PlayingScene _playingScene;
    private void OnEnable()
    {
     _playButton.onClick.AddListener( delegate {OnPlayClicked( PlayMode.Play ); } );
    }

    private void OnDisable()
    {
      _playButton.onClick.AddListener( delegate {OnPlayClicked( PlayMode.Play); } );
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

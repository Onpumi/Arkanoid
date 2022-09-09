using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _levels;
    [SerializeField] private SceneLoader _sceneLoader;

    private void OnEnable()
    {
      _playButton.onClick.AddListener( delegate {OnPlayClicked(2); } );
      _levels.onClick.AddListener( delegate {OnPlayClicked(1); } );
    }

    private void OnDisable()
    {
      _playButton.onClick.AddListener( delegate {OnPlayClicked(2); } );
      _levels.onClick.AddListener( delegate {OnPlayClicked(1); } );
    }

    private void OnPlayClicked( int numberLevel )
    {
        _sceneLoader.LoadScene( numberLevel );
    }    


}

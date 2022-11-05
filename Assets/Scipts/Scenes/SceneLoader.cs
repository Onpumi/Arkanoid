
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] StatusesGame _statusesGame;
    public int CurrentScene { get; private set; }
    public bool isLoad = false;
    

    private void OnEnable()
    {
       SceneManager.sceneLoaded += LoadActive;
    }

    private void OnDisable()
    {
       SceneManager.sceneLoaded -= LoadActive;
    }

    private void LoadActive( Scene scene, LoadSceneMode mode )
    {
      isLoad = true;
    }

    public void RestartLevel( )
    {
     // game.SetStatusRun( true );
      SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ); 
    }

    public void LoadScene( int numberScene )
    {
      CurrentScene = numberScene;
      SceneManager.LoadScene( numberScene );
    }

    public void NextLevel( GameControl game )
    {
      game.SetStatusRun( true );
      SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 ); 
    }
}

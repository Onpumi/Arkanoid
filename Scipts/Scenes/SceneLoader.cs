
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
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

    public void RestartLevel()
    {
      SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ); 
    }

    public void LoadScene( int numberScene )
    {
      CurrentScene = numberScene;
      SceneManager.LoadScene( numberScene );
    }

    public void NextLevel()
    {
      SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 ); 
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public int CurrentScene { get; private set; }

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

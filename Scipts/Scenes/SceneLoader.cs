
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
   [SerializeField] FabrikaBalls _factory;
    public int CurrentScene { get; private set; }

    private void OnEnable()
    {
       _factory.OnDestroyBall += RestartLevel;
    }

     private void OnDisable()
    {
       _factory.OnDestroyBall -= RestartLevel;
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
}

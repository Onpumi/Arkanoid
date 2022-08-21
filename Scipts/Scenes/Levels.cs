using UnityEngine.SceneManagement;

public class Levels : GameScene
{

    private const string _sceneNamePrefix = "Level";

    public static void LoadLevel( int numberLevel, LoadSceneMode loadSceneMode = LoadSceneMode.Single )
    {
        LoadScene( _sceneNamePrefix + numberLevel, loadSceneMode);
    }

}

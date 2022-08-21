using UnityEngine;
using UnityEngine.SceneManagement;

    public abstract class GameScene
    {
        protected static AsyncOperation LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

    }
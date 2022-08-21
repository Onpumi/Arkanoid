using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    public event Action OnPlayButtonClicked;

    public static SceneLoader _sceneLoader;

    private void OnEnable()
    {
      _playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnDisable()
    {
      _playButton.onClick.AddListener(OnPlayClicked);
    }

    void Start()
    {
        _sceneLoader = new SceneLoader();
    }

    private void OnPlayClicked()
    {
        //_sceneLoader.LoadScene(1);
        //SceneManager.LoadScene(1);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }    


}

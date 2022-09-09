using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    private int _numberButton;
    private Button _playButton;
    public event Action OnPlayButtonClicked;
    private void Awake()
    {
        _playButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(LoadLevel);
    }

    private void Start()
    {
        _numberButton = transform.GetChild(0).GetComponent<TextLevel>().GetNumberScene();
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(_numberButton+1);
    }


 
}

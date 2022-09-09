using System;
using TMPro;
using UnityEngine;

public class TextLevel : MonoBehaviour
{
    private TMP_Text _title;
    private void Awake()
    {
       _title = GetComponent<TMP_Text>();
    }

    public void InitTitle( int value )
    {
        _title.text = value.ToString();
    }

    public int GetNumberScene()
    {
        return Int32.Parse(_title.text);
    }
}

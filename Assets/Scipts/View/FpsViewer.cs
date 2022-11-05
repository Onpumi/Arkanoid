using TMPro;
using UnityEngine;

public class FpsViewer : MonoBehaviour
{
   private float _fps;
   private TMP_Text _fpsTmpText;
   private float _prevTime;

    private void Awake()
    {
        _fpsTmpText = GetComponent<TMP_Text>();
        _prevTime = Time.time;
    } 
    private void Update()
    {
        if( Time.time-_prevTime >= 1 )
        {
           _fps = 1f/Time.deltaTime;
           _fpsTmpText.text = _fps.ToString();
           _prevTime = Time.time;
        }
    }
}

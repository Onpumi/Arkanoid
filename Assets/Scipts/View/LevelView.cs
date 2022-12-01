using UnityEngine;
using TMPro; 
using System.Text;

public class LevelView : MonoBehaviour
{
    private TMP_Text _tmpText;
    private StringBuilder _stringBuilder;
    private const string Title = "Level:";


    private void Awake()
    {
        _stringBuilder = new StringBuilder(); 
        _tmpText = GetComponent<TMP_Text>();
        _tmpText.text = Title;
    }

    public void DisplayLevel( int numberLevel )
    {
        _stringBuilder.Clear();
        _stringBuilder.Append(Title).Append(numberLevel);
        _tmpText.text = _stringBuilder.ToString();
    }

}

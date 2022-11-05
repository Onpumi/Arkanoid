using TMPro;
using UnityEngine;

public class CountBallsView : MonoBehaviour
{
   [SerializeField] private FactoryBalls _factoryBalls;
   private TMP_Text _tmpText;
   private void Awake()
   {
     _tmpText = GetComponent<TMP_Text>();
     UpdateDisplayCount();
   }

   private void OnEnable()
   {
     _factoryBalls.OnUpdateCountShowBall += UpdateDisplayCount;
   }

   private void OnDisable()
   {
     _factoryBalls.OnUpdateCountShowBall -= UpdateDisplayCount;
   }


   private void UpdateDisplayCount( )
   {
     _tmpText.text = _factoryBalls.CountBalls.ToString();
   }


}

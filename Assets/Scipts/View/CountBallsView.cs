using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CountBallsView : MonoBehaviour
{
   [FormerlySerializedAs("_factoryBalls")] [SerializeField] private ContainerBalls _containerBalls;
   private TMP_Text _tmpText;
   private void Awake()
   {
     _tmpText = GetComponent<TMP_Text>();
     UpdateDisplayCount();
   }

   private void OnEnable()
   {
     _containerBalls.OnUpdateCountShowBall += UpdateDisplayCount;
   }

   private void OnDisable()
   {
     _containerBalls.OnUpdateCountShowBall -= UpdateDisplayCount;
   }


   private void UpdateDisplayCount( )
   {
     _tmpText.text = _containerBalls.CountBalls.ToString();
   }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridBalls : MonoBehaviour, IPointerDownHandler
{
   [SerializeField] private FactoryBalls _factory;
   [SerializeField] private Ball _ball;
  public void OnPointerDown(PointerEventData eventData)
 {
    
 }

}

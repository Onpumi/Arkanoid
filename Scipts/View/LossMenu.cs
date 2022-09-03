using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemMenu : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private SelectFromLoss _changeFromLoss;
    public event Action<SelectFromLoss> OnChangeFromLoss;
    public SelectFromLoss ChangeFromLoss => _changeFromLoss;
       public void OnPointerDown(PointerEventData events)
	 {
		OnChangeFromLoss?.Invoke( ChangeFromLoss );
	 }

}

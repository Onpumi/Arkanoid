using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class TestSO : ScriptableObject
{
    [SerializeField] public BonusBall[] _bonusBalls;
    [SerializeField] private GameControl _gameControl; // это для теста
    public float test = 5;


    public void Show()
    {
        Debug.Log(_bonusBalls.Length);
    }
}

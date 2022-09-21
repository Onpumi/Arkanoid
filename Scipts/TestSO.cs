using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class TestSO : ScriptableObject
{
    [SerializeField] public BonusBall[] _bonusBalls;
    public float test = 5;


    public void Show()
    {
        Debug.Log(_bonusBalls.Length);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Color _color;
  private void Awake()
  {
    GetComponent<SpriteRenderer>().color = _color;
  }
}

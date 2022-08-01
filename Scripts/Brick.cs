using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Brick : MonoBehaviour
{
  [SerializeField] Ball _ball;

  private void OnEnable()
  {
    _ball.OnDestroyBlock += DestroyBlock;
  }

  private void OnDisable()
  {
    _ball.OnDestroyBlock -= DestroyBlock;
  }

  private void DestroyBlock( GameObject block )
  {

      Destroy(block);
  }
}

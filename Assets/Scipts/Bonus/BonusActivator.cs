using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusActivator : MonoBehaviour
{
   [SerializeField] private Board _board;
   [SerializeField] private Ball _ball;
   [SerializeField] private FactoryBalls _factoryBalls;

   private void OnEnable()
   {
     _board.OnReproductionTwo += DoReproductionTwo;
   }

   private void OnDisable()
   {
      _board.OnReproductionTwo -= DoReproductionTwo;
   }

   private void DoReproductionOne()
   {
     //_factoryBalls.SpawnBall( _ball, 2);
   }

   private void DoReproductionTwo()
   {
       // float[] angles = {-10,0,10};
     //  _factoryBalls.SpawnBall( _ball, 3, this.transform, angles );
   }
}

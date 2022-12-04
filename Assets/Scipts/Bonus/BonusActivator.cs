using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BonusActivator : MonoBehaviour
{
   [SerializeField] private Board _board;
   [SerializeField] private Ball _ball;
   [FormerlySerializedAs("_factoryBalls")] [SerializeField] private ContainerBalls _containerBalls;

   private void OnEnable()
   {
     //_board.OnReproductionTwo += DoReproductionTwo;
   }

   private void OnDisable()
   {
      //_board.OnReproductionTwo -= DoReproductionTwo;
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

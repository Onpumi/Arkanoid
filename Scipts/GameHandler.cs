using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
   
   [SerializeField] Transform _ballsParent;
   [SerializeField] Board _board;
   [SerializeField] FabrikaBalls _factoryBalls;
   [SerializeField] Ball _prefabBall;


   public Vector3 StartPositionBoard { get; private set; }
   public Vector3 StartPositionBall { get; private set; }

   
   private void Awake()
   {
      StartPositionBall = _ballsParent.transform.GetChild(0).transform.position;
      StartPositionBoard = _board.transform.position;
   }

   private void OnEnable()
   {
      _factoryBalls.OnLossAllBalls += ResetGame;
   }

   private void OnDisable()
   {
      _factoryBalls.OnLossAllBalls -= ResetGame;
   }

   private void ResetGame()
   {
       _board.transform.position = StartPositionBoard;
       Ball ball = _factoryBalls.SpawnBall();
       ball.StateStart();
       var rayball =_board.GetComponent<RayBall>();
       rayball.enabled = true;
   }


}

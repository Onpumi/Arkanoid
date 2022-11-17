using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


public class GameControl : MonoBehaviour
{
   [SerializeField] private  Transform _ballsParent;
   [SerializeField] private Ball _ball;
   [SerializeField] private  MenuEndView _lossView;
   [SerializeField] private  MenuEndView _winView;
   [SerializeField] private  Board _board;
   [SerializeField] private  Bricks _bricks;
   [SerializeField] private  FactoryBalls _factoryBalls;
   [SerializeField] private  PlayingScene _playingScene;
   [SerializeField] private  StatusesGame _statusesGame;
   //private IStateGame _stateGame;
   private List<ItemMenu> itemsMenu = new List<ItemMenu>();
   private RayBall _rayball;
   public Vector3 StartPositionBoard { get; private set; }
   public Vector3 StartPositionBall { get; private set; }

   
   private void Awake()
   {
      StartPositionBall = _ballsParent.transform.GetChild(0).transform.position;
      StartPositionBoard = _board.transform.position;
      _rayball =_board.GetComponent<RayBall>();
  //    _stateGame = _statusesGame.RunningGame;

//      Application.targetFrameRate = 30;

      //Debug.Log(Application.targetFrameRate);
      //DontDestroyOnLoad(this);
   }

   private void OnEnable()
   {
      _factoryBalls.OnLossAllBalls += ResetStateGame;
      _board.OnLostAll += FrozeLevel;
      _bricks.OnDestroyAllBricks += FrozeLevel;
      SignTheView( _lossView );
      SignTheView( _winView );
   }

   private void OnDisable()
   {
      _factoryBalls.OnLossAllBalls -= ResetStateGame;
      _board.OnLostAll -= FrozeLevel;
      _bricks.OnDestroyAllBricks -= FrozeLevel;
      UnsubscribeViews();
   }

   private void SignTheView( MenuEndView viewFinishMenu )
   {
       int index;
       index = itemsMenu.Count;

       foreach( Transform item in viewFinishMenu.transform )
      {
          var itemMenu = item.GetComponent<ItemMenu>();
         if( itemMenu != null )
         {
           itemsMenu.Add( itemMenu );
           itemsMenu[index++].OnChangeFromLoss += ChangeAfterLost;
         }
      }
   }

   private void UnsubscribeViews( )
   {
       foreach( ItemMenu itemMenu in itemsMenu )
      {
           itemMenu.OnChangeFromLoss -= ChangeAfterLost;
      }
   }

   

   

   private void ResetStateGame()
   {
       _board.transform.position = StartPositionBoard;
       Ball ball = _factoryBalls.SpawnBall();
     //  ball.StateStart();
       _rayball.enabled = true;
   }

   private void RestartLevel()
   {
      //_sceneLoader.RestartLevel( );
   }

   private void FrozeLevel( MenuEndView view )
   {
//      _ballsParent?.gameObject.SetActive(false);
      view.transform.gameObject.SetActive(true);
      if( _rayball )
      {
       _rayball.enabled = false;
      }
      
   }


   private void ChangeAfterLost( SelectFromLoss change )
   {

      if( change == SelectFromLoss.Repeat )
      {
          RestartLevel(); 
      }
      else if( change == SelectFromLoss.Menu)
      {
        // _sceneLoader.LoadScene(0);
      }
      else if( change == SelectFromLoss.Exit)
      {
        Application.Quit();
      }
      else if( change == SelectFromLoss.Next)
      {
         _rayball.enabled = true;
         _factoryBalls.ReturnPoolAllBalls();
         _ball = _factoryBalls.SpawnBall();
         _playingScene.NextLevel();

      }

   }




   private void Update()
   {
       if( Input.GetAxis("Cancel") > 0 )
       {
         FrozeLevel( _lossView );
       }
   }
}

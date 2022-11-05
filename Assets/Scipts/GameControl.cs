using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class GameControl : MonoBehaviour
{
   [SerializeField] Transform _ballsParent;
   [SerializeField] SceneLoader _sceneLoader;
   [SerializeField] MenuEndView _lossView;
   [SerializeField] MenuEndView _winView;
   [SerializeField] Board _board;
   [SerializeField] Bricks _bricks;
   [SerializeField] FactoryBalls _factoryBalls;
   [SerializeField] StatusesGame _statusesGame;
   private IStateGame _stateGame;
   private List<ItemMenu> itemsMenu = new List<ItemMenu>();
   private RayBall _rayball;
   public Vector3 StartPositionBoard { get; private set; }
   public Vector3 StartPositionBall { get; private set; }

   
   private void Awake()
   {
      StartPositionBall = _ballsParent.transform.GetChild(0).transform.position;
      StartPositionBoard = _board.transform.position;
      _rayball =_board.GetComponent<RayBall>();
      _stateGame = _statusesGame.RunningGame;

//      Application.targetFrameRate = 30;

      //Debug.Log(Application.targetFrameRate);
   }

   private void OnEnable()
   {
      _factoryBalls.OnLossAllBalls += ResetGame;
      _board.OnLostAll += FrozeLevel;
      _bricks.OnDestroyAllBricks += FrozeLevel;
      SignTheView( _lossView );
      SignTheView( _winView );
   }

   private void OnDisable()
   {
      _factoryBalls.OnLossAllBalls -= ResetGame;
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

   public void SetStatusRun( bool isRun )
   {
      if( isRun )
      {
        _stateGame = _statusesGame.RunningGame;
      }
      else
      {
        _stateGame = _statusesGame.FrozenGame;
      }
   }

   

   private void ResetGame()
   {
       _board.transform.position = StartPositionBoard;
       Ball ball = _factoryBalls.SpawnBall();
     //  ball.StateStart();
       _rayball.enabled = true;
   }

   private void RestartLevel()
   {
      _sceneLoader.RestartLevel( );
   }

   private void FrozeLevel( MenuEndView view )
   {
      _ballsParent?.gameObject.SetActive(false);
      view.transform.gameObject.SetActive(true);
      if( _rayball )
      {
       _rayball.enabled = false;
      }
      SetStatusRun( false );
   }


   private void ChangeAfterLost( SelectFromLoss change )
   {

      if( change == SelectFromLoss.Repeat )
      {
          RestartLevel(); 
      }
      else if( change == SelectFromLoss.Menu)
      {
         _sceneLoader.LoadScene(0);
      }
      else if( change == SelectFromLoss.Exit)
      {
        Application.Quit();
      }
      else if( change == SelectFromLoss.Next)
      {
         _sceneLoader.NextLevel( this );
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

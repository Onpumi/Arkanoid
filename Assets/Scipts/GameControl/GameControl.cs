using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameControl : MonoBehaviour
{
   [SerializeField] private  Transform _ballsParent;
   [SerializeField] private Transform _textCountBalls;
   [SerializeField] private Ball _ball;
   [SerializeField] private  MenuEndView _lossView;
   [SerializeField] private  MenuEndView _winView;
   [SerializeField] private  Board _board;
   [SerializeField] private  Bricks _bricks;
   [SerializeField] private  ContainerBalls _containerBalls;
   [SerializeField] private  PlayingScene _playingScene;
   [SerializeField] private  StatusesGame _statusesGame;
   private List<ItemMenu> itemsMenu = new List<ItemMenu>();
   private RayBall _rayball;
   public Vector3 StartPositionBoard { get; private set; }
   public Vector3 StartPositionBall { get; private set; }

   
   private void Awake()
   {
      StartPositionBall = _ballsParent.transform.GetChild(0).transform.position;
      StartPositionBoard = _board.transform.position;
      _rayball =_board.GetComponent<RayBall>();
   }

   private void OnEnable()
   {
      _containerBalls.OnLossAllBalls += RestartBalls;
      _board.OnLostAll += FrozeLevel;
      _bricks.OnDestroyAllBricks += FrozeLevel;
      SignTheView( _lossView );
      SignTheView( _winView );
   }

   private void OnDisable()
   {
      _containerBalls.OnLossAllBalls -= RestartBalls;
      _board.OnLostAll -= FrozeLevel;
      _bricks.OnDestroyAllBricks -= FrozeLevel;
      UnsubscribeViews();
   }

    private void SignTheView( MenuEndView viewFinishMenu )
   {
     var index = itemsMenu.Count;
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
     foreach( var itemMenu in itemsMenu ) itemMenu.OnChangeFromLoss -= ChangeAfterLost;
  }

   private void FrozeLevel( MenuEndView view )
   {
      view.transform.parent.gameObject.SetActive(true);
      view.transform.gameObject.SetActive(true);
      _rayball.enabled = false;
//      _bricks.DisableBonuses();
      _bricks.enabled = false;
      _containerBalls.ReturnPoolAllBalls();
      _containerBalls.ResetBalls(); 
      _containerBalls.enabled = false;
      _board.enabled = false;
      _playingScene.FrozePlay();
      
   }

   private void RestartBalls()
   {
     _containerBalls.enabled = true;
   }

   private void ChangeAfterLost( SelectFromLoss change )
   {

      if( change == SelectFromLoss.Repeat )
      {
          RestartBalls();
           _playingScene.RestartLevel();
           _bricks.enabled = true;
           _board.enabled = true;
           _textCountBalls.gameObject.SetActive(true);
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
          RestartBalls();
          _playingScene.NextLevel();
          _bricks.enabled = true;
          _board.enabled = true;
          _textCountBalls.gameObject.SetActive(true);
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

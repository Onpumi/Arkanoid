using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
   
   [SerializeField] Transform _ballsParent;
   [SerializeField] SceneLoader _sceneLoader;
   [SerializeField] MenuEndView _lossView;
   [SerializeField] MenuEndView _winView;
   [SerializeField] Board _board;
   [SerializeField] Bricks _bricks;
   [SerializeField] FabrikaBalls _factoryBalls;
   [SerializeField] Ball _prefabBall;
   private List<ItemMenu> itemsMenu = new List<ItemMenu>();
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


   void SignTheView( MenuEndView viewFinishMenu )
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

   void UnsubscribeViews( )
   {
       foreach( ItemMenu itemMenu in itemsMenu )
      {
           itemMenu.OnChangeFromLoss -= ChangeAfterLost;
      }
   }


   private void ResetGame()
   {
       _board.transform.position = StartPositionBoard;
       Ball ball = _factoryBalls.SpawnBall();
       ball.StateStart();
       var rayball =_board.GetComponent<RayBall>();
       rayball.enabled = true;
   }

   private void RestartLevel()
   {
      _sceneLoader.RestartLevel();
   }

   private void FrozeLevel( MenuEndView view, Transform sendObject )
   {
      _ballsParent.gameObject.SetActive(false);
     if( sendObject != null )
     {
      sendObject.gameObject.SetActive(false);
     }
      view.transform.gameObject.SetActive(true);
   }


   private void ChangeAfterLost( SelectFromLoss change )
   {
     // _bricks.enabled = false;

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

   }


}

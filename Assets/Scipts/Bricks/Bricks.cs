using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bricks : MonoBehaviour
{
    [SerializeField] BonusBall[] _bonusPrefabs;
    [SerializeField] ContainerBalls _containerBalls;
    [SerializeField] Board _board;
    [SerializeField] MenuEndView _winView;
    [SerializeField] LevelManager _levelManager;
    private List<Brick> _bricks;
    private List<BonusBall> _bonuses;
    private int _countBricks;
    private List<Brick> _bricksBreak;
    public event Action<MenuEndView> OnDestroyAllBricks;
    

    private void OnEnable()
    {
        LoadBricks();
        InitBonuses();
      _containerBalls.OnLossAllBalls += UpdateBonuses;
      foreach ( var bonus in _bonuses )
      {
          bonus.OnReproductionOne += _containerBalls.SpawnAllBalls;
          bonus.OnReproductionTwo += _containerBalls.SpawnThreeBalls;
      }
    }

   
    public void Restart()
    {
        LoadBricks();
        InitBonuses();
    }

    private void OnDisable()
    {
        foreach ( var bonus in _bonuses )
        {
            bonus.OnReproductionOne -= _containerBalls.SpawnAllBalls;
            bonus.OnReproductionTwo -= _containerBalls.SpawnThreeBalls;
        }
        _containerBalls.OnLossAllBalls -= UpdateBonuses;
        DisableBonuses();
        _bricksBreak.Clear();
        _bricks.Clear();
    }

    private void LoadBricks()
    {
        Transform transformBricks;
         transformBricks = transform.GetChild(transform.childCount-1);
        var count = transformBricks.childCount;
        _countBricks = 0;
        _bricksBreak ??= new List<Brick>();
        _bricks ??= new List<Brick>();

        foreach( Transform transformBrick in transformBricks )
        {
            if( transformBrick.TryGetComponent(out Brick brick))
            {
                _bricksBreak.Add( brick );
                _countBricks++;
            }
        }

        foreach( Brick brick in _bricksBreak )
        {
           _bricks.Add( brick );
        }
        
        _bonuses ??= new List<BonusBall>();
        _bonuses.Clear();
    }
  
    private void SetColor( Transform transformBricks, Color color, int index )
    {
        _bricks[index] = transformBricks.GetChild(index).GetComponent<Brick>();
        _bricks[index].SetColor( color );
    }

    private void InitBonuses()
    {
       var currentLevel = _levelManager.GetCurrentLevel();
       if( _bricksBreak != null )
       {
        foreach( var bonus in _bonusPrefabs )
        {
           for( int i = 0 ; i < bonus.Count ; i++ )
           {
              bonus.SetCount( currentLevel );
              var index = UnityEngine.Random.Range(0,_bricksBreak.Count-1);
              PutBonusInBrick( bonus, index );
           }
        }
       }
    }

    private void UpdateBonuses()
    {
        foreach( var bonus in _bonuses )
        {
            if( bonus != null && bonus.IsOpen )
            {
              bonus.transform.gameObject.SetActive(false);
            }
        }
    }

    public void DisableBonuses()
    {
        foreach( var bonus in _bonuses )
        {
            bonus.DisableBonus();
        }
    }


    public void UpdateBricks()
    {
        _countBricks--;
        if( _countBricks <= 0 )
        {
          OnDestroyAllBricks?.Invoke(_winView);
        }
    }

    private void PutBonusInBrick( BonusBall bonus, int indexBrick )
    {

        if( _bricksBreak == null || _bricksBreak.Count == 0 ) { return; }
          while(  indexBrick < _bricksBreak.Count   )
          {
              if (_bricksBreak[indexBrick].IsNull != true) indexBrick++;
              else break;
          }
        if( indexBrick < _bricksBreak.Count )
        {
            _bricksBreak[indexBrick].SpawnBonus( bonus );
            _bonuses.Add( _bricksBreak[indexBrick].GetBonus() );
        }
    }

}

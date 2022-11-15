using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    [SerializeField] BonusBall[] _bonusPrefabs;
    [SerializeField] FactoryBalls _factoryBalls;
    [SerializeField] Board _board;
    [SerializeField] MenuEndView _winView;
    [SerializeField] LevelManager _levelManager;
    private Brick[] _bricks;
    private List<BonusBall> _bonuses;
    private int _countBricks;
    public event Action<MenuEndView> OnDestroyAllBricks;
    [SerializeField] private TestSO _testSO;
    
    private void Start()
    {

        Transform transformBricks = transform.GetChild(0).GetChild(0).GetChild(0);

            
        _countBricks = transformBricks.childCount;
        var count = transformBricks.childCount;
        _countBricks = 0;


        List<Brick> bricksBreak = new List<Brick>();

        foreach( Transform transformBrick in transformBricks )
        {
            if( transformBrick.TryGetComponent(out Brick brick))
            {
                bricksBreak.Add( brick );
                _countBricks++;
            }
        }

        _bricks = new Brick[_countBricks];
        int index = 0;
        foreach( Brick brick in bricksBreak )
        {
            _bricks[index++] = brick;
        }
        


        _bonuses = new List<BonusBall>();
        Color cls = new Color( 0f,1f,0.5f, 1f);
       if( _countBricks > 0 )
       {
     //   _bricks = new Brick[_countBricks];
        for( int i = 0 ; i < _countBricks ; i++ )
        {
            //_bricks[i] = transformBricks.GetChild(i).GetComponent<Brick>();
//            _bricks[i].transform.GetComponent<SpriteRenderer>().color = cls;
        }
       }

        InitBonuses();
    }


    private void OnEnable()
    {
        _factoryBalls.OnLossAllBalls += UpdateBonuses;
    }

    private void OnDisable()
    {
        _factoryBalls.OnLossAllBalls -= UpdateBonuses;
    }



    private void InitBonuses()
    {
       var currentLevel = _levelManager.GetCurrentLevel();
       if( _bricks != null )
      {
        foreach( var bonus in _bonusPrefabs )
        {
           for( int i = 0 ; i < bonus.Count ; i++ )
           {
              bonus.SetCount( currentLevel );
              var index = UnityEngine.Random.Range(0,_bricks.Length);
              GiveBonus( bonus, index );
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

    public void UpdateBricks()
    {
        _countBricks--;
        if( _countBricks <= 0 )
        {
              OnDestroyAllBricks?.Invoke(_winView);
        }

       // Debug.Log( _countBricks );
    }

    private void GiveBonus( BonusBall bonus, int indexBrick )
    {
        if( _bricks == null || _bricks.Length == 0 ) { return; }
        while( _bricks[indexBrick].IsNull != true && indexBrick < _bricks.Length )
        {
            indexBrick++;
        }
        if( indexBrick < _bricks.Length )
        {
           _bricks[indexBrick].InitBonus( bonus );
           _bonuses.Add( _bricks[indexBrick].GetBonus() );
        }
    }


}

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
    private Brick[] _bricks;
    private List<BonusBall> _bonuses;
    private int _countBricks;
    public event Action<MenuEndView> OnDestroyAllBricks;
    
    
    private void Awake()
    {
        _countBricks = transform.childCount;
        _bricks = new Brick[_countBricks];
        _bonuses = new List<BonusBall>();
        Color cls = new Color( 0f,1f,0.5f, 1f);
     
        for( int i = 0 ; i < _countBricks ; i++ )
        {
            _bricks[i] = transform.GetChild(i).GetComponent<Brick>();
            _bricks[i].transform.GetComponent<SpriteRenderer>().color = cls;
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
        foreach( var bonus in _bonusPrefabs )
        {
           for( int i = 0 ; i < bonus.Count ; i++ )
           {
              var index = UnityEngine.Random.Range(0,_bricks.Length);
              GiveBonus( bonus, index );
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
            if( _winView != null )
            {
              OnDestroyAllBricks?.Invoke(_winView);
            }
        }
    }

    private void GiveBonus( BonusBall bonus, int indexBrick )
    {
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

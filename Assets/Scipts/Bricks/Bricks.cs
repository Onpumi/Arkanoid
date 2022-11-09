using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    [SerializeField] BonusBall[] _bonusPrefabs;
  //  [SerializeField] Transform _bonusParentPrefab;
    [SerializeField] FactoryBalls _factoryBalls;
    [SerializeField] Board _board;
    [SerializeField] MenuEndView _winView;
    private Brick[] _bricks;
    private List<BonusBall> _bonuses;
    private int _countBricks;
    public event Action<MenuEndView> OnDestroyAllBricks;
    //private BonusBall[] _bonusBalls;
    [SerializeField] private TestSO _testSO;
    
    private void Awake()
    {

        _countBricks = transform.childCount;
        _bonuses = new List<BonusBall>();
        Color cls = new Color( 0f,1f,0.5f, 1f);
       if( _countBricks > 0 )
       {
        _bricks = new Brick[_countBricks];
        for( int i = 0 ; i < _countBricks ; i++ )
        {
            _bricks[i] = transform.GetChild(i).GetComponent<Brick>();
//            _bricks[i].transform.GetComponent<SpriteRenderer>().color = cls;
        }
       }

        InitBonuses();
    }

    private void Start()
    {
      //  Debug.Log(_testSO._bonusBalls.Length);

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
       if( _bricks != null )
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

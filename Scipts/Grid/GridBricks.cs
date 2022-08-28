using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class GridBricks : MonoBehaviour
{
    [SerializeField] private Ball _ball;
  //  [SerializeField] private BallMover _ballmover;
    [SerializeField] private GridLayout _gridTiles;
    [SerializeField] private Grid _grid;
    [SerializeField] private Transform _tileMapObject;
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private Sprite[] _sprites;
    private BoundsInt _bounds;
    private TileBase[] _tiles;
    private Collider2D _tilemapCollider;
    private Dictionary<Vector3,Vector3Int> _cells;

    [SerializeField] Transform test;

    //private BoundsInt cellBounds;

    private void Awake()
    {
      InitColliders();
    }

/*
  private void Start()
  {
 //   _tilemapCollider = GetComponent<Collider2D>();
//    var boundsTile = _tilemapCollider.bounds;



//var other = _tilemapCollider;

 var bounds = new BoundsInt
                        (
                          _gridTiles.WorldToCell(other.bounds.min),
                          _gridTiles.WorldToCell(other.bounds.size) + new Vector3Int(0,0,1)
                        );

//    BoundsInt bounds = new BoundsInt( new Vector3Int( (int)boundsTile.min.x, (int)boundsTile.min.y, 0 ), new Vector3Int( (int)boundsTile.size.x, (int)boundsTile.size.y, 0)   );

//    BoundsInt bounds = new BoundsInt( , new Vector3Int( (int)boundsTile.size.x, (int)boundsTile.size.y, 0)   );


    var tiles = _tileMap.GetTilesBlock(bounds);

//    Debug.Log(boundsTile.min);
   // test.position = bounds.min;

int count = 0;

var tilePositions = bounds.allPositionsWithin;

Vector3Int[,] cellPositions = new Vector3Int[bounds.size.x,bounds.size.y];

//Debug.Log(tiles[0].position);

    for( int x = 0 ; x < bounds.size.x; x++)
    {
        for( int y = 0 ; y < bounds.size.y; y++)
        {
          TileBase tile = tiles[x + y * bounds.size.x];
          if( tile != null  )
          {
            Vector3Int position = new Vector3Int(x,y,1);
            cellPositions[x,y] = tilePositions.Current;
          }
          tilePositions.MoveNext();
        }
//        tilePositions.MoveNext();
    }
      transform.gameObject.SetActive(false);
  }

*/


 private bool isDeleteTile( Sprite[] sprites, Vector3Int position )
 {
    for( int i = 0 ; i < sprites.Length ; i++)
    {
       if( sprites[i] == _tileMap.GetSprite(position))
       {
         return false;
       }
    }
    return true;
 }


   public void OnMouseDown()
   {
  
  //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      //Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);

    if( _ball.IsMove == false )
    {
      //_ball.StartMove();
    }
          //_ballmover.SetDirection((Vector2)worldPoint - (Vector2)_ball.transform.position);

   }


   private void Update()
   {

 //   _gridTiles.WorldToCell(other.bounds.min)


      if( Input.GetMouseButtonDown(0) )
      {
          var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          //test.transform.position = 
          var v = new Vector3( position.x, position.y, 1 );

          Debug.Log($"координаты {_gridTiles.WorldToCell(position)} ");
          _tileMap.SetTile(_gridTiles.WorldToCell(v),null);
      }
   }


  private void InitColliders()
  {
     var bounds = new BoundsInt
                        (
                          new Vector3Int(-1000,0,0),
                          new Vector3Int(1000,1000,0) + new Vector3Int(0,0,1)
                        );



    bounds = _tileMap.cellBounds;


     var tiles = _tileMap.GetTilesBlock(bounds);
    foreach( var cell in bounds.allPositionsWithin )
    {
       if(_tileMap.HasTile(cell))
       {
          if(_tileMap.GetTile(cell))
          {
              //test.transform.position = _gridTiles.CellToWorld(cell);
              Instantiate(test,_gridTiles.CellToWorld(cell), Quaternion.identity);
          }
       }
    }
  }


   public void FindCellToDel( Collision2D collision )
  {
     var other = collision.collider;
     var contactCell = collision.contacts[0];
     Vector2 Contact = new Vector2(contactCell.point.x, contactCell.point.y);
     var normal = contactCell.normal;
     normal.x *= (_gridTiles.transform.localScale.x * 0.5f);
     normal.y *= (_gridTiles.transform.localScale.x * 0.5f);
     Contact =  Contact - normal;
     Vector3Int pointCell = _grid.WorldToCell(Contact);
     if( isDeleteTile( _sprites, pointCell ) )
     {
      // _tileMap.SetTile(pointCell,null);
     }

  }
}

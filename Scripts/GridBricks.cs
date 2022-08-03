using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBricks : MonoBehaviour
{
    [SerializeField] private GridLayout _gridTiles;
    [SerializeField] private Transform _tileMapObject;
    [SerializeField] private Tilemap _tileMap;
    private BoundsInt _bounds;
    private TileBase[] _tiles;
    private Collider2D _tilemapCollider;
    private Dictionary<Vector3,Vector3Int> _cells;

    //private BoundsInt cellBounds;


  private void Awake()
  {
    /*
    _tiles = _tileMap.GetTilesBlock(_bounds);
    _bounds = _tileMap.cellBounds;
    _tilemapCollider = GetComponent<Collider2D>();
    var boundsTile = _tilemapCollider.bounds;
    _cells = new Dictionary<Vector3,Vector3Int>();

    cellBounds = new BoundsInt
                        (
                          _gridTiles.WorldToCell(boundsTile.min),
                          _gridTiles.WorldToCell(boundsTile.size) + new Vector3Int(0,0,1)
                        );

     var index = 0;
    foreach(var cell in cellBounds.allPositionsWithin)
    {
        if(_tileMap.HasTile(cell))
        {
            _cells[_gridTiles.CellToWorld(cell)] = cell;
        }
    }
    */
  }

 public void FindCellToDel( Collision2D collision )
  {
      var other = collision.collider;
      var cellBounds = new BoundsInt
                        (
                          _gridTiles.WorldToCell(other.bounds.min),
                          _gridTiles.WorldToCell(other.bounds.size) + new Vector3Int(0,0,1)
                        );


       var minClosestPointDistance = 1f;
       var minCellDistance = Vector3Int.zero;

      foreach(var cell in cellBounds.allPositionsWithin)
      {
          if(_tileMap.HasTile(cell))
          {
            var cellWorldCenter = _gridTiles.CellToWorld(cell);
            
             foreach( var contact in collision.contacts )
             {
                var closestPoint = Vector2.Distance( contact.point, cellWorldCenter);
                if( minClosestPointDistance > closestPoint ) 
                {
                  minClosestPointDistance = closestPoint;
                  minCellDistance = cell;
                }
             }
          }
      }
      _tileMap.SetTile(minCellDistance,null);
  }
}

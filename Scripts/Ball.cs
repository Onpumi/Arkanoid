using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


public class Ball : MonoBehaviour
{
    [SerializeField] private float _size;
    [SerializeField] private Color _color;
    [SerializeField] private InputBoard _inputBoard;
    [SerializeField] private Board _board;
    [SerializeField] private Transform _parentblocks;
    [SerializeField] private GridLayout _tileGrid;
    [SerializeField] private Transform _tileMapObject;
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private LayerMask _maskaRaycast;
    public bool IsMove { get; private set; }
    public event Action<Vector2,int> OnHit;
    public event Action OnHitPlay;
    public event Action<GameObject> OnDestroyBlock;
    public Vector2 Direction;
    private float _timeDelta;
    private GridLayout _grid;
    private BoundsInt _bounds;
    private TileBase[] _tiles;
    private Vector3 pointHit;
    private Vector2 closestPoint;
    private Vector2 Normal;
    private Vector2 Contact;
    private Vector3 Position;
    List<Vector3Int> trackedCells;


   private void Awake()
   {
      _timeDelta = Time.time;
      _bounds = _tileMap.cellBounds;
      _tiles = _tileMap.GetTilesBlock(_bounds);
      trackedCells = new List<Vector3Int>();
   }
   private void OnEnable()
   {
      _inputBoard.OnMove += Move;
      _inputBoard.OnPress += StartMove;
   }

   private void OnDisable()
   {
      _inputBoard.OnMove -= Move;
      _inputBoard.OnPress -= StartMove;
   }


  private void FindCellToDel( Collision2D collision, Collider2D other, BoundsInt cellBounds )
  {
      var exitedCells = trackedCells.ToList();
       int i = 0;

       float minClosestPointDistance = 100;
       Vector3Int minCellDistance = Vector3Int.zero;

      foreach(var cell in cellBounds.allPositionsWithin)
      {
          if(_tileMap.HasTile(cell))
          {
            var cellWorldCenter = _tileGrid.CellToWorld(cell);
            var otherClosestPoint = other.ClosestPoint(cellWorldCenter);
            var otherClosestCell = _tileGrid.WorldToCell(otherClosestPoint);
            
             foreach( var contact in collision.contacts )
             {
                
                var closestPoint = Vector2.Distance( contact.point, cellWorldCenter);
                if( minClosestPointDistance > closestPoint ) 
                {
                  minClosestPointDistance = closestPoint;
                  minCellDistance = cell;
                }
             }

              if( otherClosestCell == cell )
              {
                  if ( !trackedCells.Contains(cell) )
                  {
                    trackedCells.Add(cell);
                    
                  }
                  else
                  {
                    exitedCells.Remove(cell);
                  }
                  
              }

              i++;
          }
      }

      _tileMap.SetTile(minCellDistance,null);

     foreach( var cell in exitedCells ) 
     {
      trackedCells.Remove(cell);
     }
      Debug.Log(i);

  }


   private void OnCollisionEnter2D( Collision2D collision )
   {

        if( Time.time-_timeDelta < 0.1f && _parentblocks != collision.gameObject.transform.parent )
        {
        //  return;
        }


        var other = collision.collider;
        var cellBounds = new BoundsInt (
                        _tileGrid.WorldToCell(other.bounds.min),
                        _tileGrid.WorldToCell(other.bounds.size) + new Vector3Int(0,0,1)
                      );






         Normal = collision.contacts[0].normal;

        if( IsMove )
        {

            int angleBoard = 0;

            var bufferDirection = Direction;

            if( _board.transform == collision.transform )
            {
              Vector3 nearPointPositionBoard = collision.collider.ClosestPoint(transform.position);
              Vector3 ballLocalPosition = _board.transform.InverseTransformPoint( nearPointPositionBoard );
              angleBoard = _board.GetAngleReflect(ballLocalPosition.x);
            }

            var point = collision.contacts[0].point;
             Vector2 normal = Vector2.zero;
             
             
             for( int i = 0 ; i < collision.contactCount; i++ )
             {
               normal += collision.contacts[i].normal;
               Contact += collision.contacts[i].point;
             }
               normal /= collision.contactCount;
              OnHit?.Invoke(normal,angleBoard);


              //Debug.Log(Vector3.Angle(normal,Direction));
              //var angle = Vector3.Angle(normal,Direction) < 10;
              Contact /= collision.contactCount;
              Normal /= collision.contactCount;

              Contact.Normalize();
              Normal.Normalize();
              Position = transform.position;

              OnHitPlay?.Invoke();

           

            closestPoint = collision.collider.ClosestPoint(transform.position);
              


            //if( _tileMapObject == collision.gameObject.transform.parent )
            if( _tileMapObject == collision.gameObject.transform )
            {

              ContactPoint2D contact = collision.GetContact(0);

                    FindCellToDel( collision, other, cellBounds );
                  


            /*
               RaycastHit2D hitDown; 
               var lenghRay = 0.1f;
                hitDown = Physics2D.Raycast( (Vector3)Position, -(Vector3)Normal, lenghRay, _maskaRaycast );
                pointHit = hitDown.point + (-Normal) * 0.06f; 
               _tileMap.SetTile(_tileMap.WorldToCell(pointHit),null);
                pointHit = hitDown.point + (bufferDirection-Normal) * 0.06f; 
               _tileMap.SetTile(_tileMap.WorldToCell(pointHit),null);

               for( int i = 0 ; i < collision.contactCount; i++ )
               {
                   hitDown = Physics2D.Raycast( (Vector3)Position, -(Vector3)collision.contacts[i].normal, lenghRay, _maskaRaycast );
                  pointHit = hitDown.point + (-collision.contacts[i].normal) * 0.06f; 
                 _tileMap.SetTile(_tileMap.WorldToCell(pointHit),null);
                  pointHit = hitDown.point + (bufferDirection-collision.contacts[i].normal) * 0.06f; 
                 _tileMap.SetTile(_tileMap.WorldToCell(pointHit),null);
               }

                 for( int i = 0 ; i < collision.contactCount; i++ )
               {
                   hitDown = Physics2D.Raycast( closestPoint, -(Vector3)collision.contacts[i].normal, lenghRay, _maskaRaycast );
                  pointHit = hitDown.point + (-collision.contacts[i].normal) * 0.06f; 
                 _tileMap.SetTile(_tileMap.WorldToCell(pointHit),null);
                  pointHit = hitDown.point + (bufferDirection-collision.contacts[i].normal) * 0.06f; 
                 _tileMap.SetTile(_tileMap.WorldToCell(pointHit),null);
               }


               if(pointHit != null) 
               {
//                 Debug.Log($"есть контакт ");
               }
               else
               {
  //               Debug.Log("нет контакта");
               }




             Vector3Int v = _tileMap.WorldToCell(contact.point);
             //Destroy(_tileMap.GetTile(v));

              //var tilebase = collision.transform.GetComponent<TileBase>();
              
              if( collision.transform.TryGetComponent(typeof(Tilemap), out Component component))
              {
                        //var bounds = _tileMap.cellBounds;
                        //var tiles = _tileMap.GetTilesBlock(_bounds);
                        //var grid = collision.transform.GetComponent<Grid>();
                        //var coordinat =  grid.WorldToCell(collision.contacts[0].point);
                       foreach( var item in _tileMapObject )
                       {
                            //_tileMap.SetTile(coordinat, null);
                       }
                     var tilemapCollider = collision.transform.GetComponent<TilemapCollider2D>();


//                     Debug.Log( tilemapCollider.transform.position);  
                     //   Debug.Log(_tileMap.GetTile(new Vector3Int(0,0,0)));
              }

              */
            }

        }

        _timeDelta = (float)Time.time;
   }

   private void Move()
   {
        if( IsMove == false )
        {
            var xCoordinateBoard = _inputBoard.TouchPosition.x;
            transform.position = new Vector3( xCoordinateBoard, transform.position.y, transform.position.y );
        }
   }

   private void StartMove()
   {
     IsMove = true;
   }

   private void Update()
   {
    //Debug.DrawLine( (Vector3)Position,((Vector3)Position - (Vector3)Normal), Color.green);
    Debug.DrawRay( (Vector3)Position, - (Vector3)Normal, Color.green);
   }
  
}

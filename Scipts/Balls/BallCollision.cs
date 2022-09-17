using System;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    [SerializeField] private Transform _parentRemoveBricks;
    [SerializeField] private Transform _destroyerBorder;
    [SerializeField] private SoundsPlayer _soundPlayer;
    private BallTime _ballTime;
    private Vector2 _reflectDirection;
    private float _anglelNormalReflect;
    private Vector2 _velocity;
    public event Action OnDestroyBall;
    public event Action<Vector2> OnCollisionBall;
    public Vector3[] Normal = new Vector3[2];

     private void Awake()
     {
        _velocity = Vector2.zero;
        _ballTime = new BallTime();
     }

     private void OnCollisionEnter2D( Collision2D collision )
  {
      if( _ballTime.isNeedTime() )
   {
       if( _soundPlayer.isCanPlay )
       {
        _soundPlayer.PlayHitBall();
       }
  
      Vector2 normal = collision.contacts[0].normal;

      if( collision.contacts.Length > 1 )
      {
         Normal[1] = collision.contacts[1].normal;
         if( Vector3.Angle(Normal[0],Normal[1]) >= 90 )
         {
            normal = Normal[0].normalized + Normal[1].normalized / 2f;
            normal.Normalize();
         }
      }

     Normal[0] = normal;
     _reflectDirection = Vector3.Reflect(_velocity.normalized, normal.normalized);
     var angleRandom = UnityEngine.Random.Range(0,5);
     _reflectDirection = Quaternion.Euler(0,0,angleRandom) * _reflectDirection;
     _anglelNormalReflect = Vector3.Angle(_reflectDirection.normalized, collision.contacts[0].normal.normalized );
     _velocity = _reflectDirection;
    if( _parentRemoveBricks && collision.collider.transform.parent == _parentRemoveBricks  ) 
    {
         Destroy(collision.collider.transform.gameObject);
         _ballTime.FixedTime( Time.time );
    }

     if( _destroyerBorder && collision.collider.transform == _destroyerBorder )
     {
        OnDestroyBall?.Invoke();
     }
     OnCollisionBall?.Invoke( _velocity );
   }
  }

  private void OnCollisionStay2D( Collision2D collision )
  {
      if( _ballTime.isNeedTime() )
    {
       var angleRandom = UnityEngine.Random.Range(5,10);
      _velocity = Quaternion.Euler(0,0,angleRandom) * _velocity;

      if( _anglelNormalReflect > 90 )
      {
        _velocity = Quaternion.Euler(0,0,_anglelNormalReflect) * _velocity;
      }
      _anglelNormalReflect = Vector3.Angle(_velocity.normalized, collision.contacts[0].normal.normalized );
      if( _anglelNormalReflect == 0 )
      {
          _velocity = Quaternion.Euler(0,0, 15) * _velocity;
      }
      OnCollisionBall?.Invoke( _velocity );
    }
  }

  public void SetVelocity( Vector2 velocity )
  {
    _velocity = velocity;
  }
#if UNITY_EDITOR
 
  private void OnCollisionExit2D( Collision2D collision )
  {
    Normal[0] = Vector2.zero;
    Normal[1] = Vector2.zero;
  }

  private void Update()
  {
    Debug.DrawRay( transform.position, 0.05f * Normal[0], Color.green);
    Debug.DrawRay( transform.position, 0.05f * Normal[1], Color.blue);
    Debug.DrawRay( transform.position, 0.05f * _velocity, Color.red);
  }
#endif
  
}

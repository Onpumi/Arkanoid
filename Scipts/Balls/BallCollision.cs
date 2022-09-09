using System;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    [SerializeField] private Transform _parentRemoveBricks;
    [SerializeField] private Transform _destroyerBorder;
    private float _prevTime = 0;
    private Vector2 ReflectDirection;
    private float AnglelNormalReflect;
    private Vector2 _velocity;
    public event Action OnDestroyBall;
    public event Action<Vector2> OnCollisionBall;
    public Vector3[] Normal = new Vector3[2];

     private void Awake()
     {
        _velocity = Vector2.zero;
     }

     private void OnCollisionEnter2D( Collision2D collision )
  {

  //  if( (_prevTime != 0 && Time.time-_prevTime >= 0.02) || _prevTime == 0 )
   {

    Vector2 normal = collision.contacts[0].normal;

      if( collision.contacts.Length > 1 )
      {
        // Debug.Log(collision.contacts.Length);
         Normal[1] = collision.contacts[1].normal;
         if( Vector3.Angle(Normal[0],Normal[1]) >= 90 )
         {
            normal = Normal[0].normalized + Normal[1].normalized / 2f;
            normal.Normalize();
         //   _velocity = -_velocity;
          //  OnCollisionBall?.Invoke( _velocity );s
           // return;
         }
      }

     Normal[0] = normal;
     ReflectDirection = Vector3.Reflect(_velocity.normalized, normal.normalized);
     AnglelNormalReflect = Vector3.Angle(ReflectDirection.normalized, collision.contacts[0].normal.normalized );
     _velocity = ReflectDirection;
    if( _parentRemoveBricks && collision.collider.transform.parent == _parentRemoveBricks  ) 
    {
         Destroy(collision.collider.transform.gameObject);
         _prevTime = Time.time;
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
   // if( (_prevTime != 0 && Time.time-_prevTime >= 0.02) || _prevTime == 0 )
    {
      if( AnglelNormalReflect > 90 )
      {
        _velocity = Quaternion.Euler(0,0,AnglelNormalReflect) * _velocity;
      }
      AnglelNormalReflect = Vector3.Angle(_velocity.normalized, collision.contacts[0].normal.normalized );
      if( AnglelNormalReflect == 0 )
      {
          _velocity = Quaternion.Euler(0,0, 15) * _velocity;
      }
      OnCollisionBall?.Invoke( _velocity );
    }
  }

  private void OnCollisionExit2D( Collision2D collision )
  {
    Normal[0] = Vector2.zero;
    Normal[1] = Vector2.zero;
  }


  public void SetVelocity( Vector2 velocity )
  {
    _velocity = velocity;
  }

  private void Update()
  {
    Debug.DrawRay( transform.position, 0.05f * Normal[0], Color.green);
    Debug.DrawRay( transform.position, 0.05f * Normal[1], Color.blue);
    Debug.DrawRay( transform.position, 0.05f * _velocity, Color.red);

 }
  
}

using System;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public class BallCollision : MonoBehaviour
{
    [SerializeField] private Transform _destroyerBorder;
    [SerializeField] private SoundsPlayer _soundPlayer;
    private BallTime _ballTime;
    private Vector2 _reflectDirection;
    private float _anglelNormalReflect;
    private Vector2 _velocity;
    public event Action OnDestroyBall;
    public event Action<Vector2> OnCollisionBall;
    private Vector3[] Normal = new Vector3[2];
    private Rigidbody2D _rigidbody;

     private void Awake()
     {
        _velocity = Vector2.zero;
        _ballTime = new BallTime();
        _rigidbody = GetComponent<Rigidbody2D>();
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

       _reflectDirection = Vector3.Reflect(_velocity, normal);
       //_velocity = Vector3.Reflect(_velocity, normal);
       var angleRandom = UnityEngine.Random.Range(0,5);
       _velocity = Quaternion.Euler(0,0,angleRandom) * _reflectDirection;
      _anglelNormalReflect = Vector3.Angle(_velocity, collision.contacts[0].normal );
    
    _ballTime.FixedTime( Time.time );
     
     if( _destroyerBorder && collision.collider.transform == _destroyerBorder )
     {
        //OnDestroyBall?.Invoke();
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
 
//  private void OnCollisionExit2D( Collision2D collision )
//  {
//    Normal[0] = Vector2.zero;
//    Normal[1] = Vector2.zero;
// }


  private void Update()
  {
    //Debug.DrawRay( transform.position, 0.05f * Normal[0], Color.green);
    //Debug.DrawRay( transform.position, 0.05f * Normal[1], Color.blue);
    //Debug.DrawRay( transform.position, 0.05f * _velocity, Color.red);
        
        if( Input.GetKeyDown(KeyCode.Space))
       {
         int numTests = 500000;
         using(new CustomTimer("Controlled Test", numTests))
         {
             for( int i = 0; i < numTests ; ++i)
             {

                     //_reflectDirection = Quaternion.Euler(0,0,5) * _reflectDirection;

              //float angle = UnityEngine.Random.Range(3,5);
                //Vector3.Reflect(_velocity.normalized, Normal[0].normalized);
                //OnCollisionBall?.Invoke( _velocity );
                // _rigidbody.velocity = _rigidbody.velocity.normalized * 0.5f * Time.fixedDeltaTime ;
            //  TestCollinearity();

             }
         }
       }

  }
#endif



  
}

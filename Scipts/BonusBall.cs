using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeBonus
{
    ReproductionOne,
    ReproductionTwo,
    Star,
    None
}

public interface EventBonus
{
   void  ActivateAction( FabrikaBalls factoryBalls, Ball ball );
   void ActivateAll( Action action );
}

class ReproductionOne : EventBonus
{
    public void  ActivateAction( FabrikaBalls factoryBalls, Ball ball )
    {
        factoryBalls.SpawnBall( ball, 3);
    }

    public void ActivateAll( Action action )
    {
        action?.Invoke();
    }
}

public class BonusBall : MonoBehaviour
{
    [SerializeField] TypeBonus _type;
    [SerializeField]  private int _count;
    [SerializeField]  private float _speed;
    public EventBonus  EventBonus;
    private Transform  _parentBalls;
    private Board      _board;
    public int Count => _count;
    private Vector3 _directionMove = -Vector3.up;
    private bool _isOpen = false;
    private TypesBonus _typesBonus;

    private void Awake()
    {
       _typesBonus = new TypesBonus();
       _typesBonus.Init();
       EventBonus = _typesBonus.GetBonus( _type );
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if( collision.collider.TryGetComponent(out BorderRemover remover) )
       {
         Destroy(this.transform.gameObject);
       }

    }


    private void ActivateBonus(FabrikaBalls factoryBalls, Ball ball)
    {
        if( EventBonus != null )
        {
            EventBonus.ActivateAction( factoryBalls,ball );
        }
    
    }

     public void ActivateBonus(Action action)  
     {
         action?.Invoke();
     }


     public EventBonus ReturnBonus()
     {
        return EventBonus;
     }

    public void OpenBonus()
    {
       _isOpen = true;
       
    }
    private void Move()
    {
       transform.position += _directionMove * _speed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        if( _isOpen )
        {
            Move();
        }
    }
    

}

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
public class BonusBall : MonoBehaviour
{
    [SerializeField]  private TypeBonus _type;
    [SerializeField]  private int _count;
    [SerializeField]  private float _speed;
    public int Count => _count;
    private Vector3 _directionMove = -Vector3.up;
    public bool IsOpen {get; private set;}
    private TypesBonus _typesBonus;
    public TypeBonus Type=>_type;

    private void Awake()
    {
       _typesBonus = new TypesBonus();
       _typesBonus.Init();
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if( collision.collider.TryGetComponent(out BorderRemover remover) )
       {
         //Destroy(this.transform.gameObject);
         transform.gameObject.SetActive(false);
       }
    }

     public void ActivateBonus( Action action )  
     {
         action?.Invoke();
     }

    // public void ActivateBonus( FactoryBalls factoryBalls )
     //{
        //factoryBalls.CloningBalls();
     //}

    public void OpenBonus()
    {
       IsOpen = true;
    }

    private void Move()
    {
       transform.position += _directionMove * _speed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        if( IsOpen )
        {
            Move();
        }
    }
    

}

using System;
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
    private Vector3 _directionMove;
    public bool IsOpen {get; private set;}
    public TypeBonus Type=>_type;

    private void Awake()
    {
       transform.localScale = new Vector3(1,1,1);
       _directionMove = -Vector3.up * _speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if( collision.collider.TryGetComponent(out BorderRemover remover) )
        {
          transform.gameObject.SetActive(false);
        }
    }

    public void SetCount( Level level )
    {
      _count = level.GetCountBonus( this );
    }


    public void OpenBonus()
    {
       IsOpen = true;
    }

    private void Move()
    {
       transform.position += _directionMove;
    }

    private void FixedUpdate()
    {
        if( IsOpen )
        {
            Move();
        }
    }
    

}

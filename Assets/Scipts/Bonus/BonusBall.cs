using System;
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
    private Vector3 _directionMove;
    public bool IsOpen {get; private set;}
    public TypeBonus Type=>_type;
    public event Action OnReproductionOne;
    public event Action OnReproductionTwo;
    private Dictionary<TypeBonus, Action> _dictionaryActions;

    private void Awake()
    {
       _directionMove = -Vector3.up * _speed * Time.fixedDeltaTime;
       
    }

    private void InitActions()
    {
        _dictionaryActions = new Dictionary<TypeBonus, Action>();
        _dictionaryActions[TypeBonus.ReproductionOne] = OnReproductionOne;
        _dictionaryActions[TypeBonus.ReproductionTwo] = OnReproductionTwo;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if( collision.collider.TryGetComponent(out BorderRemover remover) )
        {
          transform.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        InitActions();
    }

    public void SetCount( Level level )
    {
      _count = level.GetCountBonus( this );
    }

    public void OpenBonus()
    {
       IsOpen = true;
    }

    public void DisableBonus()
    {
        IsOpen = false;
        
    }

    private void Move()
    {
       transform.position += _directionMove;
    }
    
    private void InvokeBonus( Action actionBonus )
    {
        actionBonus?.Invoke();
    }

    public void ActivateBonus()
    {
        InvokeBonus(_dictionaryActions[_type]);
        transform.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if( IsOpen )
        {
            Move();
        }
    }
    

}

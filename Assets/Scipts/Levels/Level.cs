using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour, IPoolable<Level>
{
   [SerializeField] private int _index;
   [SerializeField] private PropertyBonus[] _propertyBonuses;
   private const string _prefix = "Level";
   
   private readonly StringBuilder _nameBuilder = new StringBuilder();
   public string FullName { get; private set; }
   public int Index => _index;

   
    private void Awake()
    {
      FullName = _nameBuilder.Append(_prefix).Append(' ').Append(_index).ToString();
    }
    public void SpawnFrom( IPool<Level> pool )
  {
     transform.gameObject.SetActive(true);
  }

  public void Despawn()
  {
    transform.gameObject.SetActive(false);
  }

  public int GetCountBonus( BonusBall bonusBall )
  {
      foreach( var propertyBonus in _propertyBonuses)
      {
          if( bonusBall == propertyBonus.bonusBall)
          {
             return propertyBonus.countBonus;
          }
      }
      return bonusBall.Count;
  }

}

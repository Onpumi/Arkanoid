using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypesBonus
{

    public Dictionary<TypeBonus, EventBonus > bonuses;

    public void Init()
    {
        Dictionary<TypeBonus,EventBonus> _bonuses = new Dictionary<TypeBonus,EventBonus>();
//        bonuses[TypeBonus.ReproductionOne] = new ReproductionOne();
    }

    public EventBonus GetBonus( TypeBonus type )
    {
        if( type == TypeBonus.ReproductionOne )
        {
          return new ReproductionOne();
        }
        else
        {
          return null;
        }
    }



}

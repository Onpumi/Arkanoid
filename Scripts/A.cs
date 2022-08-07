using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A : MonoBehaviour, IPoolable<A>
{
    public void SpawnFrom( IPool<A> pool )
    {
       transform.position = new Vector3( -1, 0, 0);
       transform.gameObject.SetActive(true);
       var sprite = transform.GetComponent<SpriteRenderer>();
       sprite.color = Color.red;
    }

    public void Despawn()
    {
       transform.gameObject.SetActive(false);
    }

    public void Die()
    {
        
    }
}
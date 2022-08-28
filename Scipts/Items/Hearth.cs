using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearth : MonoBehaviour
{
   [SerializeField] int _count;
   [SerializeField] Transform _prefabImage;



    private void Start()
    {
      //Instantiate( _prefabImage, transform.position, Quaternion.identity);
    }

    public void DisplayItems( int count )
    {

    }

}

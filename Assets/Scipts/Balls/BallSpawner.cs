using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public class BallSpawner : MonoBehaviour

{
    [SerializeField] private FactoryBalls _factoryBalls;


    public void SpawnBalls( Ball ball )
    {
       // _factoryBalls.SpawnTwoBalls( ball );
    }
    
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerBall : MonoBehaviour
{
   [SerializeField] private Button _spawnButton;
   public event Action OnDoSpawn;

   
   private void OnEnable()
   {
    _spawnButton.onClick.AddListener(SpawnBalls);
   }

   private void OnDisable()
   {
    _spawnButton.onClick.AddListener(SpawnBalls);
   }

   private void SpawnBalls()
   {
        OnDoSpawn?.Invoke();
   } 
}


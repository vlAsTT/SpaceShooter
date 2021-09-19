using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Managers
{
    public class EnemySpawner : MonoBehaviour
    {
        #region Variables

        [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
        [SerializeField] private Transform enemiesParent;
        [SerializeField] private float SpawnRange = 300f;
        [SerializeField] private float SpawnCooldown = 5f;
        [SerializeField][Range(1, 100)] private int maxShipsAlive = 15;

        private List<GameObject> enemies = new List<GameObject>();

        private Transform _player;
        private bool _spawnEnabled = true;
        
        #endregion

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").transform;
            StartCoroutine(SpawnPlayer());
        }

        public void SetSpawnEnabled(bool status) => _spawnEnabled = status;

        private IEnumerator SpawnPlayer()
        {
            while (_spawnEnabled)
            {
                if (maxShipsAlive > enemies.Count)
                {
                    // Calculate index of a prefab taken and spawn position
                    int index = Random.Range(0, enemyPrefabs.Count);
                    var spawnPosition = GetRandomPositionInSphere(_player.position, SpawnRange);
                
                    enemies.Add(Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity, enemiesParent));
                }
                
                yield return new WaitForSeconds(SpawnCooldown);
            }
        }

        private Vector3 GetRandomPositionInSphere(Vector3 position, float range)
        {
            float x = Random.Range(position.x, position.x + range);
            float y = Random.Range(position.y, position.y + range);
            float z = Random.Range(position.z, position.z + range);

            return new Vector3(x, y, z);
        }
    }
}

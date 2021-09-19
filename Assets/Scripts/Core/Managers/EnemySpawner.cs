using System;
using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Managers
{
    public class EnemySpawner : MonoBehaviour
    {
        #region Variables

        [SerializeField] private List<PooledObjectType> enemyTypes;
        [SerializeField] private float SpawnRange = 300f;
        [SerializeField] private float SpawnCooldown = 5f;

        private Transform _player;
        private bool _spawnEnabled = true;
        
        #endregion

        private void Start()
        {
            if (enemyTypes.Count == 0)
            {
                Debug.unityLogger.Log(LogType.Assert, $"Please, add appropriate enemy types to {name}");
            }
            
            _player = GameObject.FindWithTag("Player").transform;
            StartCoroutine(SpawnEnemy());
        }

        public void SetSpawnEnabled(bool status) => _spawnEnabled = status;

        private IEnumerator SpawnEnemy()
        {
            while (_spawnEnabled)
            {
                var spawnPosition = GetRandomPositionInSphere(_player.position, SpawnRange);

                var enemyShip = ObjectPooler.Instance.GetPooledObject(enemyTypes[Random.Range(0, enemyTypes.Count)]);
                enemyShip.SetActive(true);
                enemyShip.transform.position = spawnPosition;
                
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

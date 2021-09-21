using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * As at the current project's state - there are no different enemy waves, no bosses.
 * The decision to not split Enemy & Obstacle to be different scripts with one base abstract class
 * was made as there is no need for it now, but in case of future expansion need - the refactoring can be done in few
 * clicks.
 * */

namespace Core.Managers
{
    public class ObjectSpawner : MonoBehaviour
    {
        #region Variables
        
        [SerializeField] private List<PooledObjectType> objectSpawnableTypes;
        [SerializeField] private float spawnRange = 300f;
        [SerializeField] private float spawnCooldown = 5f;
        [SerializeField] private float spawnOriginOffset = 150f;

        private Transform _player;
        private bool _spawnEnabled = true;
        
        #endregion

        private void Start()
        {
            if (objectSpawnableTypes.Count == 0)
            {
                Debug.unityLogger.Log(LogType.Assert, $"Please, add appropriate spawnable object types to {name}");
            }
            
            _player = GameObject.FindWithTag("Player").transform;
            StartCoroutine(SpawnEnemy());
        }

        public void SetSpawnEnabled(bool status) => _spawnEnabled = status;

        private IEnumerator SpawnEnemy()
        {
            while (_spawnEnabled)
            {
                var spawnPosition = GetRandomPositionInSphere(_player.position, spawnRange, spawnOriginOffset);

                var enemyShip = ObjectPooler.Instance.GetPooledObject(objectSpawnableTypes[Random.Range(0, objectSpawnableTypes.Count)]);
                enemyShip.SetActive(true);
                enemyShip.transform.position = spawnPosition;
                
                yield return new WaitForSeconds(spawnCooldown);
            }
        }

        private Vector3 GetRandomPositionInSphere(Vector3 position, float range, float positionOffset)
        {
            float x = Random.Range(position.x + positionOffset, position.x + range);
            float y = Random.Range(position.y + positionOffset, position.y + range);
            float z = Random.Range(position.z + positionOffset, position.z + range);

            return new Vector3(x, y, z);
        }
    }
}

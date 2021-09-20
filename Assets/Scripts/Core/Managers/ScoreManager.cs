using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace Core.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private List<PooledObjectToValue> scoresForObjects = new List<PooledObjectToValue>();
        public int _score;

        #endregion

        public void AddScore(PooledObjectType type)
        {
            foreach (var obj in scoresForObjects)
            {
                if (obj.Type == type)
                {
                    _score += (int)obj.Value;
                }
            }
        }

        public int GetScore() => _score;
    }
}

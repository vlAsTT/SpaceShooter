using System;
using System.Collections.Generic;
using Objects;
using Player;
using UI;
using UnityEngine;

namespace Core.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Variables

        public static ScoreManager Instance;
        
        [SerializeField] private List<PooledObjectToValue> scoresForObjects = new List<PooledObjectToValue>();
        private int _score;

        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _score = 0;
            GameOverlayManager.Instance.SetScore(0);
        }

        private void OnEnable()
        {
            PlayerDelegates.onPlayerDeath += OnGameOver;
        }

        private void OnDisable()
        {
            PlayerDelegates.onPlayerDeath -= OnGameOver;
        }

        public void AddScore(PooledObjectType type)
        {
            foreach (var obj in scoresForObjects)
            {
                if (obj.type == type)
                {
                    _score += (int)obj.value;
                    GameOverlayManager.Instance.SetScore(_score);
                }
            }
        }

        public int GetScore() => _score;

        private void OnGameOver()
        {
            Debug.unityLogger.Log(LogType.Log, $"Current Best Score is {SaveLoadManager.GetBestScore()}");
            SaveLoadManager.SaveBestScore(_score);
            Debug.unityLogger.Log(LogType.Log, $"New Best Score is {SaveLoadManager.GetBestScore()}");
        }
    }
}

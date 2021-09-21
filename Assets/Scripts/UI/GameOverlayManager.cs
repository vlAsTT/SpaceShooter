using System.Collections.Generic;
using Core.Managers;
using Objects;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// A structure that maps PooledObjectType to appropriate 
    /// </summary>
    [System.Serializable]
    public struct PooledObjectImageUI
    {
        public PooledObjectType type;
        public Image image;
    }

    // Additional implementation for the case where pooled object has a UI text
    // [System.Serializable]
    // public struct PooledObjectTextUI
    // {
    //     public PooledObjectType type;
    //     public TextMeshProUGUI text;
    // }
    
    /// <summary>
    /// Handles all UI operations of In-Game HUD at runtime
    /// </summary>
    public class GameOverlayManager : MonoBehaviour
    {
        #region Variables / References

        public static GameOverlayManager Instance;

        [Tooltip("Maps all pooled object types to appropriate Images elements in UI")]
        [SerializeField] private List<PooledObjectImageUI> pooledObjectImageUi = new List<PooledObjectImageUI>();
        // [SerializeField] private List<PooledObjectTextUI> pooledObjectTextUi = new List<PooledObjectTextUI>();

        [Header("In-Game Overlay")][Tooltip("Reference to In-Game HUD Object")]
        [SerializeField] private GameObject gameOverlay;
        [Tooltip("Reference to In-Game Score Element")]
        [SerializeField] private TextMeshProUGUI scoreText;

        [Header("Game Over HUD")][Tooltip("Reference to Game Over HUD Object")]
        [SerializeField] private GameObject gameOverCanvas;
        [Tooltip("Reference to Score Field in Game Over HUD")]
        [SerializeField] private TextMeshProUGUI finalScoreField;
        [Tooltip("Sound that is being played when Player clicks on the button")]
        [SerializeField] private AudioClip clickSound;

        private AudioSource _audioSource;
        
        #endregion

        #region Methods

        #region Unity Methods

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (!Camera.main)
            {
                Debug.unityLogger.Log(LogType.Error, "Main Camera is missing");
                return;
            }
            
            _audioSource = Camera.main.GetComponent<AudioSource>();

            if (!_audioSource)
            {
                Debug.unityLogger.Log(LogType.Error,$"Variables are not initialized at {name}");
            }
            
            gameOverlay.SetActive(true);
            gameOverCanvas.SetActive(false);
        }

        private void OnEnable()
        {
            PlayerDelegates.onPlayerDeath += GameOver;
        }

        private void OnDisable()
        {
            PlayerDelegates.onPlayerDeath -= GameOver;
        }

        #endregion

        public void SetScore(int score) => scoreText.text = score.ToString();

        public void SetPooledObjectUIStatus(PooledObjectType type, bool value)
        {
            foreach (var obj in pooledObjectImageUi)
            {
                if (obj.type == type)
                {
                    obj.image.gameObject.SetActive(value);
                    return;
                }
            }

            // Additional implementation for the case where pooled object has a UI text that needs to be turned on/off
            // foreach (var obj in pooledObjectTextUi)
            // {
            //     if (obj.type == type)
            //     {
            //         obj.text.gameObject.SetActive(value);
            //     }
            // }
        }

        /// <summary>
        /// Handles UI operations when Game is Over
        /// </summary>
        private void GameOver()
        {
            gameOverlay.gameObject.SetActive(false);

            finalScoreField.text = ScoreManager.Instance.GetScore().ToString();
            gameOverCanvas.gameObject.SetActive(true);
        }

        /// <summary>
        /// UI Called by Back to Game Menu Button in Game Over HUD
        /// </summary>
        public void OnGameOverClicked()
        {
            _audioSource.PlayOneShot(clickSound);
            
            // Unfreezes time, so next time game continues fine
            Time.timeScale = 1;
            
            // Loads Main Menu Scene
            SceneManager.LoadSceneAsync("_Scenes/Menu/MainMenu");
        }

        #endregion
    }
}

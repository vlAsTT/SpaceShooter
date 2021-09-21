using System.Collections;
using System.Collections.Generic;
using Core.Utilities;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Responsible for handling all main menu logic & game scene loading
    /// </summary>
    /// <seealso cref="SceneManager"/>
    public class MainMenuLoader : MonoBehaviour
    {
        #region Variables

        #region References
        
        [Header("Main Menu & Loading Screen")] [Tooltip("Reference to the parent menu object")]
        [SerializeField] private GameObject menu;
        
        [Tooltip("Reference to the parent loading screen object")]
        [SerializeField] private GameObject loadingScreen;
        
        [Tooltip("Reference to the Image of Loading Bar Progress")]
        [SerializeField] private Image loadingProgressBar;
        
        [Tooltip("Reference to the top score text object that displays the top score")]
        [SerializeField] private TextMeshProUGUI topScoreField;
        
        [Header("Sound Effects")] [Tooltip("Sound that is being played when Player clicks on the button")]
        [SerializeField] private AudioClip clickSound;
        
        private AudioSource _audioSource;
        
        #endregion

        /// <summary>
        /// List of scenes that are being loaded when user proceeds to the Game scene
        /// </summary>
        private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

        #endregion

        #region Methods

        #region Unity Standard

        /// <summary>
        /// Checks that all variables are initialized and initialize variables
        /// </summary>
        private void Start()
        {
            if (!Camera.main)
            {
                Debug.unityLogger.Log(LogType.Error, "Main Camera is missing");
                return;
            }
            
            _audioSource = Camera.main.GetComponent<AudioSource>();

            if (!menu || !loadingScreen || !loadingProgressBar || !topScoreField || !_audioSource || !clickSound)
            {
                Debug.unityLogger.Log(LogType.Error,$"Some variables are not initialized correctly at {name}");
            }
        }

        #endregion
        
        #region Menu Buttons Methods

        /// <summary>
        /// Loads Game Scene & Runs Loading Screen
        /// </summary>
        /// <seealso cref="HideMenu"/>
        /// <seealso cref="ShowLoadingScreen"/>
        /// <seealso cref="SceneManager"/>
        public void StartGame()
        {
            PlayClickSound();
            
            HideMenu();
            ShowLoadingScreen();

            SceneManager.LoadSceneAsync("Game");

            StartCoroutine(StartLoadingScreen());
        }
        
        public void ShowTopScore()
        {
            PlayClickSound();

            topScoreField.text = SaveLoadManager.GetBestScore().ToString();
        }

        public void Exit()
        {
            PlayClickSound();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit(1);
        }

        #endregion

        #region Utility Methods

        private void PlayClickSound()
        {
            _audioSource.PlayOneShot(clickSound);
        }
        
        private void HideMenu()
        {
            menu.SetActive(false);
        }

        private void ShowLoadingScreen()
        {
            loadingScreen.SetActive(true);
        }

        /// <summary>
        /// Updates loading progress on the loading bar
        /// </summary>
        /// <returns>IEnumerator</returns>
        /// <seealso cref="Coroutine"/>
        IEnumerator StartLoadingScreen()
        {
            var totalProgress = 0f;

            foreach (var scene in _scenesToLoad)
            {
                while (!scene.isDone)
                {
                    totalProgress += scene.progress;
                    loadingProgressBar.fillAmount = totalProgress / _scenesToLoad.Count;
                    yield return null;
                }
            }
        }

        #endregion

        #endregion
    }
}

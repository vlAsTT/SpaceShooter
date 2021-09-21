using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    /// <summary>
    /// Loads necessary scenes on the boot and other core systems for the correct game loop
    /// <seealso cref="SceneManager"/>
    /// </summary>
    public class BootLoader : MonoBehaviour
    {
        /// <summary>
        /// Loads Main Menu scene
        /// Called when the application is starting
        /// </summary>
        private void Start()
        {
            SceneManager.LoadSceneAsync("_Scenes/Menu/MainMenu");
        }
    }
}

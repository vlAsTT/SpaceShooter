using UnityEngine;

namespace Core.Utilities
{
    /// <summary>
    /// Handles all save & load related operations
    /// </summary>
    public static class SaveLoadManager
    {
        public static void SaveBestScore(int score)
        {
            var currentBestScore = PlayerPrefs.GetInt("BestScore");

            if (currentBestScore < score)
            {
                PlayerPrefs.SetInt("BestScore", score);
            }
        }

        public static int GetBestScore() => PlayerPrefs.GetInt("BestScore");
    }
}

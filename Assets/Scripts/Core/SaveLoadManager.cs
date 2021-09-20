using UnityEngine;

namespace Core
{
    public static class SaveLoadManager
    {
        public static void SaveBestScore(int score)
        {
            var currentBestScore = PlayerPrefs.GetInt("Score");

            if (currentBestScore < score)
            {
                PlayerPrefs.SetInt("Score", score);
            }
        }

        public static int GetBestScore() => PlayerPrefs.GetInt("Score");
    }
}

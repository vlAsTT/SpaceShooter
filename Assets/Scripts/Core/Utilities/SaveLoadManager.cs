using UnityEngine;

namespace Core
{
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

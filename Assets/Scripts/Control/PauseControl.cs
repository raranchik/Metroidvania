using UnityEngine;

namespace Control
{
    public static class PauseControl
    {
        public static bool GameIsPaused()
        {
            return Time.timeScale == 0f;
        }

        public static void PauseGame()
        {
            Time.timeScale = 0f;
        }

        public static void UnpauseGame()
        {
            Time.timeScale = 1f;
        }

    }

}

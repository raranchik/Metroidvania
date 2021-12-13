using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private int _countLevels;

        public static readonly string MainMenuName = "MainMenu";

        private string _levelNamePrefix = "Level_";

        public static void OnQuitGame()
        {
            Application.Quit();
        }

        public static void OnMainMenuLoad()
        {
            SceneManager.LoadScene(MainMenuName);
        }

        public void OnLevelLoad(int levelIndex)
        {
            if (levelIndex >= 0 && levelIndex <= _countLevels)
            {
                SceneManager.LoadScene(string.Concat(_levelNamePrefix, levelIndex));
            }
        }

        public void OnPauseGame()
        {
            Time.timeScale = 0f;
        }

        public void OnUnpauseGame()
        {
            Time.timeScale = 1f;
        }

    }

}

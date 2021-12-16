using System;
using Control;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
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
            PauseControl.UnpauseGame();
        }

        public void OnLevelLoad(int levelIndex)
        {
            if (levelIndex >= 0 && levelIndex <= _countLevels)
            {
                SceneManager.LoadScene(string.Concat(_levelNamePrefix, levelIndex));
                PauseControl.UnpauseGame();
            }
        }

        public void OnPauseGame()
        {
            PauseControl.PauseGame();
        }

        public void OnUnpauseGame()
        {
            PauseControl.UnpauseGame();
        }

    }

}

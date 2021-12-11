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

        private string _levelNamePrefix = "Level_";

        public static void OnQuitGame()
        {
            Application.Quit();
        }

        public void OnLevelLoad(int levelIndex)
        {
            if (levelIndex >= 0 && levelIndex <= _countLevels)
            {
                SceneManager.LoadScene(String.Concat(_levelNamePrefix, levelIndex));
            }
        }

    }

}

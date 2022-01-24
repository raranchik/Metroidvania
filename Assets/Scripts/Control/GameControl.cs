using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Control
{
    [Serializable]
    public class GameEndEvent : UnityEvent<string>
    {
    }

    sealed class GameControl: MonoBehaviour
    {
        private const string SceneNamePrefixLevel = "Level_";
        private const string SceneNameMainMenu = "MainMenu";

        [SerializeField]
        private GameObject _gameEndCanvas;

        public static GameControl Instance { get; private set; }
        public static string CurrentLevel { get; private set; } = SceneNameMainMenu;
        public static float GameTime { get; private set; }

        public GameEndEvent gameEndEvent = new GameEndEvent();

        private bool _gameIsRun = false;

        private void Awake ()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
                Instance.gameEndEvent.AddListener(OnGameEnd);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            TimerRun();
        }

        public static bool GameIsPaused()
        {
            return Time.timeScale == 0f;
        }

        public void PauseGame()
        {
            Instance._gameIsRun = false;
            Time.timeScale = 0f;
        }

        public void UnpauseGame()
        {
            Instance._gameIsRun = true;
            Time.timeScale = 1f;
        }

        public void LoadLevel(int level)
        {
            string levelName = SceneNamePrefixLevel + level;
            CurrentLevel = levelName;
            if (Application.CanStreamedLevelBeLoaded(levelName))
            {
                GameTime = 0f;
                Instance._gameIsRun = true;
                UnpauseGame();
                SceneManager.LoadScene(levelName);
            }
        }

        public void LoadMainMenu()
        {
            if (Application.CanStreamedLevelBeLoaded(SceneNameMainMenu))
            {
                CurrentLevel = SceneNameMainMenu;
                GameTime = 0f;
                Instance._gameIsRun = false;
                UnpauseGame();
                SceneManager.LoadScene("MainMenu");
            }
        }

        public void CloseGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        private void OnGameEnd(string gameStatus)
        {
            PauseGame();
            Instance._gameIsRun = false;

            GameObject gameEndCanvas = Instantiate(_gameEndCanvas);
            Canvas canvas = gameEndCanvas.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;

            Transform messagePanel = null;
            if (gameStatus.Equals("Win"))
            {
                messagePanel = gameEndCanvas.transform.Find("WinMessagePanel");

                TextMeshProUGUI timeText = messagePanel.Find("TimeText").GetComponent<TextMeshProUGUI>();
                PrintTime(timeText);

                Image[] scoreImages = messagePanel.Find("ScoreImages").GetComponentsInChildren<Image>();
                PrintCompletionRate(scoreImages);
            }
            else if (gameStatus.Equals("Loss"))
            {
                messagePanel = gameEndCanvas.transform.Find("LossMessagePanel");
            }
            messagePanel.gameObject.SetActive(true);

            GameTime = 0f;
        }

        private void PrintTime(in TextMeshProUGUI timeText)
        {
            float minutes = Mathf.FloorToInt(GameTime / 60); 
            float seconds = Mathf.FloorToInt(GameTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void PrintCompletionRate(in Image[] scoreImages)
        {
            int score = UIScoreBar.Instance.Score;
            int totalPossibleScore = UIScoreBar.TotalPossibleScore;
            float completionRate = ((float)score / (float)totalPossibleScore) * 100f;
            int completedSections = completionRate <= 33.33f ? 1 :
                completionRate > 33.33f && completionRate <= 66.66f ? 2 : 3;
            for (int i = 0; i < completedSections; i++)
            {
                Image section = scoreImages[i];
                section.color = Color.white;
            }
        }

        private void TimerRun()
        {
            if (Instance._gameIsRun && !GameIsPaused())
            {
                GameTime += Time.deltaTime;
            }
        }

         

    }

}
using System;
using System.Collections.Generic;
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

    sealed class GameControl: MonoBehaviour, IPrintCompletedData
    {
        [SerializeField]
        private GameObject _gameEndCanvas;

        public static GameControl Instance { get; private set; }

        private const string SceneNamePrefixLevel = "Level_";
        private const string SceneNameMainMenu = "MainMenu";

        private SaveLoadDataControl _saveLoadDataControl;
        private LevelData _currentLevelData;
        private bool _gameIsRun = false;

        public GameEndEvent gameEndEvent = new GameEndEvent();

        public int CurrentLevel { get; private set; }
        public float GameTime { get; private set; }

        private void Awake ()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
                Instance.gameEndEvent.AddListener(OnGameEnd);
                Instance._saveLoadDataControl = new SaveLoadDataControl();
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

        public List<LevelData> GetLevelData()
        {
            return _saveLoadDataControl.LevelData;
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
            if (!Application.CanStreamedLevelBeLoaded(levelName))
                return;

            CurrentLevel = level;
            GameTime = 0f;
            Instance._gameIsRun = true;
            UnpauseGame();

            _currentLevelData = null;
            int countCompletedLevels = _saveLoadDataControl.LevelData.Count;
            if (countCompletedLevels != 0)
            {
                if (level <= countCompletedLevels)
                {
                    _currentLevelData = _saveLoadDataControl.LevelData[level];
                }
            }

            SceneManager.LoadScene(levelName);
        }

        public void LoadMainMenu()
        {
            if (!Application.CanStreamedLevelBeLoaded(SceneNameMainMenu))
                return;

            GameTime = 0f;
            Instance._gameIsRun = false;
            UnpauseGame();

            SceneManager.LoadScene(SceneNameMainMenu);
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
                messagePanel = gameEndCanvas.transform.GetChild(1);
                Transform completedDataPanel = messagePanel.GetChild(1);

                TextMeshProUGUI timeText = completedDataPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
                PrintTime(timeText, GameTime);

                int score = UIScoreBar.Instance.Score;
                int totalPossibleScore = UIScoreBar.TotalPossibleScore;
                int completedScoreSections = GetCompletedSections(score, totalPossibleScore);
                Image[] scoreImages = completedDataPanel.GetChild(0).GetComponentsInChildren<Image>();
                PrintCompletionRate(scoreImages, completedScoreSections);

                SaveLevelResult(completedScoreSections, GameTime);
            }
            else if (gameStatus.Equals("Loss"))
            {
                messagePanel = gameEndCanvas.transform.GetChild(2);
            }
            messagePanel.gameObject.SetActive(true);

            GameTime = 0f;
        }

        private int GetCompletedSections(in int score, in int totalPossibleScore)
        {
            float completionRate = ((float) score / (float) totalPossibleScore) * 100f;
            float oneSection = 100f / 3f;
            return completionRate <= oneSection ? 1 :
                completionRate > oneSection && completionRate <= oneSection * 2 ? 2 : 3;
        }

        public void PrintTime(in TextMeshProUGUI timeText, in float gameTime)
        {
            float minutes = Mathf.FloorToInt(gameTime / 60); 
            float seconds = Mathf.FloorToInt(gameTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void PrintCompletionRate(in Image[] scoreImages, in int completedScoreSections)
        {
            for (int i = 0; i < completedScoreSections; i++)
            {
                Image section = scoreImages[i];
                section.color = Color.white;
            }
        }

        private void SaveLevelResult(in int completedScoreSections, in float completionTime)
        {
            int level = CurrentLevel;
            LevelData levelData = new LevelData(level, completedScoreSections, completionTime);
            if (_currentLevelData == null)
            {
                _saveLoadDataControl.SaveData(levelData);
            }
            else
            {
                _saveLoadDataControl.UpdateData(levelData, level);
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
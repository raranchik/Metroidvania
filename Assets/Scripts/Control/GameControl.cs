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
        [SerializeField]
        private GameObject _gameEndCanvas;

        public static GameControl Instance { get; private set; }
        public static int CurrentLevel { get; private set; }
        public static float GameTime { get; private set; }

        public GameEndEvent gameEndEvent;

        private bool _gameIsRun = false;

        private void Awake ()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if(Instance == this)
            {
                Destroy(gameObject);
            }

            GameTime = 0f;
            _gameIsRun = true;
            gameEndEvent ??= new GameEndEvent();
            gameEndEvent.AddListener(OnGameEnd);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            TimerRun();
        }

        public void LoadLevel(int level)
        {
            PauseControl.UnpauseGame();

            CurrentLevel = level;
            string levelName = "Level_" + level;
            if (Application.CanStreamedLevelBeLoaded(levelName))
            {
                GameTime = 0f;
                _gameIsRun = true;
                SceneManager.LoadScene(levelName);
            }
        }

        public void LoadMainMenu()
        {
            if (Application.CanStreamedLevelBeLoaded("MainMenu"))
            {
                GameTime = 0f;
                _gameIsRun = false;
                SceneManager.LoadScene("MainMenu");
            }
        }

        private void OnGameEnd(string gameStatus)
        {
            PauseControl.PauseGame();
            _gameIsRun = false;

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
            if (_gameIsRun)
            {
                GameTime += Time.deltaTime;
            }
        }

         

    }

}
using System.Collections.Generic;
using Control;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UILevelSelectMap : MonoBehaviour, IPrintCompletedData
    {
        [SerializeField]
        private int _countLevels;
        [SerializeField]
        private int _levelsPerPage = 4;
        [SerializeField]
        private GameObject _levelLoadButton;
        [SerializeField]
        private Transform _levelItemsPanel;
        [SerializeField]
        private TextMeshProUGUI _pageLabel;

        private int _currentPage;
        private List<Transform> _levelButtons = new List<Transform>();

        private void Awake()
        {
            if (_countLevels == 0)
                return;

            _currentPage = 0;
            for (int i = 0; i < _countLevels; i++)
            {
                GameObject buttonObj = Instantiate(_levelLoadButton, _levelItemsPanel);
                Button buttonComponent = buttonObj.GetComponent<Button>();
                int level = i;
                if (level == 0)
                    buttonComponent.interactable = true;
                buttonComponent.onClick.AddListener(() => GameControl.Instance.LoadLevel(level));

                Transform buttonTransform = buttonObj.transform;
                Transform levelNameSection = buttonTransform.GetChild(0);
                TextMeshProUGUI levelNameText = levelNameSection.GetChild(0).GetComponent<TextMeshProUGUI>();
                levelNameText.text = (i + 1).ToString();

                _levelButtons.Add(buttonTransform);
            }
        }

        private void OnEnable()
        {
            LevelPanelReset();
            UpdateButtonLabels();
        }

        private void OnDisable()
        {
            foreach (Transform button in _levelButtons)
            {
                button.gameObject.SetActive(false);
            }
        }

        private void LevelPanelReset()
        {
            _currentPage = 0;
            _pageLabel.text = (_currentPage + 1).ToString();
            for (int i = 0; i < _levelsPerPage; i++)
            {
                _levelButtons[i].gameObject.SetActive(true);
            }
        }

        private void UpdateButtonLabels()
        {
            List<LevelData> levelsData = GameControl.Instance.GetLevelData();
            int countCompletedLevel = levelsData.Count;
            if (countCompletedLevel == 0)
                return;

            for (int i = 0; i < _countLevels; i++)
            {
                Transform buttonTransform = _levelButtons[i];
                Transform completedDataSection = buttonTransform.GetChild(1);
                TextMeshProUGUI timeText = completedDataSection.GetChild(1).GetComponent<TextMeshProUGUI>();

                if (i < countCompletedLevel)
                {
                    buttonTransform.GetComponent<Button>().interactable = true;
                    LevelData levelData = levelsData[i];
                    Image[] scoreImages = completedDataSection.GetChild(0).GetComponentsInChildren<Image>();
                    int completedScoreSections = levelData.completedScoreSections;
                    PrintCompletionRate(scoreImages, completedScoreSections);
                    float completionTime = levelData.completionTime;
                    PrintTime(timeText, completionTime);
                }
                else
                {
                    timeText.text = "UNLOCKED";
                }
            }
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

    }

}
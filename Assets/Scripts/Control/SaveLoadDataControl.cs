using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Control
{
    [Serializable]
    public class LevelDataCollection
    {
        public List<LevelData> levels;

        public LevelDataCollection()
        {
            levels = new List<LevelData>();
        }

    }

    [Serializable]
    public class LevelData
    {
        public int level;
        public int completedScoreSections = 0;
        public float completionTime = 0f;

        public LevelData(int level, int completedScoreSections, float completionTime)
        {
            this.level = level;
            this.completedScoreSections = completedScoreSections;
            this.completionTime = completionTime;
        }

    }

    public class SaveLoadDataControl
    {
        public static readonly string SaveFilePath = Application.persistentDataPath + "/SaveData.json";

        private LevelDataCollection _levelDataCollection = new LevelDataCollection();

        public List<LevelData> LevelData
        {
            get => _levelDataCollection.levels;
            private set => _levelDataCollection.levels = value;
        }

        public SaveLoadDataControl()
        {
            LoadData();
        }

        public void SaveData(in LevelData newData)
        {
            _levelDataCollection.levels.Add(newData);
            WriteData();
        }

        public void UpdateData(in LevelData updateData, in int level)
        {
            _levelDataCollection.levels[level] = updateData;
            WriteData();
        }

        public void LoadData()
        {
            if (File.Exists(SaveFilePath))
            {
                string saveData = File.ReadAllText(SaveFilePath);
                _levelDataCollection = JsonUtility.FromJson<LevelDataCollection>(saveData);
            }
        }

        private void WriteData()
        {
            string levelsDataJson = JsonUtility.ToJson(_levelDataCollection);
            File.WriteAllText(SaveFilePath, levelsDataJson);
        }

    }

}
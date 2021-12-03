using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Talerock
{
    public class TopResultsPanel : MonoBehaviour
    {
        [SerializeField] private LevelCreator levelCreator;
        [SerializeField] private TextMeshProUGUI topLabel;
        [SerializeField] private TextMeshProUGUI topResultLabelPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private Button startNewGame;
        [SerializeField] private int countTopResults = 10;

        private const string FileName = "ResultData.json";

        private List<TextMeshProUGUI> _instantiatedResults;
        private List<ResultModule> _resultModules;

        private void Awake()
        {
            startNewGame.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                levelCreator.CreateNextLevel();
            });

            _instantiatedResults = new List<TextMeshProUGUI>();
            _resultModules = new List<ResultModule>();
            topLabel.text = $"Топ {countTopResults} результат{GetCorrectWordEnding(countTopResults)}:";

            LoadResults();
        }

        public void RefreshTopResults(string userName, int point)
        {
            var resultModule = new ResultModule
            {
                UserName = userName,
                CountPoint = point
            };

            _resultModules.Add(resultModule);

            SortModules();
            
            CropResultModules();

            using var fileStream = File.CreateText(Application.persistentDataPath + FileName);
            {
                var json = JsonConvert.SerializeObject(_resultModules, Formatting.Indented);
                fileStream.Write(json);
            }

            InstantiateResultLabels();
        }
        
        private static string GetCorrectWordEnding(int count)
        {
            var number = Math.Abs(count) % 100;
            if (number % 10 == 0 || number % 10 >= 5 && number % 10 <= 9 || number > 9 && number < 20)
                return "ов";

            if (number % 10 == 1)
                return "";

            return "а";
        }

        private void LoadResults()
        {
            if (!File.Exists(Application.persistentDataPath + FileName))
                return;

            var resultDataFile = File.ReadAllText(Application.persistentDataPath + FileName);
            var resultModules = JsonConvert.DeserializeObject<List<ResultModule>>(resultDataFile);

            if (resultModules == null)
                return;

            _resultModules.AddRange(resultModules);
            
            CropResultModules();

            InstantiateResultLabels();
        }

        private void InstantiateResultLabels()
        {
            foreach (var instantiatedResult in _instantiatedResults)
                Destroy(instantiatedResult.gameObject);

            _instantiatedResults.Clear();

            for (var i = 0; i < _resultModules.Count; i++)
            {
                var topResultLabel = Instantiate(topResultLabelPrefab, container);
                var resultModule = _resultModules[i];
                topResultLabel.text = $"{i + 1}. {resultModule.UserName} {resultModule.CountPoint} очков";
                _instantiatedResults.Add(topResultLabel);
            }
        }

        private void SortModules()
        {
            for (var i = 0; i < _resultModules.Count - 1; i++)
            {
                for (var j = 0; j < _resultModules.Count - 1; j++)
                {
                    var currentModule = _resultModules[j];
                    var nextModule = _resultModules[j + 1];
                    if (currentModule.CountPoint >= nextModule.CountPoint)
                        continue;

                    _resultModules[j] = nextModule;
                    _resultModules[j + 1] = currentModule;
                }
            }
        }

        private void CropResultModules()
        {
            if (_resultModules.Count > countTopResults)
                _resultModules.RemoveRange(countTopResults, _resultModules.Count - countTopResults);
        }
    }

    public struct ResultModule
    {
        public string UserName;
        public int CountPoint;
    }
}
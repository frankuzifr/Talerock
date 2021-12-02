﻿using System.Collections.Generic;
using UnityEngine;

namespace Talerock
{
    public class LevelCreator : MonoBehaviour
    {
        [Header("External dependencies")] 
        
        [Header("Levels settings")]
        [SerializeField] private List<LevelSettings> levelsSettings;

        [Header("Prefabs")] 
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private Option optionPrefab;

        [Header("Containers")] 
        [SerializeField] private Transform cellsContainerTransform;
        [SerializeField] private Transform optionsContainerTransform;

        private List<Option> _instantiatedOptions;
        private List<Cell> _instantiatedCells;
        
        private int _nextLevelNumber;

        private void Awake()
        {
            _instantiatedOptions = new List<Option>();
            _instantiatedCells = new List<Cell>();
            CreateNextLevel();
        }

        public void CreateNextLevel()
        {
            if (_nextLevelNumber > levelsSettings.Count)
                return;

            var levelsSetting = levelsSettings[_nextLevelNumber];

            var optionsColors = levelsSetting.OptionsColors;

            var optionsContainer = Environment.Instance.OptionsContainer;

            foreach (var optionColor in optionsColors)
            {
                var option = Instantiate(optionPrefab, optionsContainerTransform);
                option.SetOptionColor(optionColor);
                
                optionsContainer.AddOptionToContainer(option);
                _instantiatedOptions.Add(option);
            }

            var combinationsColors = levelsSetting.CombinationColors;

            foreach (var combinationColor in combinationsColors)
            {
                var cell = Instantiate(cellPrefab, cellsContainerTransform);
                cell.SetRightColor(combinationColor);
                
                _instantiatedCells.Add(cell);
            }

            _nextLevelNumber++;
        }
    }
}
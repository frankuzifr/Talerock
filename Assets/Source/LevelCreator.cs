using System.Collections.Generic;
using System.Linq;
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

        private List<Cell> _instantiatedCells;

        private ResultChecker _resultChecker;

        private int _nextLevelNumber;

        private void Awake()
        {
            CurrentPhase.OnPhaseChanged += () =>
            {
                if (CurrentPhase.Phase == Phases.EndGame)
                    _nextLevelNumber = 0;
            };
            
            _instantiatedCells = new List<Cell>();

            _resultChecker = Environment.Instance.ResultChecker;

            _resultChecker.OnNextLevel += CreateNextLevel;
            //CreateNextLevel();
        }

        public void CreateNextLevel()
        {
            ClearLevel();
            
            if (_nextLevelNumber == levelsSettings.Count - 1)
                _resultChecker.SetLastLevelOnTrue();    

            var levelsSetting = levelsSettings[_nextLevelNumber];
            var optionsColors = levelsSetting.OptionsColors;
            var optionsContainer = Environment.Instance.OptionsContainer;
            var timer = Environment.Instance.Timer;
            
            timer.SetTimerValues(levelsSetting.TimeForCheck, levelsSetting.TimeForAnswer);

            foreach (var optionColor in optionsColors)
            {
                var option = Instantiate(optionPrefab, optionsContainerTransform);
                option.SetOptionColor(optionColor);
                
                optionsContainer.AddOptionToContainer(option);
            }

            var combinationsColors = levelsSetting.CombinationColors;

            foreach (var combinationColor in combinationsColors)
            {
                var cell = Instantiate(cellPrefab, cellsContainerTransform);
                cell.SetRightColor(combinationColor);
                
                _instantiatedCells.Add(cell);
            }

            _nextLevelNumber++;
            
            _resultChecker.SetCells(_instantiatedCells.ToList());
            _resultChecker.SetPointsForRightAnswer(levelsSetting.PointsForRightAnswer);

            CurrentPhase.Phase = Phases.Check;
            
            timer.StartTimer();
        }

        private void ClearLevel()
        {
            foreach (var instantiatedCell in _instantiatedCells)
                Destroy(instantiatedCell.gameObject);
            
            _instantiatedCells.Clear();

            var optionsContainer = Environment.Instance.OptionsContainer;
            optionsContainer.ClearContainer();
        }

        private void OnDestroy()
        {
            _resultChecker.OnNextLevel -= CreateNextLevel;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Talerock
{
    public class LevelCreator : MonoBehaviour
    {
        [Header("Levels settings")]
        [SerializeField] private List<LevelSettings> levelsSettings;

        [Header("Prefabs")] 
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private Option optionPrefab;

        [Header("Containers")] 
        [SerializeField] private Transform cellsContainerTransform;
        [SerializeField] private Transform optionsContainerTransform;

        private List<Cell> _instantiatedCells;
        private List<Option> _instantiatedOptions;

        private ResultChecker _resultChecker;
        private OptionsContainer _optionsContainer;

        private int _nextLevelNumber;

        private void Awake()
        {
            CurrentPhase.OnPhaseChanged += () =>
            {
                if (CurrentPhase.Phase == Phases.EndGame)
                    _nextLevelNumber = 0;
            };
            
            _instantiatedCells = new List<Cell>();
            _instantiatedOptions = new List<Option>();

            _resultChecker = Environment.Instance.ResultChecker;
            _optionsContainer = Environment.Instance.OptionsContainer;

            _resultChecker.OnNextLevel += CreateNextLevel;
        }

        public void StartNewGame()
        {
            _nextLevelNumber = 0;
            CreateNextLevel();
        }

        private void CreateNextLevel()
        {
            if (_nextLevelNumber == levelsSettings.Count - 1)
                _resultChecker.SetLastLevelOnTrue();

            var levelsSetting = levelsSettings[_nextLevelNumber];
            var timer = Environment.Instance.Timer;

            timer.SetTimerValues(levelsSetting.TimeForCheck, levelsSetting.TimeForAnswer);

            var optionsColors = levelsSetting.OptionsColors;
            RefreshOptions(optionsColors.Count);

            var optionsInContainer = _optionsContainer.GetOptions();

            for (var i = 0; i < optionsColors.Count; i++)
            {
                optionsInContainer[i].SetOptionColor(optionsColors[i]);
            }
            
            var combinationsColors = levelsSetting.CombinationColors;
            RefreshCells(combinationsColors.Count);

            for (var i = 0; i < combinationsColors.Count; i++)
            {
                _instantiatedCells[i].SetRightColor(combinationsColors[i]);
            }

            _nextLevelNumber++;
            
            _resultChecker.SetCells(_instantiatedCells.ToList());
            _resultChecker.SetPointsForRightAnswer(levelsSetting.PointsForRightAnswer);

            CurrentPhase.Phase = Phases.Check;
            
            timer.StartTimer();
        }

        private void RefreshOptions(int countOptions)
        {
            foreach (var instantiatedOption in _instantiatedOptions)
            {
                instantiatedOption.EnableOption();
                instantiatedOption.ResetOptionPosition();
            }
            
            var optionsInContainer = _optionsContainer.GetOptions();
            var optionsDifference = countOptions - optionsInContainer.Length;
            
            if (optionsDifference > 0)
            {
                for (var i = 0; i < optionsDifference; i++)
                {
                    var option = Instantiate(optionPrefab, optionsContainerTransform);
                    _instantiatedOptions.Add(option);
                    _optionsContainer.AddOptionToContainer(option);
                }
            }

            if (optionsDifference < 0)
            {
                for (var i = 0; i > optionsDifference; i--)
                {
                    var instantiatedOptionsCount = _instantiatedOptions.Count;
                    _instantiatedOptions.RemoveAt(instantiatedOptionsCount - 1);
                    _optionsContainer.RemoveLastOption();
                }
            }
            
            _optionsContainer.RefreshContainer();
        }

        private void RefreshCells(int countCells)
        {
            foreach (var instantiatedCell in _instantiatedCells)
                instantiatedCell.RemoveOption();
            
            var cellsDifference = countCells - _instantiatedCells.Count;
            if (cellsDifference > 0)
            {
                for (var i = 0; i < cellsDifference; i++)
                {
                    var cell = Instantiate(cellPrefab, cellsContainerTransform);
                    _instantiatedCells.Add(cell);
                }
            }

            if (cellsDifference < 0)
            {
                for (var i = 0; i > cellsDifference; i--)
                {
                    var instantiatedCellsCount = _instantiatedCells.Count;
                    var instantiatedCell = _instantiatedCells[instantiatedCellsCount - 1];
                    Destroy(instantiatedCell.gameObject);
                    _instantiatedCells.RemoveAt(instantiatedCellsCount - 1);
                }
            }
        }

        private void OnDestroy()
        {
            _resultChecker.OnNextLevel -= CreateNextLevel;
            CurrentPhase.OnPhaseChanged -= () =>
            {
                if (CurrentPhase.Phase == Phases.EndGame)
                    _nextLevelNumber = 0;
            };
        }
    }
}
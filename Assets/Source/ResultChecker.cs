using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Talerock
{
    public class ResultChecker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private EndGamePanel endGamePanel;
        
        public Action OnWrongCombination;
        public Action OnNextLevel;

        private List<Cell> _cells;

        private int _countReadiesCells;
        private int _pointsForRightAnswer;
        private int _totalPoints;

        private bool _isLastLevel;

        private void Awake()
        {
            _cells = new List<Cell>();
            CurrentPhase.OnPhaseChanged += EndGameInvoke;
            nextLevelButton.onClick.AddListener(() =>
            {
                OnNextLevel.Invoke();
                nextLevelButton.gameObject.SetActive(false);
            });
        }

        public void SetLastLevelOnTrue()
        {
            _isLastLevel = true;
        }

        public void SetPointsForRightAnswer(int points)
        {
            _pointsForRightAnswer = points;
        }

        public void SetCells(List<Cell> cells)
        {
            _cells = cells;
        }

        public void IncreaseCountReadiesCells()
        {
            _countReadiesCells++;
            if (_countReadiesCells == _cells.Count)
                CheckResult();
        }

        public void DecreaseCountReadiesCells()
        {
            if (_countReadiesCells > 0)
                _countReadiesCells--;
        }

        private void CheckResult()
        {
            foreach (var cell in _cells)
            {
                if (cell.IsRightCombination())
                    continue;

                OnWrongCombination.Invoke();
                return;
            }

            Environment.Instance.Timer.StopTimer();

            CurrentPhase.Phase = Phases.EndLevel;
            _totalPoints += _pointsForRightAnswer;

            titleLabel.text = "Молодец";

            _countReadiesCells = 0;

            if (!_isLastLevel)
                nextLevelButton.gameObject.SetActive(true);
            else
                CurrentPhase.Phase = Phases.EndGame;
        }

        private void EndGameInvoke()
        {
            if (CurrentPhase.Phase != Phases.EndGame)
                return;
            
            endGamePanel.gameObject.SetActive(true);
            endGamePanel.SetResult(_totalPoints);
            _totalPoints = 0;
            _isLastLevel = false;
        }

        private void OnDestroy()
        {
            CurrentPhase.OnPhaseChanged -= EndGameInvoke;
        }
    }
}
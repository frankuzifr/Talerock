using System;
using System.Collections.Generic;
using UnityEngine;

namespace Talerock
{
    public class ResultChecker : MonoBehaviour
    {
        public Action OnWrongCombination;

        private List<Cell> _cells;

        private int _countReadiesCells;

        private void Awake()
        {
            _cells = new List<Cell>();
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
            Debug.Log("Win");
        }
    }
}
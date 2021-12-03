using System;
using UnityEngine;
using UnityEngine.UI;

namespace Talerock
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Image cellImage;
        [SerializeField] private Color defaultColor = Color.gray;

        private Color _rightColor;
        private Color _currentColor;

        private void Awake()
        {
            CurrentPhase.OnPhaseChanged += () =>
            {
                if (CurrentPhase.Phase == Phases.Answer)
                    SetDefaultColor();
            };
        }

        public void SetRightColor(Color color)
        {
            _rightColor = color;
            cellImage.color = color;
        }

        public void SetOption(Option option)
        {
            _currentColor = option.GetOptionColor();
            var resultChecker = Environment.Instance.ResultChecker;
            resultChecker.IncreaseCountReadiesCells();
        }

        public void RemoveOption()
        {
            _currentColor = Color.clear;
            var resultChecker = Environment.Instance.ResultChecker;
            resultChecker.DecreaseCountReadiesCells();
        }

        public bool IsRightCombination()
        {
            return Math.Abs(_rightColor.r - _currentColor.r) < 0.01 && 
                   Math.Abs(_rightColor.g - _currentColor.g) < 0.01 && 
                   Math.Abs(_rightColor.b - _currentColor.b) < 0.01 &&
                   Math.Abs(_rightColor.a - _currentColor.a) < 0.01;
        }
        
        private void SetDefaultColor()
        {
            cellImage.color = defaultColor;
        }
    }
}
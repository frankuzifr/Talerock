using UnityEngine;
using UnityEngine.UI;

namespace Talerock
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Image cellImage;
        [SerializeField] private Color defaultColor = Color.gray;
        
        private Option _option;

        private Color _rightColor;
        private Color _currentColor;

        public void SetRightColor(Color color)
        {
            _rightColor = color;
            cellImage.color = color;
        }

        public void SetDefaultColor()
        {
            cellImage.color = defaultColor;
        }

        public void SetOption(Option option)
        {
            _option = option;
            _currentColor = option.GetOptionColor();
        }

        public void RemoveOption()
        {
            _option = null;
            _currentColor = Color.clear;
        }

        public bool IsRightCombination()
        {
            return _rightColor == _currentColor;
        }
    }
}
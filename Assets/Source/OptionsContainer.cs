using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Talerock
{
    public class OptionsContainer : MonoBehaviour
    {
        private List<Option> _optionsInContainer;
        private RectTransform _containerParentRectTransform;
        private RectTransform _containerRectTransform;
        private HorizontalLayoutGroup _horizontalLayoutGroup;

        private float _currentOptionsWidth;
        private float _optionsOffset;
        private int _countVisibleOptionInContainer;
        
        private void Awake()
        {
            _optionsInContainer = new List<Option>();
            _containerRectTransform = GetComponent<RectTransform>();
            _containerParentRectTransform = _containerRectTransform.parent.GetComponent<RectTransform>();
            _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
            _optionsOffset = _horizontalLayoutGroup.spacing;
        }

        public void AddOptionToContainer(Option option)
        {
            _optionsInContainer.Add(option);

            var optionRectTransform = option.GetComponent<RectTransform>();
            var optionRect = optionRectTransform.rect;
            var optionWidth = optionRect.width;
            
            _currentOptionsWidth += _optionsOffset + optionWidth;

            if (_currentOptionsWidth > _containerParentRectTransform.rect.width)
            {
                option.gameObject.SetActive(false);
            }
            else
            {
                _containerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _currentOptionsWidth);
                _countVisibleOptionInContainer++;
            }
        }

        public void RemoveOptionFromContainer(Option option)
        {
            _optionsInContainer.Remove(option);
            
            var optionRectTransform = option.GetComponent<RectTransform>();
            var optionRect = optionRectTransform.rect;
            var optionWidth = optionRect.width;

            _currentOptionsWidth -= _optionsOffset + optionWidth;
            
            if (_countVisibleOptionInContainer - 1 < _optionsInContainer.Count)
                _optionsInContainer[_countVisibleOptionInContainer - 1].gameObject.SetActive(true);
            else
            {
                _containerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _currentOptionsWidth);
                _countVisibleOptionInContainer--;
            }
        }

        public void ClearContainer()
        {
            _optionsInContainer.Clear();
        }

        public void RefreshOptions()
        {
            
        }
    }
}
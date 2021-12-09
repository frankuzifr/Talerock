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
                option.gameObject.SetActive(true);
                _containerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _currentOptionsWidth);
                _countVisibleOptionInContainer++;
            }
        }

        public void RefreshContainer()
        {
            _currentOptionsWidth = 0f;
            _countVisibleOptionInContainer = 0;

            foreach (var option in _optionsInContainer)
            {
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
                    option.gameObject.SetActive(true);
                    _containerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _currentOptionsWidth);
                    _countVisibleOptionInContainer++;
                }
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

        public void RemoveLastOption()
        {
            var count = _optionsInContainer.Count;
            var option = _optionsInContainer[count - 1];
            
            option.transform.SetParent(transform.parent);
            option.gameObject.SetActive(false);
            _optionsInContainer.RemoveAt(count - 1);
        }

        public Option[] GetOptions()
        {
            return _optionsInContainer.ToArray();
        }
    }
}
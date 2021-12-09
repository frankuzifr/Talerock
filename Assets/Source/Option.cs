using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Talerock
{
    public class Option : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image optionImage;

        private Canvas _canvas;
        private Cell _cell;
        private OptionsContainer _optionsContainer;

        private Vector3 _optionPosition;

        private bool _isRemoved;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _optionsContainer = Environment.Instance.OptionsContainer;
        }

        public void SetOptionColor(Color color)
        {
            optionImage.color = color;
        }

        public Color GetOptionColor()
        {
            return optionImage.color;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (CurrentPhase.Phase != Phases.Answer)
                return;

            if (_cell)
                transform.SetParent(_canvas.transform);

            _optionPosition = transform.position;

            optionImage.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (CurrentPhase.Phase != Phases.Answer)
                return;

            var mousePosition = Input.mousePosition;
            transform.position = mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (CurrentPhase.Phase != Phases.Answer)
                return;

            var eventDataPointerEnter = eventData.pointerEnter;

            optionImage.raycastTarget = true;
            
            if (TrySetOptionToCell(eventDataPointerEnter))
                return;
            
            if (TrySetOptionToRemover(eventDataPointerEnter))
                return;
            
            if (_cell)
                    ResetOption();
            else
                transform.position = _optionPosition;
        }

        public void EnableOption()
        {
            if (!_isRemoved)
                return;
            
            gameObject.SetActive(true);
            _optionsContainer.AddOptionToContainer(this);
            _isRemoved = false;
        }
        
        public void ResetOptionPosition()
        {
            if (!_cell)
                return;

            _cell = null;
            transform.SetParent(_optionsContainer.transform);
            _optionsContainer.AddOptionToContainer(this);
            var resultChecker = Environment.Instance.ResultChecker;
            resultChecker.OnWrongCombination -= ResetOption;
        }

        private bool TrySetOptionToCell(GameObject eventDataPointerEnter)
        {
            if (!eventDataPointerEnter)
                return false;
            
            if (!eventDataPointerEnter.TryGetComponent<Cell>(out var cell)) 
                return false;
            
            var resultChecker = Environment.Instance.ResultChecker;
            resultChecker.OnWrongCombination += ResetOption;

            var optionTransform = transform;
                
            optionTransform.SetParent(cell.transform);
            optionTransform.position = cell.transform.position;
                
            if (!_cell)
                _optionsContainer.RemoveOptionFromContainer(this);

            if (_cell)
                _cell.RemoveOption();

            _cell = cell;
            cell.SetOption(this);

            return true;
        }

        private bool TrySetOptionToRemover(GameObject eventDataPointerEnter)
        {
            if (!eventDataPointerEnter) 
                return false;
            
            if (!eventDataPointerEnter.TryGetComponent<OptionRemover>(out _)) 
                return false;
            
            gameObject.SetActive(false);
            _optionsContainer.RemoveOptionFromContainer(this);
            transform.SetAsLastSibling();
            _isRemoved = true;

            return true;
        }

        private void ResetOption()
        {
            _cell.RemoveOption();
            ResetOptionPosition();
        }

        private void OnDestroy()
        {
            var resultChecker = Environment.Instance.ResultChecker;
            resultChecker.OnWrongCombination -= ResetOption;
        }
    }
}
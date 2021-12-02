using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Talerock
{
    public class Option : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image optionImage;

        private Vector3 _optionPosition;

        private Cell _cell;

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
            if (CurrentPhase.Phase == Phases.Check)
                return;

            _optionPosition = transform.position;

            optionImage.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (CurrentPhase.Phase == Phases.Check)
                return;

            var mousePosition = Input.mousePosition;
            transform.position = mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (CurrentPhase.Phase == Phases.Check)
                return;

            var eventDataPointerEnter = eventData.pointerEnter;
            var optionsContainer = Environment.Instance.OptionsContainer;

            if (eventDataPointerEnter && eventDataPointerEnter.TryGetComponent<Cell>(out var cell))
            {
                transform.position = cell.transform.position;
                cell.SetOption(this);

                transform.SetParent(cell.transform);
                optionsContainer.RemoveOptionFromContainer(this);
                _cell = cell;
            }
            else if (eventDataPointerEnter && eventDataPointerEnter.TryGetComponent<OptionRemover>(out _))
            {
                optionsContainer.RemoveOptionFromContainer(this);
                Destroy(gameObject);
            }
            else
            {
                if (_cell)
                {
                    optionsContainer.AddOptionToContainer(this);
                    transform.SetParent(optionsContainer.transform);
                }
                else
                {
                    transform.position = _optionPosition;
                }
            }

            optionImage.raycastTarget = true;
        }

        public void ClearCell()
        {
            _cell = null;
        }
    }
}
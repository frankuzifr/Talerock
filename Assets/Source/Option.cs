using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Talerock
{
    public class Option : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image optionImage;

        public void SetOptionColor(Color color)
        {
            optionImage.color = color;
        }

        public Color GetOptionColor()
        {
            return optionImage.color;
        }

        //for test
        public void OnPointerClick(PointerEventData eventData)
        {
            var optionsContainer = Environment.Instance.OptionsContainer;
            optionsContainer.RemoveOptionFromContainer(this);
            Destroy(gameObject);
        }
    }
}
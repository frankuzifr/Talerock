using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Talerock
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultLabel;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button saveButton;
        [SerializeField] private TopResultsPanel topResultsPanel;

        private int _result;
        
        private void Awake()
        {
            saveButton.onClick.AddListener(SaveResult);
        }

        public void SetResult(int result)
        {
            _result = result;
            resultLabel.text = $"Вы набрали {result} очков";
        }

        private void SaveResult()
        {
            var nameText = nameInputField.text;
            nameInputField.text = string.Empty;
            topResultsPanel.gameObject.SetActive(true);
            topResultsPanel.RefreshTopResults(nameText, _result);
            gameObject.SetActive(false);
        }
    }
}
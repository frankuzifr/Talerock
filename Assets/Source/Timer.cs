using System;
using TMPro;
using UnityEngine;

namespace Talerock
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleLabel;
        
        private TMP_Text _timerLabel;
        
        private float _checkTime;
        private float _answerTime;

        private bool _isTimerWork;

        private void Awake()
        {
            _timerLabel = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            if (!_isTimerWork)
                return;

            if (CurrentPhase.Phase == Phases.Check)
            {
                _checkTime -= Time.deltaTime;

                _timerLabel.text = $"Время для запоминания: {Math.Round(_checkTime, 2)}";
                titleLabel.text = "Запомните цвета";
                
                if (_checkTime > 0)
                    return;

                CurrentPhase.Phase = Phases.Answer;
            }

            if (CurrentPhase.Phase == Phases.Answer)
            {
                _answerTime -= Time.deltaTime;
                
                _timerLabel.text = $"Время для ответа: {Math.Round(_answerTime, 2)}";
                titleLabel.text = "Заполните ячеики нужными цветами";
                
                if (_answerTime > 0)
                    return;

                _isTimerWork = false;
                CurrentPhase.Phase = Phases.EndGame;
            }
        }

        public void StartTimer()
        {
            _isTimerWork = true;
        }

        public void StopTimer()
        {
            _isTimerWork = false;
        }

        public void SetTimerValues(float checkTime, float answerTime)
        {
            _checkTime = checkTime;
            _answerTime = answerTime;
        }
    }
}
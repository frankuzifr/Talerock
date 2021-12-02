using System.Collections.Generic;
using UnityEngine;

namespace Talerock
{
    [CreateAssetMenu(menuName = "Create new level")]
    public class LevelSettings : ScriptableObject
    {
        [Header("Times")]
        [SerializeField] private float timeForCheck;
        [SerializeField] private float timeForAnswer;

        [Header("Color combination")] 
        [SerializeField] private List<Color> combinationColors;

        [Header("Color options")] 
        [SerializeField] private List<Color> optionsColors;

        public float TimeForCheck => timeForCheck;
        public float TimeForAnswer => timeForAnswer;
        public List<Color> CombinationColors => combinationColors;
        public List<Color> OptionsColors => optionsColors;
    }
}
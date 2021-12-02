using UnityEngine;

namespace Talerock
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private OptionsContainer optionsContainer;
        
        public static Environment Instance;

        public OptionsContainer OptionsContainer => optionsContainer;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else 
                Destroy(gameObject);
        }
    }
}
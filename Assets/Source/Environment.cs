using UnityEngine;

namespace Talerock
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private OptionsContainer optionsContainer;
        [SerializeField] private Timer timer;
        [SerializeField] private ResultChecker resultChecker;

        public static Environment Instance;

        public OptionsContainer OptionsContainer => optionsContainer;
        public Timer Timer => timer;
        public ResultChecker ResultChecker => resultChecker;


        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else 
                Destroy(gameObject);
        }
    }
}
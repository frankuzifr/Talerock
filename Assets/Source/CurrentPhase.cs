using System;

namespace Talerock
{
    public static class CurrentPhase
    {
        public static Action OnPhaseChanged; 
        
        private static Phases _phase;

        public static Phases Phase
        {
            get => _phase;
            set
            {
                _phase = value;
                OnPhaseChanged?.Invoke();
            }
        }
    }
}
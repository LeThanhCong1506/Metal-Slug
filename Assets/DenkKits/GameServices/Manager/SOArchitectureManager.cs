using Imba.Utils;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace DenkKits.GameServices.Manager
{
    public class SoArchitectureManager : ManualSingletonMono<SoArchitectureManager>
    {
        [Header("Events")]
        public IntGameEvent OnMoneyChangedEvent;
        public IntGameEvent OnGemChangedEvent;
        public GameEvent OnLoadGameplayDoneEvent;
        public GameEvent PauseGame;
        public GameEvent ResumeGame;

    }
}
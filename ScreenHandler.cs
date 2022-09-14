using System.Collections;
using UnityEngine;
using DG.Tweening;

// simple class to manager switching between different UI screens (uses the Screen.cs class)

namespace XStudios
{
    public class ScreenHandler : MonoBehaviour
    {
        [SerializeField]
        private Screen _attractScreen;
        private Screen _currentScreen;
        [SerializeField]
        private CanvasGroup _fader;

        public static ScreenHandler Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            _currentScreen = _attractScreen;
            StartCoroutine(_attractScreen.Enter());
        }

        public void PreviousScreen()
        {
            StartCoroutine(SwitchScreen(_currentScreen, _currentScreen.PrevScreen));
        }

        public void NextScreen()
        {
            StartCoroutine(SwitchScreen(_currentScreen, _currentScreen.NextScreen));
        }

        IEnumerator SwitchScreen(Screen from, Screen to)
        {
            if (from != to)
            {
                from.PreExitCleanup();
                _currentScreen = to;
                yield return StartCoroutine(to.Enter());
                StartCoroutine(from.Exit());
            }
        }

        void GotoScreen(Screen to)
        {
            StartCoroutine(SwitchScreen(_currentScreen, to));
        }

        public void TimeoutCurrentScreen(Screen Screen = null)
        {
            if (Screen == null)
                GotoScreen(_attractScreen);
            else
                GotoScreen(Screen);
        }
    }
}

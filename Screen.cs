using System.Collections;
using UnityEngine;
using DG.Tweening;

// a simple UI screen (is managed by ScreenHandler.cs)

namespace XStudios
{
    public class Screen : MonoBehaviour
    {
        [SerializeField]
        protected Screen _prevScreen, _nextScreen;

        [Header("If this checkbox is checked, screen will timeout.")]
        [SerializeField]
        protected bool _doesTimeout;
        protected float _timer = -1;
        protected bool _isActive = false;

        public Screen PrevScreen { get => _prevScreen; }
        public Screen NextScreen { get => _nextScreen; }

        protected virtual void Init()
        {
            //called before Enter()
            //override
        }

        public virtual IEnumerator Enter()
        {
            Init();
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            yield return GetComponent<CanvasGroup>().DOFade(1f, 0.3f).WaitForCompletion();
            _timer = Time.time;
			      _isActive = true;
        }

        public virtual void PreExitCleanup()
        {
            //called before Exit() by ScreenHandler
            //override
            _isActive = false;
        }

        public virtual IEnumerator Exit()
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            yield return GetComponent<CanvasGroup>().DOFade(0f, 0.3f).WaitForCompletion();
            AfterExitCleanup();
        }

        protected virtual void AfterExitCleanup()
        {
            //called after Exit()
            //override
        }

        void Timeout()
        {
            ScreenHandler.Instance.TimeoutCurrentScreen();
        }

        protected virtual void Update()
        {
            if (_isActive)
            {
                if (Input.anyKeyDown)
                    _timer = Time.time;

                if (_doesTimeout && Time.time - _timer > ConfigLoader.Instance.GetConfig().inactivity_time)
                {
                    _isActive = false;
                    Timeout();
                }
            }
        }
    }
}

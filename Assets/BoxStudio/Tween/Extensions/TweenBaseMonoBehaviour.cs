using UnityEngine;
using System.Collections;

namespace Box.Tween {
    public abstract class TweenBaseMonoBehaviour : MonoBehaviour {
        private TweenBase _tween = null;

        [Header("--- Base Conf ---")]
        public bool autoStart = true;
        public string onStart;
        public string onComplete;
        public TweenBaseMonoBehaviour[] nexts = { };

        public bool IsRunning {
            get { return _tween != null && _tween.isRunning; }
        }
        public bool IsFinished {
            get { return _tween != null && _tween.isFinished; }
        }
        public bool IsPaused {
            get { return _tween != null && _tween.isPaused; }
        }
        public TweenBase tween {
            get { return _tween; }
        }

        protected abstract TweenBase Build();
        protected virtual void BeforeStart() { }
        public virtual bool Begin(TweenHandler handler = null) {
            if (_tween != null && !IsRunning) {
                _tween.Begin(handler);
                return true;
            } else {
                return false;
            }
        }

        #region Unity Callback
        void Awake() {
            _tween = Build();
            if (!string.IsNullOrEmpty(onComplete)) {
                _tween.OnComplete(() => SendMessage(onComplete));
            }
            if (!string.IsNullOrEmpty(onStart)) {
                _tween.OnComplete(() => SendMessage(onStart));
            }
        }

        void Start() {
            if (_tween != null) {
                foreach (var next in nexts) {
                    _tween.AddNext(next.tween);
                }
            }
            BeforeStart();

            if (autoStart && _tween != null) {
                _tween.Begin();
            }
        }

        void OnDestroy() {
            if (_tween != null && _tween.isRunning) {
                _tween.Cancel();
            }
        }

        void OnEnable() {
            if (_tween != null) {
                _tween.Resume();
            }
        }

        void OnDisable() {
            if (_tween != null) {
                _tween.Pause();
            }
        }
        #endregion
    }
}
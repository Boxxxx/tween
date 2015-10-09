using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

namespace Box.Tween {
    /// <summary>
    /// The root class of Tween system,
    /// NOTICE: It must be repeatable, that is, you can restart it any times.
    /// </summary>
    public abstract class TweenBase : ICloneable {
        private static ulong global_id_counter = 0;

        protected GameObject owner_ = null;
        protected ITweenContainer container_ = null;

        public bool ignoreTimeScale { get; set; }
        public bool includeChildren { get; set; }
        public Action onComplete { get; set; }
        public Action<bool> onOver { get; set; }
        public Action onStart { get; set; }
        public TweenBase previous { get; private set; }

        public bool isPaused { get; private set; }
        public bool isRunning { get; private set; }
        public bool isFinished { get; private set; }
        public bool hasPrevious { get { return previous != null; } }
        public ulong uniqueId { get; private set; }

        protected List<TweenBase> next_tweens_ = new List<TweenBase>();

        public TweenBase[] nextTweens {
            get {
                return next_tweens_.ToArray();
            }
        }

        public GameObject owner {
            get { return owner_; }
            internal set {
                owner_ = value;
            }
        }

        public TweenBase(GameObject owner) {
            owner_ = owner;
            uniqueId = ++global_id_counter;
        }

        #region Internal
        internal virtual void OnStart(ITweenContainer container) {
            Assert.IsTrue(container != null);
            container_ = container;

            Reset();
            if (onStart != null) {
                onStart();
            }
            isRunning = true;
        }

        internal virtual void OnFinish() {
            if (!isRunning) {
                return;
            }
            isRunning = false;
            isFinished = true;
            if (onComplete != null) {
                onComplete();
            }
            if (onOver != null) {
                onOver(true);
            }
        }

        internal virtual void OnCancel() {
            if (!isRunning) {
                return;
            }
            isRunning = false;
            if (onOver != null) {
                onOver(false);
            }
        }

        internal abstract bool OnUpdate(float delta_time, out float remain_time);

        internal bool Update(float delta_time, out float remain_time) {
            if (isPaused || isFinished) {
                remain_time = 0;
                return false;
            } else {
                return OnUpdate(delta_time, out remain_time);
            }
        }
        internal virtual void Reset() {
            isRunning = isFinished = false;
        }
        #endregion

        #region Public Interface
        public bool Finish() {
            Assert.IsTrue(container_ != null);
            return container_.FinishTween(this);
        }
        public bool Cancel() {
            Assert.IsTrue(container_ != null);
            return container_.CancelTween(this);
        }
        public void Pause() {
            isPaused = true;
        }
        public void Resume() {
            isPaused = false;
        }
        public void AddNext(TweenBase tween) {
            Assert.IsTrue(!tween.hasPrevious);

            tween.previous = this;
            next_tweens_.Add(tween);
        }
        public void Begin(TweenHandler handler = null) {
            Assert.IsTrue(!isRunning);
            if (handler == null) {
                handler = TweenHandler.Instance;
            }
            handler.BeginTween(this);
        }

        public virtual object Clone() {
            var new_tween = MemberwiseClone() as TweenBase;
            new_tween.uniqueId = ++global_id_counter;
            return new_tween;
        }
        #endregion
    }
}

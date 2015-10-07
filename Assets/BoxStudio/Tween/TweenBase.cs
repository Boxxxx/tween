using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

namespace BoxStudio.Tween {
    public abstract class TweenBase : ICloneable {
        private static ulong global_id_counter = 0;

        protected GameObject owner_ = null;
        protected TweenHandler handler_ = null;

        public bool ignoreTimeScale { get; set; }
        public bool includeChildren { get; set; }
        public Action onComplete { get; set; }
        public Action<bool> onOver { get; set; }
        public Action onStart { get; set; }

        public bool IsPaused { get; private set; }
        public bool IsFinished { get; private set; }
        public ulong UniqueId { get; private set; }

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
            UniqueId = ++global_id_counter;
        }

        #region Internal
        internal virtual void OnStart(TweenHandler handler) {
            Assert.IsTrue(handler != null);
            handler_ = handler;

            Reset();
            if (onStart != null) {
                onStart();
            }
        }

        internal virtual void OnFinish() {
            if (onComplete != null) {
                onComplete();
            }
            if (onOver != null) {
                onOver(true);
            }
        }

        internal virtual void OnStop() {
            if (onOver != null) {
                onOver(false);
            }
        }

        internal abstract bool OnUpdate(float delta_time, out float remain_time);

        internal bool Update(float delta_time, out float remain_time) {
            if (IsPaused || IsFinished) {
                remain_time = 0;
                return false;
            } else {
                IsFinished = OnUpdate(delta_time, out remain_time);
                return IsFinished;
            }
        }
        internal abstract void Reset();
        #endregion

        #region Public Interface
        public void Finish() {
            Assert.IsTrue(handler_ != null);
            handler_.Finish(this);
        }
        public void Stop() {
            Assert.IsTrue(handler_ != null);
            handler_.Stop(this);
        }
        public void Pause() {
            IsPaused = true;
        }
        public void Resume() {
            IsPaused = false;
        }
        public void AddNext(TweenBase tween) {
            next_tweens_.Add(tween);
        }
        public void Begin(TweenHandler handler = null) {
            if (handler == null) {
                handler = TweenHandler.Instance;
            }
            handler.Begin(this);
        }

        public virtual object Clone() {
            var new_tween = MemberwiseClone() as TweenBase;
            new_tween.UniqueId = ++global_id_counter;
            return new_tween;
        }
        #endregion
    }
}

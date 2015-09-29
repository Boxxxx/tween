using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

namespace BoxStudio {
    public abstract class TweenBase {
        protected GameObject owner_ = null;
        protected TweenHandler handler_ = null;

        protected Action on_complete_;
        protected Action<bool> on_over_;
        protected Action on_start_;

        protected List<TweenBase> next_tweens_ = new List<TweenBase>();

        public TweenBase[] NextTweens {
            get {
                return next_tweens_.ToArray();
            }
        }

        public GameObject Owner {
            get { return owner_; }
        }

        public TweenBase(GameObject owner) {
            owner_ = owner;
        }

        public virtual void OnStart(TweenHandler handler) {
            Assert.IsTrue(handler != null);
            handler_ = handler;

            Reset();
            if (on_start_ != null) {
                on_start_();
            }
        }

        public virtual void OnFinish() {
            if (on_complete_ != null) {
                on_complete_();
            }
            if (on_over_ != null) {
                on_over_(true);
            }
        }

        public virtual void OnStop() {
            if (on_over_ != null) {
                on_over_(false);
            }
        }

        public abstract bool Update(float deltaTime, out float remainTime);

        public abstract void Reset();

        #region Set interface
        public TweenBase WhenComplete(Action onComplete) {
            on_complete_ += onComplete;
            return this;
        }
        public TweenBase WhenStart(Action onStart) {
            on_start_ += onStart;
            return this;
        }

        public TweenBase Then(TweenBase tween) {
            next_tweens_.Add(tween);
            return this;
        }
        public TweenBase Begin(TweenHandler handler = null) {
            if (handler == null) {
                handler = TweenHandler.Instance;
            }
            handler.Begin(this);
            return this;
        }
        #endregion
    }
}

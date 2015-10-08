using UnityEngine;
using UnityEngine.Assertions;

namespace Box.Tween {
    public abstract class TweenFromTo<T> : TweenDuration {
        // used to set default value in inherited class.
        protected T from_;
        protected T to_;
        protected T origin_value_;
        private bool has_from_ = false;
        private bool has_to_ = false;

        public T From {
            get {
                return from_;
            }
            set {
                has_from_ = true;
                from_ = value;
            }
        }
        public T To {
            get {
                return to_;
            }
            set {
                has_to_ = true;
                to_ = value;
            }
        }

        public TweenFromTo(GameObject owner, float duration) : base(owner, duration) { }
        public TweenFromTo(GameObject owner, float duration, T from, T to) : base(owner, duration) {
            From = from;
            To = to;
        }

        internal override void Reset() {
            base.Reset();

            Assert.IsTrue(has_from_ || has_to_);
            origin_value_ = GetValue();
            if (!has_from_) {
                from_ = origin_value_;
            }
            if (!has_to_) {
                to_ = origin_value_;
            }
        }
        protected override void OnUpdateValue(float value) {
            SetValue(LerpValue(From, To, value));
        }
        internal abstract T GetValue();
        internal abstract void SetValue(T value);
        internal abstract T LerpValue(T from, T to, float value);
    }
}
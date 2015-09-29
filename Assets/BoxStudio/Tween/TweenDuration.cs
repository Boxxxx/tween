using UnityEngine;
using System;

namespace BoxStudio {
    public abstract class TweenDuration : TweenBase {
        public enum LoopType {
            Once,
            Loop,
            Pingpong
        }
        protected EaseFuncs.EaseType ease_type_ = EaseFuncs.EaseType.Linear;
        protected LoopType loop_type_ = LoopType.Once;

        protected float duration_;
        protected Action<float> on_update_;
        protected Action<float> on_update_value_;
        protected Action<int> on_loop_;

        private EaseFuncs.EaseFuncDelegate ease_func_;
        private float now_time_ = 0;
        // -1 means infinity
        private int repeat_cnt_ = -1;
        private bool is_reverse_ = false;

        public TweenDuration(GameObject owner, float duration) : base(owner) {
            duration_ = duration;
        }

        public override void Reset() {
            now_time_ = 0;
            ease_func_ = EaseFuncs.GetEaseFunc(ease_type_);
            if (repeat_cnt_ > 0) {
                repeat_cnt_--;
            }
        }

        public override bool Update(float delta_time, out float remain_time) {
            remain_time = 0;
            now_time_ += delta_time;

            if (now_time_ >= duration_) {
                if (loop_type_ == LoopType.Once || repeat_cnt_ == 0) {
                    OnUpdateValue(is_reverse_ ? 0 : 1);
                    remain_time = now_time_ - duration_;
                    return true;
                }
                else {
                    while (now_time_ >= duration_) {
                        now_time_ -= duration_;
                        if (repeat_cnt_ > 0) {
                            repeat_cnt_--;
                        }
                        if (loop_type_ == LoopType.Pingpong) {
                            is_reverse_ = !is_reverse_;
                        }
                    }
                }
            }

            float value;
            if (!is_reverse_) {
                value = ease_func_(0, 1, now_time_ / duration_);
            }
            else {
                value = ease_func_(0, 1, 1 - now_time_ / duration_);
            }

            OnUpdateValue(value);

            if (on_update_ != null) {
                on_update_(now_time_);
            }
            if (on_update_value_ != null) {
                on_update_value_(value);
            }

            return false;
        }

        protected abstract void OnUpdateValue(float value);

        #region Set interface
        public TweenDuration SetEaseType(EaseFuncs.EaseType ease_type) {
            ease_type_ = ease_type;
            return this;
        }
        public TweenDuration SetLoopType(LoopType loop_type) {
            loop_type_ = loop_type;
            return this;
        }
        public TweenDuration SetRepeat(int repeat_cnt) {
            repeat_cnt_ = repeat_cnt;
            return this;
        }

        public TweenDuration WhenUpdate(Action<float> onUpdate) {
            on_update_ += onUpdate;
            return this;
        }
        public TweenDuration WhenUpdateValue(Action<float> onUpdateValue) {
            on_update_value_ += onUpdateValue;
            return this;
        }
        public TweenDuration WhenLoop(Action<int> onLoop) {
            on_loop_ += onLoop;
            return this;
        }
        #endregion
    }
}
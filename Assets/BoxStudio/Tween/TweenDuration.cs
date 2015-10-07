using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace BoxStudio.Tween {
    public abstract class TweenDuration : TweenBase {
        public enum LoopType {
            Once,
            Loop,
            Pingpong
        }
        #region Tween Data (should not be modified at runtime)
        protected EaseFuncs.EaseType ease_type_ = EaseFuncs.EaseType.Linear;
        public EaseFuncs.EaseType easeType {
            get { return ease_type_; }
            set { ease_type_ = value; }
        }
        protected LoopType loop_type_ = LoopType.Once;
        public LoopType loopType {
            get { return loop_type_; }
            set { loop_type_ = value; }
        }

        public float duration { get; private set; }
        public Action<float> onUpdate { get; set; }
        public Action<float> onUpdateValue { get; set; }
        public Action<int> onLoop { get; set; }
        // -1 means infinity
        private int repeat_cnt_ = -1;
        public int RepeatCnt { get { return repeat_cnt_; } set { repeat_cnt_ = value; } }
        #endregion

        private EaseFuncs.EaseFuncDelegate ease_func_;
        private float now_time_ = 0;
        private int repeat_cnt_dynamic_ = 0;
        private bool is_reverse_ = false;

        public TweenDuration(GameObject owner, float duration) : base(owner) {
            Assert.IsTrue(duration > 0);
            this.duration = duration;
        }

        internal override void Reset() {
            now_time_ = 0;
            ease_func_ = EaseFuncs.GetEaseFunc(ease_type_);
            // if the loopType is Pingpong, then repeat_cnt should multiplay 2.
            repeat_cnt_dynamic_ = RepeatCnt * (loopType == LoopType.Pingpong ? 2 : 1);
            is_reverse_ = false;
        }

        internal override bool OnUpdate(float delta_time, out float remain_time) {
            remain_time = 0;
            now_time_ += delta_time;

            if (now_time_ >= duration) {
                int pass_cnt = Mathf.FloorToInt(now_time_ / duration);
                if (loop_type_ == LoopType.Once 
                        || repeat_cnt_dynamic_ >= 0 && repeat_cnt_dynamic_ <= pass_cnt) {
                    OnUpdateValue(is_reverse_ ? 0 : 1);
                    remain_time = now_time_ - duration;
                    return true;
                }
                else {
                    now_time_ -= duration * pass_cnt;
                    if (repeat_cnt_dynamic_ >= 0) {
                        repeat_cnt_dynamic_ -= pass_cnt;
                    }
                    if (loop_type_ == LoopType.Pingpong) {
                        is_reverse_ = ((pass_cnt & 1) == 0) ? is_reverse_ : !is_reverse_;
                    }
                }
            }

            float value;
            if (!is_reverse_) {
                value = ease_func_(0, 1, now_time_ / duration);
            }
            else {
                value = ease_func_(0, 1, 1 - now_time_ / duration);
            }

            OnUpdateValue(value);

            if (onUpdate != null) {
                onUpdate(now_time_);
            }
            if (onUpdateValue != null) {
                onUpdateValue(value);
            }

            return false;
        }

        protected abstract void OnUpdateValue(float value);
    }
}
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Box.Tween {
    public class TweenWrapper : TweenBase, ITweenContainer {
        // configure variables section
        private int repeat_cnt_ = 1;
        private float time_limit_ = 0;
        private bool finish_all_when_timeout_ = true;
        private List<TweenBase> tweens_ = new List<TweenBase>();

        // dynamic variables section
        private int repeat_cnt_dynamic_ = 0;
        private float now_time_ = 0;
        private List<TweenBase> running_tweens_ = new List<TweenBase>();

        public int repeatCnt {
            get { return repeat_cnt_; }
            set { repeat_cnt_ = value; }
        }
        public float timeLimit {
            get { return time_limit_; }
            set { time_limit_ = value; }
        }
        public bool finishAllWhenTimeout {
            get { return finish_all_when_timeout_; }
            set { finish_all_when_timeout_ = value; }
        }
        public bool cancelAllWhenOneCanceled {
            get; set;
        }

        public TweenWrapper(GameObject owner) : base(owner) { }

        public void Add(TweenBase tween) {
            if (isRunning) {
                Debug.LogWarning("[Box.Tween] can not add new tween during running!");
                return;
            }
            tweens_.Add(tween);
        }
        public bool FinishTween(TweenBase tween) {
            if (running_tweens_.Contains(tween) && tween.isRunning) {
                Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();
                queue.Enqueue(Util.MakePair(tween, float.MaxValue));
                TweenHelper.UpdateQueue(queue, this);
                return true;
            } else {
                return false;
            }
        }
        public bool CancelTween(TweenBase tween) {
            if (running_tweens_.Contains(tween) && tween.isRunning) {
                running_tweens_.Remove(tween);
                // IF one canceled, the whole parallel is canceled.
                if (cancelAllWhenOneCanceled) {
                    Cancel();
                } else {
                    tween.OnCancel();
                }
                return true;
            } else {
                return false;
            }
        }
        public void OnTweenComplete(TweenBase tween) {
            running_tweens_.Remove(tween);
            tween.OnFinish();
        }
        public void BeginTween(TweenBase tween) {
            Assert.IsTrue(!tween.isRunning);

            if (tween.includeChildren) {
                // Clone and apply this tween to all children.
                for (var i = 0; i < tween.owner.transform.childCount; i++) {
                    var child = tween.owner.transform.GetChild(i);
                    var new_tween = TweenHelper.CloneAndApplyTo(tween, child.gameObject);
                    BeginTween(tween);
                }
            }

            running_tweens_.Add(tween);
            tween.OnStart(this);
        }

        internal override void Reset() {
            base.Reset();

            now_time_ = 0;
            repeat_cnt_dynamic_ = repeatCnt;

            running_tweens_.Clear();
            foreach (var tween in tweens_) {
                Assert.IsTrue(!tween.hasPrevious);

                tween.Reset();
                running_tweens_.Add(tween);
            }
        }
        internal override void OnStart(ITweenContainer container) {
            base.OnStart(container);

            foreach (var tween in running_tweens_) {
                tween.OnStart(this);
            }
        }
        internal override void OnCancel() {
            foreach (var tween in running_tweens_) {
                tween.Cancel();
            }

            running_tweens_.Clear();
            base.OnCancel();
        }

        internal override bool OnUpdate(float delta_time, out float remain_time) {
            Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();
            foreach (var tween in running_tweens_) {
                queue.Enqueue(
                    Util.MakePair(tween, delta_time));
            }

            float minRemainTime = TweenHelper.UpdateQueue(queue, this);
            if (running_tweens_.Count == 0) {
                remain_time = minRemainTime;
                return true;
            }

            remain_time = 0;
            now_time_ += delta_time;
            if (timeLimit > 0 && now_time_ >= timeLimit) {
                remain_time = now_time_ - timeLimit;
                if (finishAllWhenTimeout) {
                    Finish();
                }
                else {
                    Cancel();
                }
                return true;
            }
            else {
                remain_time = 0;
                return false;
            }
        }
    }
}

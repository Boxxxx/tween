using UnityEngine;
using System.Collections.Generic;

namespace BoxStudio {
    public class TweenHandler : MonoBehaviour {
        public static TweenHandler instance_;
        public static TweenHandler Instance {
            get {
                if (instance_ == null) {
                    var obj = new GameObject("~BoxTween");
                    DontDestroyOnLoad(obj);
                    instance_ = obj.AddComponent<TweenHandler>();
                }
                return instance_;
            }
        }

        private List<TweenBase> tweens_ = new List<TweenBase>();
        private Dictionary<GameObject, List<TweenBase>> tweens_dict_ = new Dictionary<GameObject, List<TweenBase>>();

        public void Begin(TweenBase tween) {
            tweens_.Add(tween);
            if (tween.Owner != null) {
                if (!tweens_dict_.ContainsKey(tween.Owner)) {
                    tweens_dict_[tween.Owner] = new List<TweenBase>();
                }
                tweens_dict_[tween.Owner].Add(tween);
            }
            tween.OnStart(this);
        }

        public void Finish(TweenBase tween) {
            Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();
            queue.Enqueue(Util.MakePair(tween, float.MaxValue));
            UpdateQueue(queue);
        }
        public void FinishAll(TweenBase[] tweens) {
            Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();
            foreach (var tween in tweens) {
                queue.Enqueue(Util.MakePair(tween, float.MaxValue));
            }
            UpdateQueue(queue);
        }
        public void FinishAll(GameObject owner) {
            if (tweens_dict_.ContainsKey(owner)) {
                Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();
                foreach (var tween in tweens_dict_[owner]) {
                    queue.Enqueue(Util.MakePair(tween, float.MaxValue));
                }
                UpdateQueue(queue);
            }
        }
        public void FinishAll() {
            UpdateWithDelta(float.MaxValue);
        }

        public void Stop(TweenBase tween) {
            tweens_.Remove(tween);
            if (tween.Owner != null) {
                tweens_dict_[tween.Owner].Remove(tween);
            }
            tween.OnStop();
        }
        public void StopAll(TweenBase[] tweens) {
            foreach (var tween in tweens) {
                Stop(tween);
            }
        }
        public void StopAll(GameObject owner) {
            if (tweens_dict_.ContainsKey(owner)) {
                foreach (var tween in tweens_dict_[owner]) {
                    tweens_.Remove(tween);
                    tween.OnStop();
                }
                tweens_dict_[owner].Clear();
            }
        }
        public void StopAll() {
            foreach (var tween in tweens_) {
                tween.OnStop();
            }
            tweens_.Clear();
            tweens_dict_.Clear();
        }

        private void DoComplete(TweenBase tween) {
            tweens_.Remove(tween);
            if (tween.Owner != null) {
                tweens_dict_[tween.Owner].Remove(tween);
            }
            tween.OnFinish();
        }

        private void UpdateWithDelta(float deltaTime) {
            Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();

            foreach (var tween in tweens_) {
                queue.Enqueue(Util.MakePair(tween, deltaTime));
            }

            UpdateQueue(queue);
        }

        private void UpdateQueue(Queue<KeyValuePair<TweenBase, float>> queue) {
            while (queue.Count > 0) {
                var tweenData = queue.Dequeue();
                var tween = tweenData.Key;
                var delta = tweenData.Value;

                float remainTime;
                if (tween.Update(delta, out remainTime)) {
                    DoComplete(tween);
                    remainTime = Mathf.Clamp(remainTime, 0, 1);
                    foreach (var next in tween.NextTweens) {
                        Begin(next);
                        if (remainTime > 0) {
                            queue.Enqueue(Util.MakePair(next, remainTime));
                        }
                    }
                }
            }
        }

        void Update() {
            UpdateWithDelta(Time.deltaTime);
        }
    }
}

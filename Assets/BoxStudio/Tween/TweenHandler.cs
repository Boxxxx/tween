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
        private Dictionary<ulong, TweenBase> tweens_id_map_ = new Dictionary<ulong, TweenBase>();
        private Dictionary<GameObject, List<TweenBase>> tweens_obj_map_ = new Dictionary<GameObject, List<TweenBase>>();

        public void Begin(TweenBase tween) {
            if (tween.includeChildren) {
                // Clone and apply this tween to all children.
                for (var i = 0; i < tween.owner.transform.childCount; i++) {
                    var child = tween.owner.transform.GetChild(i);
                    var new_tween = CloneAndApplyTo(tween, child.gameObject);
                    Begin(tween);
                }
            }

            tweens_.Add(tween);
            if (tween.owner != null) {
                if (!tweens_obj_map_.ContainsKey(tween.owner)) {
                    tweens_obj_map_[tween.owner] = new List<TweenBase>();
                }
                tweens_obj_map_[tween.owner].Add(tween);
                tweens_id_map_.Add(tween.UniqueId, tween);
            }
            tween.OnStart(this);
        }

        public TweenBase FindById(ulong unique_id) {
            if (tweens_id_map_.ContainsKey(unique_id)) {
                return tweens_id_map_[unique_id];
            }
            else {
                return null;
            }
        }
        public bool IsTweening(ulong unique_id) {
            var tween = FindById(unique_id);
            return tween == null ? false : !tween.IsFinished;
        }
        public int HowManyTween(GameObject owner) {
            if (tweens_obj_map_.ContainsKey(owner)) {
                return tweens_obj_map_[owner].Count;
            }
            else {
                return 9;
            }
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
        public void Finish(ulong unique_id) {
            var tween = FindById(unique_id);
            if (tween != null) {
                Finish(unique_id);
            }
        }
        public void FinishAll(ulong[] unique_ids) {
            foreach (var unique_id in unique_ids) {
                var tween = FindById(unique_id);
                if (tween != null) {
                    Finish(tween);
                }
            }
        }
        public void FinishAll(GameObject owner) {
            if (tweens_obj_map_.ContainsKey(owner)) {
                Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();
                foreach (var tween in tweens_obj_map_[owner]) {
                    queue.Enqueue(Util.MakePair(tween, float.MaxValue));
                }
                UpdateQueue(queue);
            }
        }
        public void FinishAll() {
            UpdateWithDelta(float.MaxValue, float.MaxValue);
        }

        public void Stop(TweenBase tween) {
            tweens_.Remove(tween);
            if (tween.owner != null) {
                tweens_obj_map_[tween.owner].Remove(tween);
            }
            tween.OnStop();
        }
        public void StopAll(TweenBase[] tweens) {
            foreach (var tween in tweens) {
                Stop(tween);
            }
        }
        public void Stop(ulong unique_id) {
            var tween = FindById(unique_id);
            if (tween != null) {
                Stop(unique_id);
            }
        }
        public void StopAll(ulong[] unique_ids) {
            foreach (var unique_id in unique_ids) {
                var tween = FindById(unique_id);
                if (tween != null) {
                    Stop(tween);
                }
            }
        }
        public void StopAll(GameObject owner) {
            if (tweens_obj_map_.ContainsKey(owner)) {
                foreach (var tween in tweens_obj_map_[owner]) {
                    tweens_.Remove(tween);
                    tween.OnStop();
                }
                tweens_obj_map_[owner].Clear();
            }
        }
        public void StopAll() {
            foreach (var tween in tweens_) {
                tween.OnStop();
            }
            tweens_.Clear();
            tweens_obj_map_.Clear();
        }

        public void Pause(TweenBase tween) {
            if (tweens_.Contains(tween)) {
                tween.Pause();
            }
        }
        public void PauseAll(TweenBase[] tweens) {
            foreach (var tween in tweens) {
                tween.Pause();
            }
        }
        public void Pause(ulong unique_id) {
            var tween = FindById(unique_id);
            if (tween != null) {
                Pause(unique_id);
            }
        }
        public void PauseAll(ulong[] unique_ids) {
            foreach (var unique_id in unique_ids) {
                var tween = FindById(unique_id);
                if (tween != null) {
                    Pause(tween);
                }
            }
        }
        public void PauseAll(GameObject owner) {
            if (tweens_obj_map_.ContainsKey(owner)) {
                foreach (var tween in tweens_obj_map_[owner]) {
                    tween.Pause();
                }
            }
        }
        public void PauseAll() {
            foreach (var tween in tweens_) {
                tween.Pause();
            }
        }

        public void Resume(TweenBase tween) {
            if (tweens_.Contains(tween)) {
                tween.Resume();
            }
        }
        public void ResumeAll(TweenBase[] tweens) {
            foreach (var tween in tweens) {
                tween.Resume();
            }
        }
        public void Resume(ulong unique_id) {
            var tween = FindById(unique_id);
            if (tween != null) {
                Resume(unique_id);
            }
        }
        public void ResumeAll(ulong[] unique_ids) {
            foreach (var unique_id in unique_ids) {
                var tween = FindById(unique_id);
                if (tween != null) {
                    Resume(tween);
                }
            }
        }
        public void ResumeAll(GameObject owner) {
            if (tweens_obj_map_.ContainsKey(owner)) {
                foreach (var tween in tweens_obj_map_[owner]) {
                    tween.Resume();
                }
            }
        }
        public void ResumeAll() {
            foreach (var tween in tweens_) {
                tween.Resume();
            }
        }

        private void DoComplete(TweenBase tween) {
            tweens_.Remove(tween);
            if (tween.owner != null) {
                tweens_obj_map_[tween.owner].Remove(tween);
            }
            tween.OnFinish();
        }
        private void UpdateWithDelta(float deltaTime, float unscaledDeltaTime) {
            Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();

            foreach (var tween in tweens_) {
                queue.Enqueue(
                    Util.MakePair(tween, tween.ignoreTimeScale ? unscaledDeltaTime : deltaTime));
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
                    foreach (var next in tween.nextTweens) {
                        Begin(next);
                        if (remainTime > 0) {
                            queue.Enqueue(Util.MakePair(next, remainTime));
                        }
                    }
                }
            }
        }
        private TTween CloneAndApplyTo<TTween>(TTween tween, GameObject other) where TTween : TweenBase {
            var new_tween = tween.Clone() as TTween;
            new_tween.owner = other;
            return new_tween;
        }

        void Update() {
            UpdateWithDelta(Time.deltaTime, Time.unscaledDeltaTime);
        }
    }
}

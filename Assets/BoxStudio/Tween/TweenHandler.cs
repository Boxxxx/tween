using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

namespace Box.Tween {
    public class TweenHandler : MonoBehaviour, ITweenContainer {
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

            tweens_.Add(tween);
            if (tween.owner != null) {
                if (!tweens_obj_map_.ContainsKey(tween.owner)) {
                    tweens_obj_map_[tween.owner] = new List<TweenBase>();
                }
                tweens_obj_map_[tween.owner].Add(tween);
                tweens_id_map_.Add(tween.uniqueId, tween);
            }
            tween.OnStart(this);
        }
        public void OnTweenComplete(TweenBase tween) {
            tweens_.Remove(tween);
            if (tween.owner != null) {
                tweens_obj_map_[tween.owner].Remove(tween);
            }
            tween.OnFinish();
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
            return tween == null ? false : !tween.isFinished;
        }
        public int HowManyTween(GameObject owner) {
            if (tweens_obj_map_.ContainsKey(owner)) {
                return tweens_obj_map_[owner].Count;
            }
            else {
                return 9;
            }
        }

        public bool FinishTween(TweenBase tween) {
            if (tweens_.Contains(tween) && tween.isRunning) {
                Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();
                queue.Enqueue(Util.MakePair(tween, float.MaxValue));
                TweenHelper.UpdateQueue(queue, this);
                return true;
            } else {
                Debug.LogWarning("[Box.Tween] tween is not running in this handler!");
                return false;
            }
        }
        public void FinishAll(TweenBase[] tweens) {
            Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();
            foreach (var tween in tweens) {
                queue.Enqueue(Util.MakePair(tween, float.MaxValue));
            }
            TweenHelper.UpdateQueue(queue, this);
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
                    FinishTween(tween);
                }
            }
        }
        public void FinishAll(GameObject owner) {
            if (tweens_obj_map_.ContainsKey(owner)) {
                Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();
                foreach (var tween in tweens_obj_map_[owner]) {
                    queue.Enqueue(Util.MakePair(tween, float.MaxValue));
                }
                TweenHelper.UpdateQueue(queue, this);
            }
        }
        public void FinishAll() {
            UpdateWithDelta(float.MaxValue, float.MaxValue);
        }

        public bool CancelTween(TweenBase tween) {
            if (tween.isRunning && tweens_.Contains(tween)) {
                tweens_.Remove(tween);
                if (tween.owner != null) {
                    tweens_obj_map_[tween.owner].Remove(tween);
                }
                tween.OnCancel();
                return true;
            } else {
                Debug.LogWarning("[Box.Tween] tween is not running in this handler!");
                return false;
            }
        }
        public void CancelAll(TweenBase[] tweens) {
            foreach (var tween in tweens) {
                CancelTween(tween);
            }
        }
        public void Cancel(ulong unique_id) {
            var tween = FindById(unique_id);
            if (tween != null) {
                Cancel(unique_id);
            }
        }
        public void CancelAll(ulong[] unique_ids) {
            foreach (var unique_id in unique_ids) {
                var tween = FindById(unique_id);
                if (tween != null) {
                    CancelTween(tween);
                }
            }
        }
        public void CancelAll(GameObject owner) {
            if (tweens_obj_map_.ContainsKey(owner)) {
                foreach (var tween in tweens_obj_map_[owner]) {
                    tweens_.Remove(tween);
                    tween.OnCancel();
                }
                tweens_obj_map_[owner].Clear();
            }
        }
        public void CancelAll() {
            foreach (var tween in tweens_) {
                tween.OnCancel();
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

        private void UpdateWithDelta(float deltaTime, float unscaledDeltaTime) {
            Queue<KeyValuePair<TweenBase, float>> queue = new Queue<KeyValuePair<TweenBase, float>>();

            foreach (var tween in tweens_) {
                queue.Enqueue(
                    Util.MakePair(tween, tween.ignoreTimeScale ? unscaledDeltaTime : deltaTime));
            }

            TweenHelper.UpdateQueue(queue, this);
        }

        void Update() {
            UpdateWithDelta(Time.deltaTime, Time.unscaledDeltaTime);
        }
    }
}

using UnityEngine;

namespace BoxStudio {
    public class TweenPosition : TweenDuration {
        private GameObject owner_ = null;
        private Vector3 from_;
        private Vector3 to_;
        private bool is_local_ = false;

        public TweenPosition(GameObject owner, float duration, Vector3 from, Vector2 to)
                : base(owner, duration) {
            owner_ = owner;
            from_ = from;
            to_ = to;
        }

        protected override void OnUpdateValue(float value) {
            if (is_local_) {
                owner_.transform.localPosition = Vector3.Lerp(from_, to_, value);
            }
            else {
                owner_.transform.position = Vector3.Lerp(from_, to_, value);
            }
        }

        #region Set interface
        public TweenPosition IsLocal(bool is_local) {
            is_local_ = is_local;
            return this;
        }
        #endregion
    }
}
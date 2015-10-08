using UnityEngine;

namespace Box.Tween {
    public class TweenRotate : TweenFromTo<Vector3> {
        private bool is_local_ = true;
        public bool isLocal {
            get {
                return is_local_;
            }
            set {
                is_local_ = value;
            }
        }

        public TweenRotate(GameObject owner, float duration)
                : base(owner, duration) { }
        public TweenRotate(GameObject owner, float duration, Vector3 from, Vector3 to)
                : base(owner, duration, from, to) { }

        internal override Vector3 GetValue() {
            return isLocal ? owner.transform.localEulerAngles : owner.transform.eulerAngles;
        }
        internal override void SetValue(Vector3 value) {
            if (isLocal) {
                owner.transform.localEulerAngles = value;
            }
            else {
                owner.transform.eulerAngles = value;
            }
        }
        internal override Vector3 LerpValue(Vector3 from, Vector3 to, float value) {
            return Vector3.Lerp(from, to, value);
        }
    }
}

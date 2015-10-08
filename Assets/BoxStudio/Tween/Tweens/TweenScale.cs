using UnityEngine;

namespace Box.Tween {
    public class TweenScale : TweenFromTo<Vector3> {
        public bool isMultiply { get; set; }

        public TweenScale(GameObject owner, float duration)
                : base(owner, duration) { }
        public TweenScale(GameObject owner, float duration, Vector3 from, Vector3 to)
                : base(owner, duration, from, to) { }

        internal override Vector3 GetValue() {
            return owner.transform.localScale;
        }
        internal override void SetValue(Vector3 value) {
            if (isMultiply) {
                owner.transform.localScale =
                    new Vector3(origin_value_.x * value.x,
                                origin_value_.y * value.y,
                                origin_value_.z * value.z);
            } else {
                owner.transform.localScale = value;
            }
        }
        internal override Vector3 LerpValue(Vector3 from, Vector3 to, float value) {
            return Vector3.Lerp(from, to, value);
        }

        public TweenScale IsMultiply(bool is_multiply) {
            isMultiply = is_multiply;
            return this;
        }
    }
}

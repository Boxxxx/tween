using System;
using UnityEngine;

namespace BoxStudio {
    public class TweenMove : TweenFromTo<Vector3> {
        public bool isLocal { get; set; }

        public TweenMove(GameObject owner, float duration)
                : base(owner, duration) { }
        public TweenMove(GameObject owner, float duration, Vector3 from, Vector3 to)
                : base(owner, duration, from, to) { }

        internal override Vector3 GetValue() {
            return isLocal ? owner.transform.localPosition : owner.transform.position;
        }
        internal override void SetValue(Vector3 value) {
            if (isLocal) {
                owner.transform.localPosition = value;
            } else {
                owner.transform.position = value;
            }
        }
        internal override Vector3 LerpValue(Vector3 from, Vector3 to, float value) {
            return Vector3.Lerp(from, to, value);
        }
    }
}
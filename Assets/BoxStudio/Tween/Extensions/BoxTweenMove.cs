using UnityEngine;
using UnityEngine.Assertions;

namespace Box.Tween {
    public class BoxTweenMove : TweenBaseMonoBehaviour {
        [Header("--- Tween Data ---")]
        public float time = 1;
        public bool isLocal = false;

        [Space(10)]
        public bool currentAsFrom = false;
        public Vector3 from = Vector3.zero;
        public Transform fromTrans = null;

        [Space(10)]
        public bool currentAsTo = false;
        public Vector3 to = Vector3.zero;
        public Transform toTrans = null;

        [Space(10)]
        public EaseFuncs.EaseType easeType = EaseFuncs.EaseType.Linear;
        public TweenDuration.LoopType loopType = TweenDuration.LoopType.Once;
        public int repeatCnt = 1;

        protected override TweenBase Build() {
            Assert.IsFalse(currentAsFrom && currentAsTo);

            var tween = Tweens.Move(gameObject, time, isLocal)
                         .SetEaseType(easeType)
                         .SetLoopType(loopType)
                         .SetRepeat(repeatCnt);
            if (!currentAsFrom) {
                tween.From = from;
            }
            if (!currentAsTo) {
                tween.To = to;
            }

            return tween;
        }

        public override bool Begin(TweenHandler handler = null) {
            if (!currentAsFrom && fromTrans != null) {
                (tween as TweenMove).From = isLocal ? fromTrans.localPosition : fromTrans.position;
            }
            if (!currentAsTo && toTrans != null) {
                (tween as TweenMove).To = isLocal ? toTrans.localPosition : toTrans.position;
            }

            return base.Begin(handler);
        }
    }
}

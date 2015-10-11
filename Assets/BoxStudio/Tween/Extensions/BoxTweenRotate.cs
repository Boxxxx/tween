using UnityEngine;
using UnityEngine.Assertions;

namespace Box.Tween {
    public class BoxTweenRotate : TweenBaseMonoBehaviour {
        [Header("--- Tween Data ---")]
        public float time = 1;
        public bool isLocal = true;

        [Space(10)]
        public bool currentAsFrom = false;
        public Vector3 from = Vector3.zero;

        [Space(10)]
        public bool currentAsTo = false;
        public Vector3 to = Vector3.zero;

        [Space(10)]
        public EaseFuncs.EaseType easeType = EaseFuncs.EaseType.Linear;
        public TweenDuration.LoopType loopType = TweenDuration.LoopType.Once;
        public int repeatCnt = 1;

        protected override TweenBase Build() {
            Assert.IsFalse(currentAsFrom && currentAsTo);

            var tween = Tweens.Rotate(gameObject, time, isLocal)
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
    }
}

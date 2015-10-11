using UnityEngine;
using UnityEngine.Assertions;

namespace Box.Tween {
    public class BoxTweenScale : TweenBaseMonoBehaviour {
        [Header("--- Tween Data ---")]
        public float time = 1;

        [Space(10)]
        public bool currentAsFrom = false;
        public Vector3 from = new Vector3(1, 1, 1);

        [Space(10)]
        public bool currentAsTo = false;
        public Vector3 to = new Vector3(1, 1, 1);

        [Space(10)]
        public EaseFuncs.EaseType easeType = EaseFuncs.EaseType.Linear;
        public TweenDuration.LoopType loopType = TweenDuration.LoopType.Once;
        public int repeatCnt = 1;

        protected override TweenBase Build() {
            Assert.IsFalse(currentAsFrom && currentAsTo);

            var tween = Tweens.Scale(gameObject, time)
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

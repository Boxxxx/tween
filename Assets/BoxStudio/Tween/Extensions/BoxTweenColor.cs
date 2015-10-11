using UnityEngine;
using UnityEngine.Assertions;

namespace Box.Tween {
    public class BoxTweenColor : TweenBaseMonoBehaviour {
        [Header("--- Tween Data ---")]
        public float time = 1;

        [Space(10)]
        public bool currentAsFrom = false;
        public Color from;

        [Space(10)]
        public bool currentAsTo = false;
        public Color to;

        [Space(10)]
        public EaseFuncs.EaseType easeType = EaseFuncs.EaseType.Linear;
        public TweenDuration.LoopType loopType = TweenDuration.LoopType.Once;
        public int repeatCnt = 1;

        protected override TweenBase Build() {
            Assert.IsFalse(currentAsFrom && currentAsTo);

            var tween = Tweens.Color(gameObject, time)
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

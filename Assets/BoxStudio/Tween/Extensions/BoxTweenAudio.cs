using UnityEngine;
using UnityEngine.Assertions;

namespace Box.Tween {
    public class BoxTweenAudio : TweenBaseMonoBehaviour {
        [Header("--- Tween Data ---")]
        public float time = 1;

        [Space(10)]
        public bool currentAsFrom = false;
        [Range(0.0f, 1.0f)]
        public float fromVolume = 1;
        public float fromPitch = 1;

        [Space(10)]
        public bool currentAsTo = false;
        [Range(0.0f, 1.0f)]
        public float toVolume = 0;
        public float toPitch = 1;

        [Space(10)]
        public EaseFuncs.EaseType easeType = EaseFuncs.EaseType.Linear;
        public TweenDuration.LoopType loopType = TweenDuration.LoopType.Once;
        public int repeatCnt = 1;

        protected override TweenBase Build() {
            Assert.IsFalse(currentAsFrom && currentAsTo);

            var tween = Tweens.Audio(gameObject, time)
                         .SetEaseType(easeType)
                         .SetLoopType(loopType)
                         .SetRepeat(repeatCnt);
            if (!currentAsFrom) {
                tween.From = Util.MakePair(fromVolume, fromPitch);
            }
            if (!currentAsTo) {
                tween.To = Util.MakePair(toVolume, toPitch);
            }

            return tween;
        }
    }
}
namespace Box.Tween {
    public class BoxTweenFade : TweenBaseMonoBehaviour {
        public float time = 1;
        [Range(0.0f, 1.0f)]
        public float from = 1;
        [Range(0.0f, 1.0f)]
        public float to = 0;
        public EaseFuncs.EaseType easeType = EaseFuncs.EaseType.Linear;
        public TweenDuration.LoopType loopType = TweenDuration.LoopType.Once;
        public int repeatCnt = 1;
        
        protected override TweenBase Build() {
            return Tweens.Fade(gameObject, from, to, time)
                         .SetEaseType(easeType)
                         .SetLoopType(loopType)
                         .SetRepeat(repeatCnt);
        }
    }
}

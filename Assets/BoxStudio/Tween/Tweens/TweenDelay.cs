namespace BoxStudio.Tween {
    public class TweenDelay : TweenDuration {
        public TweenDelay(float duration) : base(null, duration) { }

        protected override void OnUpdateValue(float value) { }
    }
}
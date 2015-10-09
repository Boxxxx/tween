namespace Box.Tween {
    public interface ITweenContainer {
        bool FinishTween(TweenBase tween);
        bool CancelTween(TweenBase tween);
        void OnTweenComplete(TweenBase tween);
        void BeginTween(TweenBase tween);
    }
}

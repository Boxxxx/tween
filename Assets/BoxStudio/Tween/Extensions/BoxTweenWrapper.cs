using UnityEngine;

namespace Box.Tween {
    public class BoxTweenWrapper : TweenBaseMonoBehaviour {
        [Header("--- Tween Data ---")]
        public TweenBaseMonoBehaviour[] tweens = { };

        protected override TweenBase Build() {
            return new TweenWrapper(gameObject);
        }
        protected override void BeforeStart() {
            foreach (var tween_behaviour in tweens) {
                (tween as TweenWrapper).Add(tween_behaviour.tween);
            }
        }
    }
}

using UnityEngine;

namespace BoxStudio {
    public class Tweens {
        public static TweenDelay Delay(float duration) {
            return new TweenDelay(duration);
        }
        public static TweenPosition Position(GameObject owner, float duration, Vector3 from, Vector3 to) {
            return new TweenPosition(owner, duration, from, to);
        }
    }
}
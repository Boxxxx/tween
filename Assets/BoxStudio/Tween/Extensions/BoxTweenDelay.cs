using UnityEngine;
using UnityEngine.Assertions;

namespace Box.Tween {
    public class BoxTweenDelay : TweenBaseMonoBehaviour {
        [Header("--- Tween Data ---")]
        public float time = 1;

        protected override TweenBase Build() {
            return Tweens.Delay(time);
        }
    }
}

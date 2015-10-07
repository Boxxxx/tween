using UnityEngine;
using System.Collections;
using BoxStudio;

public class TweenTest : MonoBehaviour {
    void Start() {
        Tweens.Delay(5)
              .Then(Tweens.MoveAdd(gameObject, new Vector3(0, 5, 0), 3)
                          .SetEaseType(EaseFuncs.EaseType.EaseOutCubic)
                          .SetLoopType(TweenDuration.LoopType.Pingpong)
                          .SetRepeat(5))
              .Then(Tweens.Fade(gameObject, 1, 0, 3)
                          .SetLoopType(TweenDuration.LoopType.Pingpong))
              .Then(Tweens.RotateAdd(gameObject, new Vector3(0, 0, 180), 3)
                          .SetLoopType(TweenDuration.LoopType.Pingpong))
              .Then(Tweens.ScaleBy(gameObject, new Vector3(2, 2, 2), 3)
                          .SetLoopType(TweenDuration.LoopType.Pingpong))
              .Begin();
    }
}

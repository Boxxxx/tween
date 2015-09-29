using UnityEngine;
using System.Collections;
using BoxStudio;

public class TweenTest : MonoBehaviour {
    void Start() {
        Tweens.Delay(5)
              .Then(Tweens.Position(gameObject, 3, transform.position, transform.position + new Vector3(0, 5, 0))
                          .SetEaseType(EaseFuncs.EaseType.EaseOutCubic)
                          .SetLoopType(TweenDuration.LoopType.Pingpong)
                          .SetRepeat(5))
              .Begin();
    }
}

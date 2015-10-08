using UnityEngine;
using Box.Tween;

public class TweenTest : MonoBehaviour {
    public GameObject objA;
    public GameObject objB;

    void Start() {
        Tweens.Sequence(
            Tweens.Parallel(
                Tweens.Delay(5)
                      .Then(Tweens.Fade(objA, 1, 0, 2)
                                  .SetLoopType(TweenDuration.LoopType.Pingpong)
                                  .SetRepeat(1),
                            Tweens.ScaleBy(objA, new Vector3(2, 2, 1), 2)
                                  .SetLoopType(TweenDuration.LoopType.Pingpong)
                                  .SetRepeat(1)),
                Tweens.Delay(3)
                      .Then(Tweens.MoveAdd(objB, new Vector3(0, 5, 0), 2)
                                  .SetLoopType(TweenDuration.LoopType.Pingpong)
                                  .SetRepeat(2))
            ),
            Tweens.Parallel(
                Tweens.RotateAdd(objA, new Vector3(0, 0, 360), 3)
                        .SetLoopType(TweenDuration.LoopType.Pingpong)
                        .SetRepeat(1),
                Tweens.RotateAdd(objB, new Vector3(0, 0, 360), 3)
                        .SetLoopType(TweenDuration.LoopType.Pingpong)
                        .SetRepeat(1))
        )
        .WhenStart(() => Debug.Log("Start!"))
        .WhenComplete(() => Debug.Log("Finished!"))
        .Begin();
    }
}

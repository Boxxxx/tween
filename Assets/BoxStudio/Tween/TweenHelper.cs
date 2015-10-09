using UnityEngine;
using System.Collections.Generic;

namespace Box.Tween {
    public static class TweenHelper {
        internal static float UpdateQueue(Queue<KeyValuePair<TweenBase, float>> queue, ITweenContainer container) {
            float minRemainTime = float.MaxValue;
            while (queue.Count > 0) {
                var tweenData = queue.Dequeue();
                var tween = tweenData.Key;
                var delta = tweenData.Value;

                float remainTime;
                if (tween.Update(delta, out remainTime)) {
                    minRemainTime = Mathf.Min(minRemainTime, remainTime);
                    container.OnTweenComplete(tween);
                    remainTime = Mathf.Clamp(remainTime, 0, 1);
                    foreach (var next in tween.nextTweens) {
                        container.BeginTween(next);
                        if (remainTime > 0) {
                            queue.Enqueue(Util.MakePair(next, remainTime));
                        }
                    }
                }
            }

            return minRemainTime;
        }
        internal static TTween CloneAndApplyTo<TTween>(TTween tween, GameObject other) where TTween : TweenBase {
            var new_tween = tween.Clone() as TTween;
            new_tween.owner = other;
            return new_tween;
        }
    }
}

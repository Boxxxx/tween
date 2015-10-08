using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace Box.Tween {
    public static class Tweens {
        #region Factory functions
        public static TweenDelay Delay(float duration) {
            return new TweenDelay(duration);
        }

        public static TweenMove Move(GameObject owner, Vector3 from, Vector3 to, float duration, bool is_local = false) {
            var tween = new TweenMove(owner, duration, from, to);
            tween.isLocal = is_local;
            return tween;
        }
        public static TweenMove MoveFrom(GameObject owner, Vector3 from, float duration, bool is_local = false) {
            var tween = new TweenMove(owner, duration);
            tween.From = from;
            tween.isLocal = is_local;
            return tween;
        }
        public static TweenMove MoveTo(GameObject owner, Vector3 to, float duration, bool is_local = false) {
            var tween = new TweenMove(owner, duration);
            tween.To = to;
            tween.isLocal = is_local;
            return tween;
        }
        public static TweenMove MoveAdd(GameObject owner, Vector3 delta, float duration, bool is_local = false) {
            var tween = new TweenMove(owner, duration);
            tween.To = tween.GetValue() + delta;
            tween.isLocal = is_local;
            return tween;
        }

        public static TweenRotate Rotate(GameObject owner, Vector3 from, Vector3 to, float duration, bool is_local = false) {
            var tween = new TweenRotate(owner, duration, from, to);
            tween.isLocal = is_local;
            return tween;
        }
        public static TweenRotate RotateFrom(GameObject owner, Vector3 from, float duration, bool is_local = false) {
            var tween = new TweenRotate(owner, duration);
            tween.From = from;
            tween.isLocal = is_local;
            return tween;
        }
        public static TweenRotate RotateTo(GameObject owner, Vector3 to, float duration, bool is_local = false) {
            var tween = new TweenRotate(owner, duration);
            tween.To = to;
            tween.isLocal = is_local;
            return tween;
        }
        public static TweenRotate RotateAdd(GameObject owner, Vector3 delta, float duration, bool is_local = false) {
            var tween = new TweenRotate(owner, duration);
            tween.To = tween.GetValue() + delta;
            tween.isLocal = is_local;
            return tween;
        }

        public static TweenScale Scale(GameObject owner, Vector3 from, Vector3 to, float duration) {
            var tween = new TweenScale(owner, duration, from, to);
            return tween;
        }
        public static TweenScale ScaleFrom(GameObject owner, Vector3 from, float duration) {
            var tween = new TweenScale(owner, duration);
            tween.From = from;
            return tween;
        }
        public static TweenScale ScaleTo(GameObject owner, Vector3 to, float duration) {
            var tween = new TweenScale(owner, duration);
            tween.To = to;
            return tween;
        }
        public static TweenScale ScaleAdd(GameObject owner, Vector3 delta, float duration) {
            var tween = new TweenScale(owner, duration);
            tween.To = tween.GetValue() + delta;
            return tween;
        }
        public static TweenScale ScaleBy(GameObject owner, Vector3 scale, float duration) {
            var tween = new TweenScale(owner, duration, new Vector3(1, 1, 1), scale);
            tween.isMultiply = true;
            return tween;
        }

        public static TweenColor Color(GameObject owner, Color from, Color to, float duration) {
            var tween = new TweenColor(owner, duration, from, to);
            return tween;
        }
        public static TweenColor ColorFrom(GameObject owner, Color from, float duration) {
            var tween = new TweenColor(owner, duration);
            tween.From = from;
            return tween;
        }
        public static TweenColor ColorTo(GameObject owner, Color to, float duration) {
            var tween = new TweenColor(owner, duration);
            tween.To = to;
            return tween;
        }
        public static TweenColor ColorAdd(GameObject owner, Color delta, float duration) {
            var tween = new TweenColor(owner, duration);
            tween.To = tween.GetValue() + delta;
            return tween;
        }

        public static TweenAlpha Fade(GameObject owner, float from, float to, float duration) {
            var tween = new TweenAlpha(owner, duration, from, to);
            return tween;
        }
        public static TweenAlpha FadeFrom(GameObject owner, float from, float duration) {
            var tween = new TweenAlpha(owner, duration);
            tween.From = from;
            return tween;
        }
        public static TweenAlpha FadeTo(GameObject owner, float to, float duration) {
            var tween = new TweenAlpha(owner, duration);
            tween.To = to;
            return tween;
        }
        public static TweenAlpha FadeAdd(GameObject owner, float delta, float duration) {
            var tween = new TweenAlpha(owner, duration);
            tween.To = tween.GetValue() + delta;
            return tween;
        }

        public static TweenAudio Audio(GameObject owner, float from_volume, float from_pitch, float to_volumne, float to_pitch, float duration) {
            var tween = new TweenAudio(owner, duration);
            tween.From = Util.MakePair(from_volume, from_pitch);
            tween.To = Util.MakePair(to_volumne, to_pitch);
            return tween;
        }
        public static TweenAudio AudioFrom(GameObject owner, float volume, float pitch, float duration) {
            var tween = new TweenAudio(owner, duration);
            tween.From = Util.MakePair(volume, pitch);
            return tween;
        }
        public static TweenAudio AudioTo(GameObject owner, float volume, float pitch, float duration) {
            var tween = new TweenAudio(owner, duration);
            tween.To = Util.MakePair(volume, pitch);
            return tween;
        }

        public static TweenWrapper Wrap(params TweenBase[] tweens) {
            Assert.IsTrue(tweens.Length > 0);

            TweenWrapper wrapper = new TweenWrapper(null);
            foreach (var tween in tweens) {
                wrapper.Add(tween);
            }
            return wrapper;
        }
        public static TweenWrapper Sequence(params TweenBase[] tweens) {
            Assert.IsTrue(tweens.Length > 0);

            TweenWrapper wrapper = new TweenWrapper(null);
            TweenBase last_tween = tweens[0];
            wrapper.Add(last_tween);
            for (var i = 1; i < tweens.Length; i++) {
                var tween = tweens[i];
                if (last_tween != null) {
                    last_tween.AddNext(tween);
                }
                last_tween = tween;
            }
            return wrapper;
        }
        public static TweenWrapper Parallel(params TweenBase[] tweens) {
            return Wrap(tweens);
        }
        #endregion

        #region Chaining set functions
        public static TTween SetEaseType<TTween>(this TTween tween, EaseFuncs.EaseType ease_type) where TTween : TweenDuration {
            tween.easeType = ease_type;
            return tween;
        }
        public static TTween SetLoopType<TTween>(this TTween tween, TweenDuration.LoopType loop_type) where TTween : TweenDuration {
            tween.loopType = loop_type;
            return tween;
        }
        public static TTween SetRepeat<TTween>(this TTween tween, int repeat_cnt) where TTween : TweenDuration {
            tween.repeatCnt = repeat_cnt;
            return tween;
        }
        public static TTween SetFrom<TTween, TValue>(this TTween tween, TValue value) where TTween : TweenFromTo<TValue> {
            tween.From = value;
            return tween;
        }
        public static TTween SetTo<TTween, TValue>(this TTween tween, TValue value) where TTween : TweenFromTo<TValue> {
            tween.To = value;
            return tween;
        }

        public static TTween WhenComplete<TTween>(this TTween tween, Action onComplete) where TTween : TweenBase {
            tween.onComplete += onComplete;
            return tween;
        }
        public static TTween WhenStart<TTween>(this TTween tween, Action onStart) where TTween : TweenBase {
            tween.onStart += onStart;
            return tween;
        }
        public static TTween WhenUpdate<TTween>(this TTween tween, Action<float> on_update) where TTween : TweenDuration {
            tween.onUpdate += on_update;
            return tween;
        }
        public static TTween WhenUpdateValue<TTween>(this TTween tween, Action<float> on_update_value) where TTween : TweenDuration {
            tween.onUpdateValue += on_update_value;
            return tween;
        }
        public static TTween WhenLoop<TTween>(this TTween tween, Action<int> on_loop) where TTween : TweenDuration {
            tween.onLoop += on_loop;
            return tween;
        }

        public static TTween Then<TTween>(this TTween tween, params TweenBase[] tweens) where TTween : TweenBase {
            foreach (var next_tween in tweens) {
                tween.AddNext(next_tween);
            }
            return tween;
        }
        public static TTween IgnoreTimeScale<TTween>(this TTween tween, bool ignore_time_scale) where TTween : TweenBase {
            tween.ignoreTimeScale = ignore_time_scale;
            return tween;
        }
        public static TTween IncludeChildren<TTween>(this TTween tween, bool ignore_time_scale) where TTween : TweenBase {
            tween.ignoreTimeScale = ignore_time_scale;
            return tween;
        }

        public static TweenMove IsLocal(this TweenMove tween, bool is_local) {
            tween.isLocal = is_local;
            return tween;
        }
        public static TweenRotate IsLocal(this TweenRotate tween, bool is_local) {
            tween.isLocal = is_local;
            return tween;
        }
        public static TweenColor SetNamedColorValue(this TweenColor tween, string named_color_value) {
            tween.namedColorValue = named_color_value;
            return tween;
        }
        public static TweenAlpha SetNamedColorValue(this TweenAlpha tween, string named_color_value) {
            tween.namedColorValue = named_color_value;
            return tween;
        }
        public static TweenWrapper SetRepeat(this TweenWrapper tween, int repeat_cnt) {
            tween.repeatCnt = repeat_cnt;
            return tween;
        }
        public static TweenWrapper SetTimeLimit(this TweenWrapper tween, float time_limit) {
            tween.timeLimit = time_limit;
            return tween;
        }
        public static TweenWrapper SetFinishAllWhenTimeout(this TweenWrapper tween, bool value) {
            tween.finishAllWhenTimeout = value;
            return tween;
        }
        public static TweenWrapper SetCancelWhenOneCanceled(this TweenWrapper tween, bool value) {
            tween.cancelAllWhenOneCanceled = value;
            return tween;
        }
        #endregion
    }
}
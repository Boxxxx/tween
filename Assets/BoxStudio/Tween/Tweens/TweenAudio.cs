using UnityEngine;
using System.Collections.Generic;

namespace Box.Tween {
    using AudioData = KeyValuePair<float, float>;
    public class TweenAudio : TweenFromTo<AudioData> {
        public TweenAudio(GameObject owner, float duration)
                : base(owner, duration) {
            from_ = to_ = GetValue();
        }
        public TweenAudio(GameObject owner, float duration, float volume, float pitch)
                : base(owner, duration) {
            To = Util.MakePair(volume, pitch);
        }

        internal override AudioData GetValue() {
            var audioSource = owner_.GetComponent<AudioSource>();
            return Util.MakePair(audioSource.volume, audioSource.pitch);
        }
        internal override void SetValue(AudioData value) {
            var audioSource = owner_.GetComponent<AudioSource>();
            audioSource.volume = value.Key;
            audioSource.pitch = value.Value;
        }
        internal override AudioData LerpValue(AudioData from, AudioData to, float value) {
            return Util.MakePair(Mathf.Lerp(from.Key, to.Key, value),
                                 Mathf.Lerp(from.Value, to.Value, value));
        }

        public TweenAudio SetFromVolume(float volume) {
            From = Util.MakePair(volume, From.Value);
            return this;
        }
        public TweenAudio SetToVolume(float volume) {
            To = Util.MakePair(volume, To.Value);
            return this;
        }
        public TweenAudio SetFromPitch(float pitch) {
            From = Util.MakePair(From.Key, pitch);
            return this;
        }
        public TweenAudio SetToPitch(float pitch) {
            To = Util.MakePair(To.Value, pitch);
            return this;
        }
    }
}
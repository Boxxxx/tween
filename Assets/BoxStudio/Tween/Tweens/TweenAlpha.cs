using UnityEngine;
using UnityEngine.UI;
using System;

namespace BoxStudio {
    public class TweenAlpha : TweenFromTo<float> {
        private TweenColor.SourceType source_type_ = TweenColor.SourceType.None;

        // This value specifics the name of color value used in shader (if no Color_ found).
        // In default, it's '_TintColor'.
        public string named_color_value_ = "_TintColor";
        public string namedColorValue {
            get { return named_color_value_; }
            set {
                named_color_value_ = value;
            }
        }

        public TweenAlpha(GameObject owner, float duration)
                : base(owner, duration) { }
        public TweenAlpha(GameObject owner, float duration, float from, float to)
                : base(owner, duration, from, to) { }

        internal override void Reset() {
            source_type_ = TweenColor.GetSourceType(owner_);

            base.Reset();
        }
        internal override float GetValue() {
            return TweenColor.GetSourceColor(owner_, source_type_, named_color_value_).a;
        }
        internal override void SetValue(float value) {
            var color = TweenColor.GetSourceColor(owner_, source_type_, named_color_value_);
            color.a = value;
            TweenColor.SetSourceColor(owner_, color, source_type_, named_color_value_);
        }
        internal override float LerpValue(float from, float to, float value) {
            return Mathf.Lerp(from, to, value);
        }
    }
}

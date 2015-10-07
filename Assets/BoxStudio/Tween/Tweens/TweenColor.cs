using UnityEngine;
using UnityEngine.UI;
using System;

namespace BoxStudio {
    public class TweenColor : TweenFromTo<Color> {
        internal enum SourceType {
            None,
            SpriteRenderer,
            Image,
            GUITexture,
            GUIText,
            Renderer,
            NGUI_UILabel,
            NGUI_UISprite,
            NGUI_UIPanel
        }
        private SourceType source_type_ = SourceType.None;

        // This value specifics the name of color value used in shader (if no Color_ found).
        // In default, it's '_TintColor'.
        public string named_color_value_ = "_TintColor";
        public string namedColorValue {
            get { return named_color_value_; }
            set {
                named_color_value_ = value;
            }
        }

        public TweenColor(GameObject owner, float duration)
                : base(owner, duration) { }
        public TweenColor(GameObject owner, float duration, Color from, Color to)
                : base(owner, duration, from, to) { }

        internal override void Reset() {
            source_type_ = GetSourceType(owner_);

            base.Reset();
        }
        internal override Color GetValue() {
            return GetSourceColor(owner_, source_type_, named_color_value_);
        }
        internal override void SetValue(Color value) {
            SetSourceColor(owner_, value, source_type_, named_color_value_);
        }
        internal override Color LerpValue(Color from, Color to, float value) {
            return Color.Lerp(from, to, value);
        }

        internal static SourceType GetSourceType(GameObject owner) {
            if (owner.GetComponent<SpriteRenderer>()) {
                return SourceType.SpriteRenderer;
            }
            else if (owner.GetComponent<Image>()) {
                return SourceType.Image;
            }
            else if (owner.GetComponent<GUITexture>()) {
                return SourceType.GUITexture;
            }
            else if (owner.GetComponent<GUIText>()) {
                return SourceType.GUIText;
            }
            /* NGUI
            else if (owner_.GetComponent<UILabel>()) {
                return SourceType.UILabel;
            }
            else if (owner_.GetComponent<UISprite>()) {
                return SourceType.UISprite;
            }
            else if (owner_.GetComponent<UIPanel>()) {
                return SourceType.UIPanel;
            } */
            else if (owner.GetComponent<Renderer>()) {
                return SourceType.Renderer;
            }

            throw new NotImplementedException();
        }
        internal static void SetSourceColor(GameObject owner, Color value, SourceType source_type, string named_color_value) {
            switch (source_type) {
                case SourceType.SpriteRenderer:
                    owner.GetComponent<SpriteRenderer>().color = value;
                    return;
                case SourceType.Image:
                    owner.GetComponent<Image>().color = value;
                    return;
                case SourceType.GUITexture:
                    owner.GetComponent<GUITexture>().color = value;
                    return;
                case SourceType.GUIText:
                    owner.GetComponent<GUIText>().color = value;
                    return;
                /*NGUI
                case SourceType.NGUI_UILabel:
                    owner_.GetComponent<UILabel>().color = value;
                    return;
                case SourceType.NGUI_UIPanel:
                    owner_.GetComponent<UIPanel>().color = value;
                    return;
                case SourceType.NGUI_UISprite:
                    owner_.GetComponent<UISprite>().color = value;
                    return;
                */
                case SourceType.Renderer:
                    var renderer = owner.GetComponent<Renderer>();
                    if (renderer.material.HasProperty("_Color")) {
                        renderer.material.color = value;
                        return;
                    }
                    else if (renderer.material.HasProperty(named_color_value)) {
                        renderer.material.SetColor(named_color_value, value);
                        return;
                    }
                    break;
            }
            throw new NotImplementedException();
        }
        internal static Color GetSourceColor(GameObject owner, SourceType source_type, string named_color_value) {
            switch (source_type) {
                case SourceType.SpriteRenderer:
                    return owner.GetComponent<SpriteRenderer>().color;
                case SourceType.Image:
                    return owner.GetComponent<Image>().color;
                case SourceType.GUITexture:
                    return owner.GetComponent<GUITexture>().color;
                case SourceType.GUIText:
                    return owner.GetComponent<GUIText>().color;
                /*NGUI
                case SourceType.NGUI_UILabel:
                    return owner_.GetComponent<UILabel>().color;
                case SourceType.NGUI_UIPanel:
                    return owner_.GetComponent<UIPanel>().color;
                case SourceType.NGUI_UISprite:
                    return owner_.GetComponent<UISprite>().color;
                */
                case SourceType.Renderer:
                    var renderer = owner.GetComponent<Renderer>();
                    if (renderer.material.HasProperty("_Color")) {
                        return renderer.material.color;
                    }
                    else if (renderer.material.HasProperty(named_color_value)) {
                        return renderer.material.GetColor(named_color_value);
                    }
                    break;
            }
            throw new NotImplementedException();
        }
    }
}

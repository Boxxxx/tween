using UnityEngine;
using System.Collections;

namespace BoxStudio {
    public static class EaseFuncs {
        public enum EaseType {
            EaseInQuad,
            EaseOutQuad,
            EaseInOutQuad,
            EaseInCubic,
            EaseOutCubic,
            EaseInOutCubic,
            EaseInQuart,
            EaseOutQuart,
            EaseInOutQuart,
            EaseInQuint,
            EaseOutQuint,
            EaseInOutQuint,
            EaseOutInQuint,
            EaseInSine,
            EaseOutSine,
            EaseInOutSine,
            EaseInExpo,
            EaseOutExpo,
            EaseInOutExpo,
            EaseInCirc,
            EaseOutCirc,
            EaseInOutCirc,
            Linear,
            Spring,
            EaseInBounce,
            EaseOutBounce,
            EaseInOutBounce,
            EaseInBack,
            EaseOutBack,
            EaseInOutBack,
            EaseInElastic,
            EaseOutElastic,
            EaseInOutElastic,
            Punch
        };

        public delegate float EaseFuncDelegate(float start, float end, float value);

        public static float Linear(float start, float end, float value) {
            return Mathf.Lerp(start, end, value);
        }

        public static float Clerp(float start, float end, float value) {
            float min = 0.0f;
            float max = 360.0f;
            float half = Mathf.Abs((max - min) / 2.0f);
            float retval = 0.0f;
            float diff = 0.0f;
            if ((end - start) < -half) {
                diff = ((max - start) + end) * value;
                retval = start + diff;
            }
            else if ((end - start) > half) {
                diff = -((max - end) + start) * value;
                retval = start + diff;
            }
            else retval = start + (end - start) * value;
            return retval;
        }

        public static float Spring(float start, float end, float value) {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        public static float EaseInQuad(float start, float end, float value) {
            end -= start;
            return end * value * value + start;
        }

        public static float EaseOutQuad(float start, float end, float value) {
            end -= start;
            return -end * value * (value - 2) + start;
        }

        public static float EaseInOutQuad(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * value * value + start;
            value--;
            return -end / 2 * (value * (value - 2) - 1) + start;
        }

        public static float EaseInCubic(float start, float end, float value) {
            end -= start;
            return end * value * value * value + start;
        }

        public static float EaseOutCubic(float start, float end, float value) {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
        }

        public static float EaseInOutCubic(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * value * value * value + start;
            value -= 2;
            return end / 2 * (value * value * value + 2) + start;
        }

        public static float EaseInQuart(float start, float end, float value) {
            end -= start;
            return end * value * value * value * value + start;
        }

        public static float EaseOutQuart(float start, float end, float value) {
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }

        public static float EaseInOutQuart(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * value * value * value * value + start;
            value -= 2;
            return -end / 2 * (value * value * value * value - 2) + start;
        }

        public static float EaseInQuint(float start, float end, float value) {
            end -= start;
            return end * value * value * value * value * value + start;
        }

        public static float EaseOutInQuint(float start, float end, float value) {
            value /= .5f;
            if (value <= 1)
                return EaseOutQuint(start, (start + end) * .5f, value);
            else
                return EaseInQuint((start + end) * .5f, end, value - 1);
        }

        public static float EaseOutQuint(float start, float end, float value) {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;
        }

        public static float EaseInOutQuint(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * value * value * value * value * value + start;
            value -= 2;
            return end / 2 * (value * value * value * value * value + 2) + start;
        }

        public static float EaseInSine(float start, float end, float value) {
            end -= start;
            return -end * Mathf.Cos(value / 1 * (Mathf.PI / 2)) + end + start;
        }

        public static float EaseOutSine(float start, float end, float value) {
            end -= start;
            return end * Mathf.Sin(value / 1 * (Mathf.PI / 2)) + start;
        }

        public static float EaseInOutSine(float start, float end, float value) {
            end -= start;
            return -end / 2 * (Mathf.Cos(Mathf.PI * value / 1) - 1) + start;
        }

        public static float EaseInExpo(float start, float end, float value) {
            end -= start;
            return end * Mathf.Pow(2, 10 * (value / 1 - 1)) + start;
        }

        public static float EaseOutExpo(float start, float end, float value) {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * value / 1) + 1) + start;
        }

        public static float EaseInOutExpo(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * Mathf.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end / 2 * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }

        public static float EaseInCirc(float start, float end, float value) {
            end -= start;
            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }

        public static float EaseOutCirc(float start, float end, float value) {
            value--;
            end -= start;
            return end * Mathf.Sqrt(1 - value * value) + start;
        }

        public static float EaseInOutCirc(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return -end / 2 * (Mathf.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end / 2 * (Mathf.Sqrt(1 - value * value) + 1) + start;
        }

        public static float EaseInBounce(float start, float end, float value) {
            end -= start;
            float d = 1f;
            return end - EaseOutBounce(0, end, d - value) + start;
        }

        public static float EaseOutBounce(float start, float end, float value) {
            value /= 1f;
            end -= start;
            if (value < (1 / 2.75f)) {
                return end * (7.5625f * value * value) + start;
            }
            else if (value < (2 / 2.75f)) {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;
            }
            else if (value < (2.5 / 2.75)) {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;
            }
            else {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;
            }
        }

        public static float EaseInOutBounce(float start, float end, float value) {
            end -= start;
            float d = 1f;
            if (value < d / 2) return EaseInBounce(0, end, value * 2) * 0.5f + start;
            else return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }

        public static float EaseInBack(float start, float end, float value) {
            end -= start;
            value /= 1;
            float s = 1.70158f;
            return end * (value) * value * ((s + 1) * value - s) + start;
        }

        public static float EaseOutBack(float start, float end, float value) {
            float s = 1.70158f;
            end -= start;
            value = (value / 1) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }

        public static float EaseInOutBack(float start, float end, float value) {
            float s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1) {
                s *= (1.525f);
                return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }

        public static float EaseInElastic(float start, float end, float value) {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p / 4;
            }
            else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        }

        public static float EaseOutElastic(float start, float end, float value) {
            /* GFX47 MOD END */
            //Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p / 4;
            }
            else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
        }

        public static float EaseInOutElastic(float start, float end, float value) {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d / 2) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p / 4;
            }
            else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
            return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }

        public static float Punch(float start, float end, float value) {
            float amplitude = (value - start) / (end - start);
            float s = 9;
            if (value == 0) {
                return 0;
            }
            if (value == 1) {
                return 0;
            }
            float period = 1 * 0.3f;
            s = period / (2 * Mathf.PI) * Mathf.Asin(0);
            return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
        }

        public static EaseFuncDelegate GetEaseFunc(EaseType easeType) {
            switch (easeType) {
                case EaseType.EaseInQuart:
                    return EaseInQuart;
                case EaseType.EaseOutQuart:
                    return EaseOutQuart;
                case EaseType.EaseInOutQuart:
                    return EaseInOutQuart;
                case EaseType.EaseInBack:
                    return EaseInBack;
                case EaseType.EaseOutBack:
                    return EaseOutBack;
                case EaseType.EaseInOutBack:
                    return EaseInOutBack;
                case EaseType.EaseOutElastic:
                    return EaseOutElastic;
                case EaseType.EaseInCubic:
                    return EaseInCubic;
                case EaseType.EaseOutCubic:
                    return EaseOutCubic;
                case EaseType.EaseInOutCubic:
                    return EaseInOutCubic;
                case EaseType.Punch:
                    return Punch;
                case EaseType.EaseInQuad:
                    return EaseInQuad;
                case EaseType.EaseOutQuad:
                    return EaseOutQuad;
                case EaseType.EaseInOutQuad:
                    return EaseInOutQuad;
                case EaseType.EaseInQuint:
                    return EaseInQuint;
                case EaseType.EaseOutQuint:
                    return EaseOutQuint;
                case EaseType.EaseInOutQuint:
                    return EaseInOutQuint;
                case EaseType.EaseOutInQuint:
                    return EaseOutInQuint;
                case EaseType.EaseInSine:
                    return EaseInSine;
                case EaseType.EaseOutSine:
                    return EaseOutSine;
                case EaseType.EaseInOutSine:
                    return EaseInOutSine;
                case EaseType.EaseInExpo:
                    return EaseInExpo;
                case EaseType.EaseOutExpo:
                    return EaseOutExpo;
                case EaseType.EaseInOutExpo:
                    return EaseInOutExpo;
                case EaseType.EaseInCirc:
                    return EaseInCirc;
                case EaseType.EaseOutCirc:
                    return EaseOutCirc;
                case EaseType.EaseInOutCirc:
                    return EaseInOutCirc;
                case EaseType.Spring:
                    return Spring;
                case EaseType.EaseInBounce:
                    return EaseInBounce;
                case EaseType.EaseOutBounce:
                    return EaseOutBounce;
                case EaseType.EaseInOutBounce:
                    return EaseInOutBounce;
                case EaseType.EaseInElastic:
                    return EaseInElastic;
                case EaseType.EaseInOutElastic:
                    return EaseInOutElastic;
                case EaseType.Linear:
                    return Linear;
                default:
                    Debug.LogWarningFormat("Easeype {0} not support.", easeType);
                    return Linear;
            }
        }
    }
}
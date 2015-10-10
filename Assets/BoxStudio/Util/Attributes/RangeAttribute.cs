using UnityEngine;

namespace Box {
    public class RangeAttribute : PropertyAttribute {
        public float min;
        public float max;

        public RangeAttribute(float min, float max) {
            this.min = min;
            this.max = max;
        }
    }
}
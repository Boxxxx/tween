using UnityEngine;
using System;
using System.Collections.Generic;

namespace Box {
    public static class Util {
        public static KeyValuePair<K, V> MakePair<K, V>(K key, V value) {
            return new KeyValuePair<K, V>(key, value);
        }
    }
}

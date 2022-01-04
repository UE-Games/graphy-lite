using System;
using UnityEngine;


namespace UniEnt.Graphy_Lite.Runtime.Util {


    static class IntString {


        /// <summary>
        ///     List of negative ints casted to strings.
        /// </summary>
        static string[] _negativeBuffer = Array.Empty<string>();

        /// <summary>
        ///     List of positive ints casted to strings.
        /// </summary>
        static string[] _positiveBuffer = Array.Empty<string>();

        /// <summary>
        ///     The lowest int value of the existing number buffer.
        /// </summary>
        static int MinValue => -(_negativeBuffer.Length - 1);

        /// <summary>
        ///     The highest int value of the existing number buffer.
        /// </summary>
        static int MaxValue => _positiveBuffer.Length;


        /// <summary>
        ///     Initialize the buffers.
        /// </summary>
        /// <param name="minNegativeValue">
        ///     Lowest negative value allowed.
        /// </param>
        /// <param name="maxPositiveValue">
        ///     Highest positive value allowed.
        /// </param>
        public static void Init(int minNegativeValue, int maxPositiveValue) {
            if (MinValue > minNegativeValue && minNegativeValue <= 0) {
                int length = Mathf.Abs(minNegativeValue);

                _negativeBuffer = new string[length];

                for (var i = 0; i < length; i++)
                    _negativeBuffer[i] = (-i - 1).ToString();
            }

            if (MaxValue >= maxPositiveValue || maxPositiveValue < 0)
                return;

            _positiveBuffer = new string[maxPositiveValue + 1];

            for (var i = 0; i < maxPositiveValue + 1; i++)
                _positiveBuffer[i] = i.ToString();
        }


        public static void Dispose() {
            _negativeBuffer = Array.Empty<string>();
            _positiveBuffer = Array.Empty<string>();
        }


        /// <summary>
        ///     Returns this int as a cached string.
        /// </summary>
        /// <param name="value">
        ///     The required int.
        /// </param>
        /// <returns>
        ///     A cached number string if within the buffer ranges.
        /// </returns>
        public static string ToStringNonAlloc(this int value) {
            return value switch {
                < 0 when -value <= _negativeBuffer.Length => _negativeBuffer[-value - 1],
                >= 0 when value < _positiveBuffer.Length => _positiveBuffer[value],
                _ => value.ToString()
            };

            // If the value is not within the buffer ranges, just do a normal .ToString()
        }


    }


}

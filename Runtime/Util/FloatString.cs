using System;
using System.Globalization;
using UnityEngine;


namespace UniEnt.GraphyLite.Runtime.Util {


    static class FloatString {


        /// <summary>
        ///     Float represented as a string, formatted.
        /// </summary>
        const string FloatFormat = "0.0";

        /// <summary>
        ///     The currently defined, globally used decimal multiplier.
        /// </summary>
        const float DecimalMultiplier = 10f;

        /// <summary>
        ///     List of negative floats casted to strings.
        /// </summary>
        static string[] _negativeBuffer = Array.Empty<string>();

        /// <summary>
        ///     List of positive floats casted to strings.
        /// </summary>
        static string[] _positiveBuffer = Array.Empty<string>();

        /// <summary>
        ///     The lowest float value of the existing number buffer.
        /// </summary>
        static float MinValue => -(_negativeBuffer.Length - 1).FromIndex();

        /// <summary>
        ///     The highest float value of the existing number buffer.
        /// </summary>
        static float MaxValue => (_positiveBuffer.Length - 1).FromIndex();


        /// <summary>
        ///     Initialize the buffers.
        /// </summary>
        /// <param name="minNegativeValue">
        ///     Lowest negative value allowed.
        /// </param>
        /// <param name="maxPositiveValue">
        ///     Highest positive value allowed.
        /// </param>
        public static void Init(float minNegativeValue, float maxPositiveValue) {
            int negativeLength = minNegativeValue.ToIndex();
            int positiveLength = maxPositiveValue.ToIndex();

            if (MinValue > minNegativeValue && negativeLength >= 0) {
                _negativeBuffer = new string[negativeLength];

                for (var i = 0; i < negativeLength; i++)
                    _negativeBuffer[i] = (-i - 1).FromIndex().ToString(FloatFormat);
            }

            if (!(MaxValue < maxPositiveValue) || positiveLength < 0)
                return;

            _positiveBuffer = new string[positiveLength + 1];

            for (var i = 0; i < positiveLength + 1; i++)
                _positiveBuffer[i] = i.FromIndex().ToString(FloatFormat);
        }


        public static void Dispose() {
            _negativeBuffer = Array.Empty<string>();
            _positiveBuffer = Array.Empty<string>();
        }


        /// <summary>
        ///     Returns this float as a cached string.
        /// </summary>
        /// <param name="value">
        ///     The required float.
        /// </param>
        /// <returns>
        ///     A cached number string.
        /// </returns>
        public static string ToStringNonAlloc(this float value) {
            int valIndex = value.ToIndex();

            return value switch {
                < 0 when valIndex < _negativeBuffer.Length => _negativeBuffer[valIndex],
                >= 0 when valIndex < _positiveBuffer.Length => _positiveBuffer[valIndex],
                _ => value.ToString(CultureInfo.InvariantCulture)
            };
        }


        /// <summary>
        ///     Returns this float as a cached string.
        /// </summary>
        /// <param name="value">
        ///     The required float.
        /// </param>
        /// <param name="format">
        ///     Format.
        /// </param>
        /// <returns>
        ///     A cached number string.
        /// </returns>
        public static string ToStringNonAlloc(this float value, string format) {
            int valIndex = value.ToIndex();

            return value switch {
                < 0 when valIndex < _negativeBuffer.Length => _negativeBuffer[valIndex],
                >= 0 when valIndex < _positiveBuffer.Length => _positiveBuffer[valIndex],
                _ => value.ToString(format)
            };
        }


        /// <summary>
        ///     Returns a float as a casted int.
        /// </summary>
        /// <param name="f">
        ///     The given float.
        /// </param>
        /// <returns>
        ///     The given float as an int.
        /// </returns>
        static int ToInt(this float f) => (int)f;


        /// <summary>
        ///     Returns an int as a casted float.
        /// </summary>
        /// <returns>
        ///     The given int as a float.
        /// </returns>
        static float ToFloat(this int i) => i;


        static int ToIndex(this float f) => Mathf.Abs((f * DecimalMultiplier).ToInt());


        static float FromIndex(this int i) => i.ToFloat() / DecimalMultiplier;


    }


}

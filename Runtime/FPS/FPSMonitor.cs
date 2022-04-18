using System;
using UnityEngine;


namespace UniEnt.Graphy_Lite.Runtime.FPS {


    public sealed class FPSMonitor : MonoBehaviour {


        const short FPSSamplesCapacity = 1024;


        short[] _fpsSamples;
        short _fpsSamplesCount;
        short[] _fpsSamplesSorted;
        short _indexSample;
        short _onePercentSamples = 10;
        float _unscaledDeltaTime;
        short _zero1PercentSamples = 1;


        short CurrentFPS { get; set; }

        internal short AverageFPS { get; private set; }

        internal short OnePercentFPS { get; private set; }

        internal short Zero1PercentFps { get; private set; }


        void Awake() {
            Init();
        }


        void Update() {
            _unscaledDeltaTime = Time.unscaledDeltaTime;

            CurrentFPS = (short)Mathf.RoundToInt(1f / _unscaledDeltaTime);

            uint averageAddedFps = 0;

            _indexSample++;

            if (_indexSample >= FPSSamplesCapacity)
                _indexSample = 0;

            _fpsSamples[_indexSample] = CurrentFPS;

            if (_fpsSamplesCount < FPSSamplesCapacity)
                _fpsSamplesCount++;

            for (var i = 0; i < _fpsSamplesCount; i++)
                averageAddedFps += (uint)_fpsSamples[i];

            AverageFPS = (short)(averageAddedFps / (float)_fpsSamplesCount);

            _fpsSamples.CopyTo(_fpsSamplesSorted, 0);

            /*
             * TODO: Find a faster way to do this.
             * We can probably avoid copying the full array every time and insert the new item already sorted
             * in the list.
             */

            // The lambda expression avoids garbage generation
            Array.Sort(_fpsSamplesSorted, static (x, y) => x.CompareTo(y));

            var zero1PercentCalculated = false;

            uint totalAddedFps = 0;

            short samplesToIterateThroughForOnePercent = _fpsSamplesCount < _onePercentSamples ? _fpsSamplesCount : _onePercentSamples;

            short samplesToIterateThroughForZero1Percent = _fpsSamplesCount < _zero1PercentSamples ? _fpsSamplesCount : _zero1PercentSamples;

            var sampleToStartIn = (short)(FPSSamplesCapacity - _fpsSamplesCount);

            for (short i = sampleToStartIn; i < sampleToStartIn + samplesToIterateThroughForOnePercent; i++) {
                totalAddedFps += (ushort)_fpsSamplesSorted[i];

                if (zero1PercentCalculated || i < samplesToIterateThroughForZero1Percent - 1)
                    continue;

                zero1PercentCalculated = true;

                Zero1PercentFps = (short)(totalAddedFps / (float)_zero1PercentSamples);
            }

            OnePercentFPS = (short)(totalAddedFps / (float)_onePercentSamples);
        }


        internal void UpdateParameters() {
            _onePercentSamples = FPSSamplesCapacity / 100;
            _zero1PercentSamples = FPSSamplesCapacity / 1000;
        }


        void Init() {
            _fpsSamples = new short[FPSSamplesCapacity];
            _fpsSamplesSorted = new short[FPSSamplesCapacity];

            UpdateParameters();
        }


    }


}

using JetBrains.Annotations;
using UniEnt.GraphyLite.Runtime.Util;
using UnityEngine;
using UnityEngine.UI;


namespace UniEnt.GraphyLite.Runtime.FPS {


    public sealed class FPSText : MonoBehaviour {


        const string MSStringFormat = "0.0";


        [SerializeField]
        Text fps;

        [SerializeField]
        Text ms;

        [SerializeField]
        Text avg;

        [SerializeField]
        Text onePercent;

        [SerializeField]
        Text zeroOnePercent;

        float _deltaTime;
        float _fps;
        FPSMonitor _fpsMonitor;
        int _frameCount;
        GraphyLite _graphyLite;
        float _ms;
        int _updateRate = 4; // 4 updates per sec.


        void Awake() {
            Init();
        }


        void Update() {
            _deltaTime += Time.unscaledDeltaTime;

            _frameCount++;

            // Only update texts 'm_updateRate' times per second

            if (!(_deltaTime > 1f / _updateRate))
                return;

            _fps = _frameCount / _deltaTime;
            _ms = _deltaTime / _frameCount * 1000f;

            // Update fps
            fps.text = Mathf.RoundToInt(_fps).ToStringNonAlloc();
            SetFpsRelatedTextColor(fps, _fps);

            // Update ms
            ms.text = _ms.ToStringNonAlloc(MSStringFormat);
            SetFpsRelatedTextColor(ms, _fps);

            // Update 1% fps
            onePercent.text = ((int)_fpsMonitor.OnePercentFPS).ToStringNonAlloc();
            SetFpsRelatedTextColor(onePercent, _fpsMonitor.OnePercentFPS);

            // Update 0.1% fps
            zeroOnePercent.text = ((int)_fpsMonitor.Zero1PercentFps).ToStringNonAlloc();
            SetFpsRelatedTextColor(zeroOnePercent, _fpsMonitor.Zero1PercentFps);

            // Update avg fps
            avg.text = ((int)_fpsMonitor.AverageFPS).ToStringNonAlloc();
            SetFpsRelatedTextColor(avg, _fpsMonitor.AverageFPS);

            // Reset variables
            _deltaTime = 0f;
            _frameCount = 0;
        }


        internal void UpdateParameters() {
            _updateRate = GraphyLite.FPSTextUpdateRate;
        }


        /// <summary>
        ///     Assigns color to a text according to their fps numeric value and
        ///     the colors specified in the 3 categories (Good, Caution, Critical).
        /// </summary>
        /// <param name="text">
        ///     UI Text component to change its color
        /// </param>
        /// <param name="f">
        ///     Numeric fps value
        /// </param>
        void SetFpsRelatedTextColor([NotNull] Graphic text, float f) {
            text.color = f switch {
                > GraphyLite.GoodFPSThreshold => _graphyLite._goodFPSColor,
                > GraphyLite.CautionFPSThreshold => _graphyLite._cautionFPSColor,
                _ => _graphyLite._criticalFPSColor
            };
        }


        void Init() {
            IntString.Init(0, 2000); // Max fps expected
            FloatString.Init(0, 100); // Max ms expected per frame

            _graphyLite = transform.root.GetComponentInChildren<GraphyLite>();
            _fpsMonitor = GetComponent<FPSMonitor>();

            UpdateParameters();
        }


    }


}

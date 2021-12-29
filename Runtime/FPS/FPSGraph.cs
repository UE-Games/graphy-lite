using UniEnt.GraphyLite.Runtime.Shader;
using UnityEngine;
using UnityEngine.UI;


namespace UniEnt.GraphyLite.Runtime.FPS {


    public sealed class FPSGraph : MonoBehaviour {


        [SerializeField]
        Image graph;

        [SerializeField]
        UnityEngine.Shader shader;

        // This keeps track of whether Init() has run or not
        [SerializeField]
        bool isInitialized;

        int[] _fpsArray;
        FPSMonitor _fpsMonitor;
        GraphyLite _graphyLite;
        int _highestFPS;
        int _resolution = 150;
        GraphShader _shaderGraph;


        void Update() {
            UpdateGraph();
        }


        internal void UpdateParameters() {
            if (_shaderGraph == null) {
                // TODO: While Graphy is disabled (e.g. by default via Ctrl+H) and while in Editor after a Hot-Swap,
                // the OnApplicationFocus calls this while m_shaderGraph == null, throwing a NullReferenceException
                return;
            }

            _shaderGraph.arrayMaxSize = GraphShader.ArrayMaxSize;
            _shaderGraph.image.material = new Material(shader);

            _shaderGraph.InitializeShader();

            _resolution = GraphyLite.FPSGraphResolution;

            CreatePoints();
        }


        void Init() {
            _graphyLite = transform.root.GetComponentInChildren<GraphyLite>();

            _fpsMonitor = GetComponent<FPSMonitor>();

            _shaderGraph = new GraphShader {
                image = graph
            };

            UpdateParameters();

            isInitialized = true;
        }


        void UpdateGraph() {
            // Since we no longer initialize by default OnEnable(),
            // we need to check here, and Init() if needed
            if (!isInitialized)
                Init();

            var fps = (short)(1 / Time.unscaledDeltaTime);

            var currentMaxFps = 0;

            for (var i = 0; i <= _resolution - 1; i++) {
                if (i >= _resolution - 1)
                    _fpsArray[i] = fps;
                else
                    _fpsArray[i] = _fpsArray[i + 1];

                // Store the highest fps to use as the highest point in the graph

                if (currentMaxFps < _fpsArray[i])
                    currentMaxFps = _fpsArray[i];
            }

            _highestFPS = _highestFPS < 1 || _highestFPS <= currentMaxFps ? currentMaxFps : _highestFPS - 1;

            _highestFPS = _highestFPS > 0 ? _highestFPS : 1;

            if (_shaderGraph.shaderArrayValues == null) {
                _fpsArray = new int[_resolution];
                _shaderGraph.shaderArrayValues = new float[_resolution];
            }

            for (var i = 0; i <= _resolution - 1; i++)
                _shaderGraph.shaderArrayValues[i] = _fpsArray[i] / (float)_highestFPS;

            // Update the material values

            _shaderGraph.UpdatePoints();

            // ReSharper disable once PossibleLossOfFraction
            _shaderGraph.average = _fpsMonitor.AverageFPS / _highestFPS;
            _shaderGraph.UpdateAverage();

            _shaderGraph.goodThreshold = GraphyLite.GoodFPSThreshold / _highestFPS;
            _shaderGraph.cautionThreshold = GraphyLite.CautionFPSThreshold / _highestFPS;
            _shaderGraph.UpdateThresholds();
        }


        void CreatePoints() {
            if (_shaderGraph.shaderArrayValues == null || _fpsArray.Length != _resolution) {
                _fpsArray = new int[_resolution];
                _shaderGraph.shaderArrayValues = new float[_resolution];
            }

            for (var i = 0; i < _resolution; i++)
                _shaderGraph.shaderArrayValues[i] = 0;

            _shaderGraph.goodColor = _graphyLite._goodFPSColor;
            _shaderGraph.cautionColor = _graphyLite._cautionFPSColor;
            _shaderGraph.criticalColor = _graphyLite._criticalFPSColor;

            _shaderGraph.UpdateColors();

            _shaderGraph.UpdateArray();
        }


    }


}

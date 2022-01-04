using UniEnt.Graphy_Lite.Runtime.Shader;
using UnityEngine;
using UnityEngine.UI;


namespace UniEnt.Graphy_Lite.Runtime.RAM {


    public sealed class RAMGraph : MonoBehaviour {


        [SerializeField]
        Image allocated;

        [SerializeField]
        Image reserved;

        [SerializeField]
        Image mono;

        [SerializeField]
        UnityEngine.Shader shader;

        [SerializeField]
        bool isInitialized;

        float[] _allocatedArray;
        GraphyLite _graphyLite;
        float _highestMemory;
        float[] _monoArray;
        RAMMonitor _ramMonitor;
        float[] _reservedArray;
        int _resolution = 150;
        GraphShader _shaderGraphAllocated;
        GraphShader _shaderGraphMono;
        GraphShader _shaderGraphReserved;


        void Update() {
            UpdateGraph();
        }


        internal void UpdateParameters() {
            if (_shaderGraphAllocated == null || _shaderGraphReserved == null || _shaderGraphMono == null) {
                /*
                 * Note: this is fine, since we don't much
                 * care what granularity we use if the graph
                 * has not been initialized, i.e. it's disabled.
                 * There is no chance that for some reason
                 * parameters will not stay up to date if
                 * at some point in the future the graph is enabled:
                 * at the end of Init(), UpdateParameters() is
                 * called again.
                 */
                return;
            }

            _shaderGraphAllocated.arrayMaxSize = GraphShader.ArrayMaxSize;
            _shaderGraphReserved.arrayMaxSize = GraphShader.ArrayMaxSize;
            _shaderGraphMono.arrayMaxSize = GraphShader.ArrayMaxSize;

            _shaderGraphAllocated.image.material = new Material(shader);
            _shaderGraphReserved.image.material = new Material(shader);
            _shaderGraphMono.image.material = new Material(shader);

            _shaderGraphAllocated.InitializeShader();
            _shaderGraphReserved.InitializeShader();
            _shaderGraphMono.InitializeShader();

            _resolution = GraphyLite.RAMGraphResolution;

            CreatePoints();
        }


        void Init() {
            _graphyLite = transform.root.GetComponentInChildren<GraphyLite>();

            _ramMonitor = GetComponent<RAMMonitor>();

            _shaderGraphAllocated = new GraphShader();
            _shaderGraphReserved = new GraphShader();
            _shaderGraphMono = new GraphShader();

            _shaderGraphAllocated.image = allocated;
            _shaderGraphReserved.image = reserved;
            _shaderGraphMono.image = mono;

            UpdateParameters();

            isInitialized = true;
        }


        void UpdateGraph() {
            // Since we no longer initialize by default OnEnable(),
            // we need to check here, and Init() if needed
            if (!isInitialized)
                Init();

            float allocatedMemory = _ramMonitor.AllocatedRam;
            float reservedMemory = _ramMonitor.ReservedRam;
            float monoMemory = _ramMonitor.MonoRAM;

            _highestMemory = 0;

            for (var i = 0; i <= _resolution - 1; i++) {
                if (i >= _resolution - 1) {
                    _allocatedArray[i] = allocatedMemory;
                    _reservedArray[i] = reservedMemory;
                    _monoArray[i] = monoMemory;
                }
                else {
                    _allocatedArray[i] = _allocatedArray[i + 1];
                    _reservedArray[i] = _reservedArray[i + 1];
                    _monoArray[i] = _monoArray[i + 1];
                }

                if (_highestMemory < _reservedArray[i])
                    _highestMemory = _reservedArray[i];
            }

            for (var i = 0; i <= _resolution - 1; i++) {
                _shaderGraphAllocated.shaderArrayValues[i] = _allocatedArray[i] / _highestMemory;

                _shaderGraphReserved.shaderArrayValues[i] = _reservedArray[i] / _highestMemory;

                _shaderGraphMono.shaderArrayValues[i] = _monoArray[i] / _highestMemory;
            }

            _shaderGraphAllocated.UpdatePoints();
            _shaderGraphReserved.UpdatePoints();
            _shaderGraphMono.UpdatePoints();
        }


        void CreatePoints() {
            if (_shaderGraphAllocated.shaderArrayValues?.Length != _resolution) {
                _allocatedArray = new float[_resolution];
                _reservedArray = new float[_resolution];
                _monoArray = new float[_resolution];

                _shaderGraphAllocated.shaderArrayValues = new float[_resolution];
                _shaderGraphReserved.shaderArrayValues = new float[_resolution];
                _shaderGraphMono.shaderArrayValues = new float[_resolution];
            }

            for (var i = 0; i < _resolution; i++) {
                _shaderGraphAllocated.shaderArrayValues[i] = 0;
                _shaderGraphReserved.shaderArrayValues[i] = 0;
                _shaderGraphMono.shaderArrayValues[i] = 0;
            }

            _shaderGraphAllocated.goodColor = _graphyLite._allocatedRamColor;
            _shaderGraphAllocated.cautionColor = _graphyLite._allocatedRamColor;
            _shaderGraphAllocated.criticalColor = _graphyLite._allocatedRamColor;

            _shaderGraphAllocated.UpdateColors();

            _shaderGraphReserved.goodColor = _graphyLite._reservedRamColor;
            _shaderGraphReserved.cautionColor = _graphyLite._reservedRamColor;
            _shaderGraphReserved.criticalColor = _graphyLite._reservedRamColor;

            _shaderGraphReserved.UpdateColors();

            _shaderGraphMono.goodColor = _graphyLite._monoRamColor;
            _shaderGraphMono.cautionColor = _graphyLite._monoRamColor;
            _shaderGraphMono.criticalColor = _graphyLite._monoRamColor;

            _shaderGraphMono.UpdateColors();

            _shaderGraphAllocated.goodThreshold = 0;
            _shaderGraphAllocated.cautionThreshold = 0;
            _shaderGraphAllocated.UpdateThresholds();

            _shaderGraphReserved.goodThreshold = 0;
            _shaderGraphReserved.cautionThreshold = 0;
            _shaderGraphReserved.UpdateThresholds();

            _shaderGraphMono.goodThreshold = 0;
            _shaderGraphMono.cautionThreshold = 0;
            _shaderGraphMono.UpdateThresholds();

            _shaderGraphAllocated.UpdateArray();
            _shaderGraphReserved.UpdateArray();
            _shaderGraphMono.UpdateArray();

            _shaderGraphAllocated.average = 0;
            _shaderGraphReserved.average = 0;
            _shaderGraphMono.average = 0;

            _shaderGraphAllocated.UpdateAverage();
            _shaderGraphReserved.UpdateAverage();
            _shaderGraphMono.UpdateAverage();
        }


    }


}

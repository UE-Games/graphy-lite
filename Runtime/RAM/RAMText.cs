using UniEnt.Graphy_Lite.Runtime.Util;
using UnityEngine;
using UnityEngine.UI;


namespace UniEnt.Graphy_Lite.Runtime.RAM {


    public sealed class RAMText : MonoBehaviour {


        [SerializeField]
        Text allocated;

        [SerializeField]
        Text reserved;

        [SerializeField]
        Text mono;

        float _deltaTime;
        GraphyLite _graphyLite;
        RAMMonitor _ramMonitor;
        float _updateRate = 4f; // 4 updates per sec.


        void Awake() {
            Init();
        }


        void Update() {
            _deltaTime += Time.unscaledDeltaTime;

            if (!(_deltaTime > 1f / _updateRate))
                return;

            allocated.text = ((int)_ramMonitor.AllocatedRam).ToStringNonAlloc();
            reserved.text = ((int)_ramMonitor.ReservedRam).ToStringNonAlloc();
            mono.text = ((int)_ramMonitor.MonoRAM).ToStringNonAlloc();

            _deltaTime = 0f;
        }


        internal void UpdateParameters() {
            allocated.color = _graphyLite._allocatedRamColor;
            reserved.color = _graphyLite._reservedRamColor;
            mono.color = _graphyLite._monoRamColor;

            _updateRate = GraphyLite.RAMTextUpdateRate;
        }


        void Init() {
            // We assume no game will consume more than 16GB of RAM.
            // If it does, who cares about some minuscule garbage allocation lol.
            IntString.Init(0, 16386);

            _graphyLite = transform.root.GetComponentInChildren<GraphyLite>();

            _ramMonitor = GetComponent<RAMMonitor>();

            UpdateParameters();
        }


    }


}

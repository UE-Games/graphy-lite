using UniEnt.GraphyLite.Runtime.Util;
using UnityEngine;


namespace UniEnt.GraphyLite.Runtime {


    public sealed class GraphyLite : Singleton<GraphyLite> {


        internal const float GoodFPSThreshold = 60;
        internal const float CautionFPSThreshold = 30;
        internal const int FPSGraphResolution = 150;
        internal const int FPSTextUpdateRate = 3;
        internal const int RAMGraphResolution = 150;
        internal const int RAMTextUpdateRate = 3;

        internal readonly Color _allocatedRamColor = new Color32(255, 190, 60, 255);
        internal readonly Color _cautionFPSColor = new Color32(243, 232, 0, 255);
        internal readonly Color _criticalFPSColor = new Color32(220, 41, 30, 255);
        internal readonly Color _goodFPSColor = new Color32(118, 212, 58, 255);
        internal readonly Color _monoRamColor = new(0.3f, 0.65f, 1f, 1);
        internal readonly Color _reservedRamColor = new Color32(205, 84, 229, 255);


        void Start() {
            DontDestroyOnLoad(transform.root.gameObject);
        }


        void OnDestroy() {
            IntString.Dispose();
            FloatString.Dispose();
        }


    }


}

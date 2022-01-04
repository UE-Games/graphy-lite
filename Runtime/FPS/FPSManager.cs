using UnityEngine;


namespace UniEnt.Graphy_Lite.Runtime.FPS {


    public sealed class FPSManager : MonoBehaviour {


        FPSGraph _fpsGraph;
        FPSMonitor _fpsMonitor;
        FPSText _fpsText;
        RectTransform _rectTransform;


        void Awake() {
            Init();
        }


        void Start() {
            UpdateParameters();
        }


        internal void SetPosition() {
            float xSideOffset = Mathf.Abs(_rectTransform.anchoredPosition.x);
            float ySideOffset = Mathf.Abs(_rectTransform.anchoredPosition.y);

            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.anchorMin = Vector2.one;
            _rectTransform.anchoredPosition = new Vector2(-xSideOffset, -ySideOffset);
        }


        void UpdateParameters() {
            _fpsGraph.UpdateParameters();
            _fpsMonitor.UpdateParameters();
            _fpsText.UpdateParameters();
        }


        internal void RefreshParameters() {
            _fpsGraph.UpdateParameters();
            _fpsMonitor.UpdateParameters();
            _fpsText.UpdateParameters();
        }


        void Init() {
            _rectTransform = GetComponent<RectTransform>();

            _fpsGraph = GetComponent<FPSGraph>();
            _fpsMonitor = GetComponent<FPSMonitor>();
            _fpsText = GetComponent<FPSText>();
        }


    }


}

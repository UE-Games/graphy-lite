using UnityEngine;


namespace UniEnt.GraphyLite.Runtime.RAM {


    public sealed class RAMManager : MonoBehaviour {


        RAMGraph _ramGraph;
        RAMText _ramText;
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
            _ramGraph.UpdateParameters();
            _ramText.UpdateParameters();
        }


        internal void RefreshParameters() {
            _ramGraph.UpdateParameters();
            _ramText.UpdateParameters();
        }


        void Init() {
            transform.root.GetComponentInChildren<GraphyLite>();

            _ramGraph = GetComponent<RAMGraph>();
            _ramText = GetComponent<RAMText>();

            _rectTransform = GetComponent<RectTransform>();
        }


    }


}

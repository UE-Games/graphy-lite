using UnityEngine;
using UnityEngine.UI;


namespace UniEnt.Graphy_Lite.Runtime.Shader {


    /// <summary>
    ///     This class communicates directly with the shader to draw the graphs. Performance here is very important
    ///     to reduce as much overhead as possible, as we are updating hundreds of values every frame.
    /// </summary>
    sealed class GraphShader {


        public const int ArrayMaxSize = 512;


        const string Name = "GraphValues"; // The name of the array
        const string NameLength = "GraphValues_Length";

        static readonly int GraphValues = UnityEngine.Shader.PropertyToID(Name);
        static readonly int GraphValuesLength = UnityEngine.Shader.PropertyToID(NameLength);

        int _averagePropertyId;
        int _cautionColorPropertyId;
        int _cautionThresholdPropertyId;
        int _criticalColorPropertyId;
        int _goodColorPropertyId;
        int _goodThresholdPropertyId;

        public int arrayMaxSize = 128;
        public float average = 0;
        public Color cautionColor = Color.white;
        public float cautionThreshold = 0;
        public Color criticalColor = Color.white;
        public Color goodColor = Color.white;
        public float goodThreshold = 0;
        public Image image = null;
        public float[] shaderArrayValues;


        /// <summary>
        ///     This is done to avoid a design problem that arrays in shaders have,
        ///     and should be called before initializing any shader graph.
        ///     The first time that you use initialize an array, the size of the array in the shader is fixed.
        ///     This is why sometimes you will get a warning saying that the array size will be capped.
        ///     It shouldn't generate any issues, but in the worst case scenario just reset the Unity Editor
        ///     (if for some reason the shaders reload).
        ///     I also cache the Property IDs, that make access faster to modify shader parameters.
        /// </summary>
        public void InitializeShader() {
            image.material.SetFloatArray(GraphValues, new float[arrayMaxSize]);

            _averagePropertyId = UnityEngine.Shader.PropertyToID("Average");

            _goodThresholdPropertyId = UnityEngine.Shader.PropertyToID("_GoodThreshold");
            _cautionThresholdPropertyId = UnityEngine.Shader.PropertyToID("_CautionThreshold");

            _goodColorPropertyId = UnityEngine.Shader.PropertyToID("_GoodColor");
            _cautionColorPropertyId = UnityEngine.Shader.PropertyToID("_CautionColor");
            _criticalColorPropertyId = UnityEngine.Shader.PropertyToID("_CriticalColor");
        }


        /// <summary>
        ///     Updates the material linked with this shader graph  with the values in the float[] array.
        /// </summary>
        public void UpdateArray() {
            image.material.SetInt(GraphValuesLength, shaderArrayValues.Length);
        }


        /// <summary>
        ///     Updates the average parameter in the material.
        /// </summary>
        public void UpdateAverage() {
            image.material.SetFloat(_averagePropertyId, average);
        }


        /// <summary>
        ///     Updates the thresholds in the material.
        /// </summary>
        public void UpdateThresholds() {
            image.material.SetFloat(_goodThresholdPropertyId, goodThreshold);
            image.material.SetFloat(_cautionThresholdPropertyId, cautionThreshold);
        }


        /// <summary>
        ///     Updates the colors in the material.
        /// </summary>
        public void UpdateColors() {
            image.material.SetColor(_goodColorPropertyId, goodColor);
            image.material.SetColor(_cautionColorPropertyId, cautionColor);
            image.material.SetColor(_criticalColorPropertyId, criticalColor);
        }


        /// <summary>
        ///     Updates the points in the graph with the set array of values.
        /// </summary>
        public void UpdatePoints() {
            // Requires an array called "name"
            // and another one called "name_Length"

            image.material.SetFloatArray(GraphValues, shaderArrayValues);
        }


    }


}

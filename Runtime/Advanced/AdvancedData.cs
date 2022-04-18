using System.Text;
using UniEnt.Graphy_Lite.Runtime.Util;
using UnityEngine;
using UnityEngine.UI;


namespace UniEnt.Graphy_Lite.Runtime.Advanced {


    public sealed class AdvancedData : MonoBehaviour {


        [SerializeField]
        Text graphicsDevice;

        [SerializeField]
        Text processorType;

        [SerializeField]
        Text operatingSystem;

        [SerializeField]
        Text systemMemory;

        [SerializeField]
        Text graphicsAPI;

        [SerializeField]
        Text graphicsMemory;

        [SerializeField]
        Text screenResolution;

        [SerializeField]
        Text windowResolution;

        [Range(1, 60)]
        [SerializeField]
        float updateRate = 1f; // 1 update per sec.

        readonly string[] _windowStrings = {
            "Window: ",
            " \u00D7 ",
            " \u00B7 ",
            "Hz",
            " [",
            "DPI]"
        };
        float _deltaTime;
        StringBuilder _sb;


        void Update() {
            _deltaTime += Time.unscaledDeltaTime;

            if (!(_deltaTime > 1f / updateRate))
                return;

            // Update screen window resolution
            _sb.Length = 0;

            _sb.Append(_windowStrings[0]).Append(Screen.width.ToStringNonAlloc()).Append(_windowStrings[1]).Append(Screen.height.ToStringNonAlloc()).Append(_windowStrings[2]).Append(Screen.currentResolution.refreshRate.ToStringNonAlloc()).Append(_windowStrings[3]).Append(_windowStrings[4]).Append(((int)Screen.dpi).ToStringNonAlloc()).Append(_windowStrings[5]);

            windowResolution.text = _sb.ToString();

            // Reset variables
            _deltaTime = 0f;
        }


        void OnEnable() {
            Init();
        }


        void Init() {
            IntString.Init(0, 7680);

            _sb = new StringBuilder();

            processorType.text = $"CPU: {SystemInfo.processorType} [{SystemInfo.processorCount} cores]";
            systemMemory.text = $"RAM: {SystemInfo.systemMemorySize} MB";
            graphicsDevice.text = $"GPU: {SystemInfo.graphicsDeviceName}";
            graphicsAPI.text = $"Graphics API: {SystemInfo.graphicsDeviceVersion}";
            graphicsMemory.text = $"VRAM: {SystemInfo.graphicsMemorySize} MB \u00B7 Max Texture Size: {SystemInfo.maxTextureSize} px \u00B7 Shader Level: {SystemInfo.graphicsShaderLevel}";

            Resolution res = Screen.currentResolution;

            screenResolution.text = $"Screen: {res.width} \u00D7 {res.height} \u00B7 {res.refreshRate} Hz";
            operatingSystem.text = $"OS: {SystemInfo.operatingSystem} [{SystemInfo.deviceType}]";
        }


    }


}

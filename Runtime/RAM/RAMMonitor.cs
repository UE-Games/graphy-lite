using UnityEngine;
using UnityEngine.Profiling;


namespace UniEnt.GraphyLite.Runtime.RAM {


    public sealed class RAMMonitor : MonoBehaviour {


        internal float AllocatedRam { get; private set; }

        internal float ReservedRam { get; private set; }

        internal float MonoRAM { get; private set; }


        void Update() {
            AllocatedRam = Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
            ReservedRam = Profiler.GetTotalReservedMemoryLong() / 1048576f;
            MonoRAM = Profiler.GetMonoUsedSizeLong() / 1048576f;
        }


    }


}

using UnityEngine;


namespace UniEnt.Graphy_Lite.Runtime.Util {


    /// <inheritdoc />
    /// <summary>
    ///     Be aware this will not prevent a non singleton constructor
    ///     such as `T myT = new T();`
    ///     To prevent that, add `protected T () {}` to your singleton class.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {


        static T _instance;

        static readonly object Lock = new();


        public static T Instance {
            get {
                lock (Lock) {
                    if (_instance == null)
                        Debug.Log($"[Singleton] An instance of {typeof(T)} is trying to be accessed, but it wasn't initialized first. Make sure to add an instance of {typeof(T)} in the scene before  trying to access it.");

                    return _instance;
                }
            }
        }


        void Awake() {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = GetComponent<T>();
        }


        void OnDestroy() {
            if (_instance == this)
                _instance = null;
        }


    }


}

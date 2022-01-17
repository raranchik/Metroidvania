using UnityEngine;

namespace Control
{
    sealed class CameraControl: MonoBehaviour
    {
        public static CameraControl Instance = null;

        private void Start ()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if(Instance == this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

    }

}
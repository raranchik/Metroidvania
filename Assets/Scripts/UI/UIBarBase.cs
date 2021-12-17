using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class UIBarBase : MonoBehaviour
    {
        public static UIBarBase Instance { get; private set; }

        [SerializeField]
        protected TextMeshProUGUI _barValueText;

        protected virtual void Awake()
        {
            Instance = this;
        }

        public abstract void SetBarValue(int value = 0);

    }

}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHealthBar : MonoBehaviour
    {
        public static UIHealthBar Instance { get; private set; }

        [SerializeField]
        private TextMeshProUGUI _healthValueText;

        private Slider _healthHerochar;

        private void Awake()
        {
            Instance = this;
            _healthHerochar = GetComponent<Slider>();
        }

        private void Start()
        {
            float maxValue = _healthHerochar.maxValue;
            SetHealthValue(maxValue);
        }

        public void SetMaxHealthValue(float maxValue)
        {
            _healthHerochar.maxValue = maxValue;
        }

        public void SetHealthValue(float value)
        {
            float maxValue = _healthHerochar.maxValue;
            if (value >= 0 && value <= maxValue)
            {
                _healthHerochar.value = value;
                _healthValueText.text = value + "/" + maxValue;
            }
        }

    }

}

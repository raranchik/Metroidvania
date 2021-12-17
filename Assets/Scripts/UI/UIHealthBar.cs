using UnityEngine.UI;

namespace UI
{
    public class UIHealthBar : UIBarBase
    {
        public new static UIHealthBar Instance { get; private set; }

        private Slider _healthHerochar;

        protected override void Awake()
        {
            Instance = this;
            _healthHerochar = GetComponent<Slider>();
        }

        public void SetBarMaxValue(int maxValue)
        {
            _healthHerochar.maxValue = maxValue;
            SetBarValue(maxValue);
        }

        public override void SetBarValue(int value = 0)
        {
            float maxValue = _healthHerochar.maxValue;
            if (value >= 0 && value <= maxValue)
            {
                _healthHerochar.value = value;
                _barValueText.text = value + "/" + maxValue;
            }
        }

    }

}

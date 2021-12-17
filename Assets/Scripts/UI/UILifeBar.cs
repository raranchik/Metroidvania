namespace UI
{
    public class UILifeBar : UIBarBase
    {
        public new static UILifeBar Instance { get; private set; }

        protected override void Awake()
        {
            Instance = this;
        }

        public override void SetBarValue(int value = 0)
        {
            if (value >= 0)
            {
                _barValueText.text = value + "X";
            }
        }

    }

}

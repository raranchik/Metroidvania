namespace UI
{
    public class UIScoreBar : UIBarBase
    {
        public new static UIScoreBar Instance { get; private set; }

        private int _score = 0;

        protected override void Awake()
        {
            Instance = this;
        }

        public override void SetBarValue(int value = 0)
        {
            _score++;
            _barValueText.text = _score + "X";
        }

        public void SetInitialValue()
        {
            _barValueText.text = 0 + "X";
        }

    }

}

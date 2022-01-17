namespace UI
{
    public class UIScoreBar : UIBarBase
    {
        public new static UIScoreBar Instance { get; private set; }
        public static int TotalPossibleScore = 0;

        private int _score = 0;

        public int Score
        {
            get => _score;
            private set => _score = value;
        }

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

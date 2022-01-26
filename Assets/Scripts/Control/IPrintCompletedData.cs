using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Control
{
    public interface IPrintCompletedData
    {
        public void PrintTime(in TextMeshProUGUI timeText, in float gameTime);

        public void PrintCompletionRate(in Image[] scoreImages, in int completedScoreSections);

    }

}
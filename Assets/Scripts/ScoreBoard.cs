using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;


public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI[] scoresText_10Pairs;
    public TextMeshProUGUI[] dateText_10Pairs;

    public TextMeshProUGUI[] scoresText_15Pairs;
    public TextMeshProUGUI[] dateText_15Pairs;

    public TextMeshProUGUI[] scoresText_20Pairs;
    public TextMeshProUGUI[] dateText_20Pairs;

    private void Start()
    {
        UpdateScoreboard();
    }

    public void UpdateScoreboard()
    {
        Config.UpdateScoreList();

        DisplayPairScoreData(Config.ScoreTimeList10Pairs, Config.PairNumberList10Pairs, scoresText_10Pairs, dateText_10Pairs);
        DisplayPairScoreData(Config.ScoreTimeList15Pairs, Config.PairNumberList15Pairs, scoresText_15Pairs, dateText_15Pairs);
        DisplayPairScoreData(Config.ScoreTimeList20Pairs, Config.PairNumberList20Pairs, scoresText_20Pairs, dateText_20Pairs);

    }

    private void DisplayPairScoreData(float[] scoreTimeList, string[] pairNumberList, TextMeshProUGUI[] scoreText, TextMeshProUGUI[] dataText)
    {
        for(var index = 0; index < 3; index++)
        {
            if (scoreTimeList[index] > 0)
            {
                var dataTime = Regex.Split(pairNumberList[index], "T");

                var minutes = Mathf.Floor(scoreTimeList[index] / 60);
                float seconds = Mathf.RoundToInt(scoreTimeList[index] % 60);

                scoreText[index].text = minutes.ToString("00") + ":" + seconds.ToString("00");
                dataText[index].text = dataTime[0] + " " + dataTime[1];
            }
            else
            {
                scoreText[index].text = " ";
                dataText[index].text = " ";
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_HighscoreList : MonoBehaviour
{
    [SerializeField] int scoreSlots = 3;
    [SerializeField] float spacing = 20f;
    [SerializeField] Transform container;
    [SerializeField] Transform template;

    ScoreList currentHighscoreList;

    private void Awake()
    {
        // holt die highscoredaten rein
        currentHighscoreList = SaveSystem.LoadScore();


        // Generiert die UI slots
        template.gameObject.SetActive(false);

        for (int i = 0; i < scoreSlots; i++)
        {
            Transform entryTransform = Instantiate(template, container);
            RectTransform entryRecTransform = entryTransform.GetComponent<RectTransform>();
            entryRecTransform.anchoredPosition = new Vector2(0, -spacing * i);

            entryTransform.gameObject.SetActive(true);

            int rank = i + 1;
            string rankString;

            switch (rank)
            {
                default:
                    rankString = rank + "TH";
                    break;
                case 1:
                    rankString = rank + "ST";
                    break;
                case 2:
                    rankString = rank + "ND";
                    break;
                case 3:
                    rankString = rank + "RD";
                    break;
            }

            entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;

            if (currentHighscoreList.scoreDataList.Count >= rank)
            {
                entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = currentHighscoreList.scoreDataList[i].score.ToString();
                entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = currentHighscoreList.scoreDataList[i].playerName.ToString();
                entryTransform.Find("timeText").GetComponent<TextMeshProUGUI>().text = currentHighscoreList.scoreDataList[i].date.ToString();
            }
        }


    }
}

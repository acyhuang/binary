using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject scoreOddPrefab;
    public GameObject scoreEvenPrefab;
    public Transform leaderboardContainer;

    private GameObject scoreObject;
    private TextMeshProUGUI scoreTMP;
    private TextMeshProUGUI dateTMP;

    private UserManager user;

    void Start()
    {
        user = GameObject.FindGameObjectWithTag("Player").GetComponent<UserManager>();
        LoadLeaderboard();
    }

    public void LoadLeaderboard()
    {
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < user.highScores.Count; i++)
        {
            ScoreEntry entry = user.highScores[i];
            if ((i + 1) % 2 == 0) 
            {
                scoreObject = Instantiate(scoreEvenPrefab, leaderboardContainer);
            } else {
                scoreObject = Instantiate(scoreOddPrefab, leaderboardContainer);
            }

            scoreTMP = scoreObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            dateTMP = scoreObject.transform.Find("Date").GetComponent<TextMeshProUGUI>();

            scoreTMP.text = entry.score.ToString();
            dateTMP.text = entry.date;
        }
    }
}

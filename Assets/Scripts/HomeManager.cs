using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class HomeManager : MonoBehaviour
{
    public Button settingsButton;
    public Button helpButton;
    public Button leaderboardButton;
    public RectTransform settings;
    public RectTransform leaderboard;

    // Start is called before the first frame update
    void Start()
    {
        helpButton.onClick.AddListener(OpenTutorial);
        settingsButton.onClick.AddListener(OpenSettings);
        settings.anchoredPosition = new Vector2(0, -1800);
        leaderboard.anchoredPosition = new Vector2(0, -1800);
    }

    public void OpenGame(){
        SceneManager.LoadScene("Game");
    }

    void OpenTutorial(){
        helpButton.onClick.RemoveListener(OpenTutorial);
        SceneManager.LoadScene("Tutorial");
    }

    void OpenSettings(){
        settings.DOAnchorPos(new Vector2(0, -100), 0.5f, true);
    }

    public void CloseSettings(){
        settings.DOAnchorPos(new Vector2(0, -1800), 0.5f, true);
    }

    public void OpenLeaderboard(){
        leaderboard.DOAnchorPos(new Vector2(0, -100), 0.5f, true);
    }

    public void CloseLeaderboard(){
        leaderboard.DOAnchorPos(new Vector2(0, -1800), 0.5f, true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartManager : MonoBehaviour
{
    public RectTransform howToPanel; 
    public Button startButton;
    public Button howToButton;
    public Button closeButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        howToPanel.anchoredPosition = new Vector2(0, -2000);
        howToButton.onClick.AddListener(OpenHowToPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame(){
        SceneManager.LoadScene("Game");
    }

    void OpenHowToPanel(){
        Debug.Log("How to play");
        howToPanel.DOAnchorPos(new Vector2(0, 1000), 0.5f, true);
        closeButton.onClick.AddListener(CloseHowToPanel);
    }

    void CloseHowToPanel(){
        howToPanel.DOAnchorPos(new Vector2(0, -2000), 0.5f, true);

    }
}

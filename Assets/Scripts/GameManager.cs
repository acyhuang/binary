using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI numDisplay;
    public List<Button> bitButtons = new List<Button>(); 
    public Button checkButton;
    public TextMeshProUGUI scoreDisplay;
    public Button exitButton;
    public Feedback feedback;
    public BitManager bitManager;

    private bool roundInProgress = false;
    private float[] levelDuration = new float[] {10.5f, 10.5f, 10.5f, 10.5f, 10.5f, 10.5f, 10.5f, 7.5f, 5.5f};
    private int[] levelDifficulty = new int[] {1, 2, 2, 3, 3, 3, 4, 4, 4};
    private int correctThisLevel = 0;
    private Coroutine roundCoroutine;
    private int currentLevel = 0;
    private int displayLevel = 1;
    private int prevNum = -1;
    private int prevHighScore;
    
    private int timeRemaining = 0;
    private int combo = 1;
    public int score = 0;
    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI prevDisplay;
    public RectTransform GameOver;
    public Button playAgainButton;
    public TextMeshProUGUI scoreDisplayGO; // scoreDisplay in GameOver
    public AudioSource errorSFX;
    public AudioSource correctSFX;
    public AudioSource levelupSFX;

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    // The greater the value of LowPassKernelWidthInSeconds, the slower the
    // filtered value will converge towards current input sample (and vice versa).
    float lowPassKernelWidthInSeconds = 1.0f;
    // This next parameter is initialized to 2.0 per Apple's recommendation,
    float shakeDetectionThreshold = 2.0f;

    float lowPassFilterFactor;
    Vector3 lowPassValue;
    bool shaking = false;

    private UserManager user;
    public LeaderboardManager leaderboardManager;

    void Start()
    {
        user = GameObject.FindGameObjectWithTag("Player").GetComponent<UserManager>();
        prevHighScore = user.highScores.FirstOrDefault()?.score ?? 0;
        prevDisplay.text = "PREV: " + prevHighScore.ToString();

        GameOver.anchoredPosition = new Vector2(0, -1800);
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;

        StartRound();
    }

    void Update()
    {
        // check for shake
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold && !shaking)
        {
            StartCoroutine(ShakeEvent());
        }
    }

    IEnumerator ShakeEvent()
    {
        shaking = true;
        Handheld.Vibrate();

        for (int i = 0; i < bitButtons.Count; i++){
            if (bitButtons[i].gameObject.GetComponent<BitButton>().value == true){
                bitButtons[i].gameObject.GetComponent<BitButton>().ToggleState();
            }
        }

        yield return new WaitForSeconds(2); // Only accept a new shake every 2sec.
        shaking = false;
    }

    private void StartRound()
    {
        roundInProgress = true;
        roundCoroutine = StartCoroutine(Timer());
        GenerateNumber();

        if (0 < currentLevel && currentLevel < 7)
        {
            bitManager.AddButtonToLeft();
        }
    }

    private void RoundComplete()
    {
        StopCoroutine(roundCoroutine);
        roundInProgress = false;
        if (currentLevel < 10)
        {
            currentLevel++;
        }
        displayLevel++;
        correctThisLevel = 0;

        levelDisplay.text = "LEVEL " + displayLevel.ToString();

        StartRound();
    }

    IEnumerator Timer()
    {
        float start = Time.time;
        float elapsed = 0;

        while (elapsed < levelDuration[currentLevel] - 0.01 && roundInProgress)
        {
            elapsed = Time.time - start;        
            timeRemaining = Mathf.FloorToInt((levelDuration[currentLevel] - elapsed) % 60);
            timeDisplay.text = string.Format("{0}:{1:00}", 0, timeRemaining);

            yield return null;
        }

        if (correctThisLevel < levelDifficulty[currentLevel])
        {
            OpenGameOver();
            //ExitGame();
        }
    }

    private int GetMinByBits(int numBits)
    {
        switch (numBits)
        {
            case 1:
                return 0;
            case 2:
                return 0;
            case 3:
                return 2;
            case 4:
                return 6;
            case 5:
                return 14;
            case 6:
                return 30;
            default:
                return 0;
        }
    }

    private int GetMaxByBits(int numBits)
    {
        switch (numBits)
        {
            case 1:
                return 1;
            case 2:
                return 3;
            case 3:
                return 7;
            case 4:
                return 15;
            case 5:
                return 31;
            case 6:
                return 63;
            default:
                return 127;
        }
    }

    public void GenerateNumber()
    {
        int min = GetMinByBits(currentLevel + 1);
        int max = GetMaxByBits(currentLevel + 1);
        int newNum;

        do {
            newNum = UnityEngine.Random.Range(min, max + 1);
        } while (newNum == prevNum);

        numDisplay.text = newNum.ToString();
        prevNum = newNum;
        
        for (int i = 0; i < bitButtons.Count; i++){
            if (bitButtons[i].gameObject.GetComponent<BitButton>().value == true){
                bitButtons[i].gameObject.GetComponent<BitButton>().ToggleState();
            }
        }
    }

    public void AddBitButton(Button newButton)
    {
        bitButtons.Add(newButton);
    }

    public int GetBitCount(){
        return bitButtons.Count;
    }

    public void CheckHandler()
    {
        if (CheckBinaryValue())
        {
            correctThisLevel++;
            combo++;
            score += (int)((timeRemaining + 1) * (currentLevel + 1) * (combo / 10.0));
            scoreDisplay.text = score.ToString();

            if (correctThisLevel < levelDifficulty[currentLevel])
            {
                StopCoroutine(roundCoroutine);
                GenerateNumber();
                roundCoroutine = StartCoroutine(Timer());
                if (user.wantsSFX)
                {
                    correctSFX.Play();
                }
            }
            else
            {
                RoundComplete();
                if (user.wantsSFX)
                {
                    levelupSFX.Play();
                }
            }
            
        } else {
            combo = 1;
            if (user.wantsSFX)
            {
                errorSFX.Play();
            }
        }
    }

    public bool CheckBinaryValue()
    {
        bool result = false;
        string binaryString = "";

        for (int i = bitButtons.Count - 1; i >= 0; i--)
        {
            binaryString += bitButtons[i].gameObject.GetComponent<BitButton>().value ? "1" : "0";
        }

        int decimalValue = Convert.ToInt32(binaryString, 2);

        int targetNum = int.Parse(numDisplay.text);
        
        if (decimalValue == targetNum){
            result = true;
        } else {
            feedback.ShowFeedback();
        }

        // Debug.Log("Binary: " + binaryString + ", Decimal: " + decimalValue); 
        return result;       
    }

    public void OpenGameOver()
    {
        user.AddHighScore(score);
        leaderboardManager.LoadLeaderboard();

        GameOver.DOAnchorPos(new Vector2(0, 700), 0.5f, true);
        scoreDisplayGO.text = score.ToString();
    }

    public void NewGame()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("Game"); 
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Home");
    }
}



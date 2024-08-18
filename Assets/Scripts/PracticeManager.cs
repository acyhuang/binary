using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class PracticeManager : MonoBehaviour
{
    public TextMeshProUGUI numDisplay;
    public Button checkButton;
    public Button exitButton;
    public Feedback feedback;
    public GameObject bitPrefab;
    public Transform bitParent; 
    public List<GameObject> bitButtons = new List<GameObject>(); 
    public int bitButtonsCount = 1;
    // public PracticeBitManager bitManager;
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 2.0f;
    public RectTransform directions;

    float lowPassFilterFactor;
    Vector3 lowPassValue;
    bool shaking = false;

    // Start is called before the first frame update
    void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;

        StartRound();
    }

    // Update is called once per frame
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
        GenerateNumber();
    }

    private void RoundComplete()
    {
        StartRound();
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
        int min = GetMinByBits(bitButtonsCount);
        int max = GetMaxByBits(bitButtonsCount);

        // Debug.Log("bitButtonsCount=" + bitButtonsCount.ToString());
        // Debug.Log("min=" + min.ToString() + ", max=" + max.ToString());
        // Debug.Log("---");

        int new_num = UnityEngine.Random.Range(min, max + 1);
        numDisplay.text = new_num.ToString();
        
        for (int i = 0; i < bitButtons.Count; i++){
            if (bitButtons[i].gameObject.GetComponentInChildren<BitButton>().value == true){
                bitButtons[i].gameObject.GetComponentInChildren<BitButton>().ToggleState();
            }
        }
    }

    public void AddBit(){
        if (bitButtonsCount < 8){
            GameObject newBit = Instantiate(bitPrefab, bitParent);
            TMP_Text bitNumText = newBit.transform.Find("BitNum").GetComponent<TMP_Text>();
            bitNumText.text = Math.Pow(2, bitButtonsCount).ToString();

            RectTransform rectTransform = newBit.GetComponent<RectTransform>();

            Vector2 finalPosition = new Vector2(100, rectTransform.anchoredPosition.y);
            rectTransform.anchoredPosition = finalPosition;
            HorizontalLayoutGroup layoutGroup = bitParent.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.enabled = true;
            
            newBit.transform.SetAsFirstSibling();

            bitButtonsCount++;
            bitButtons.Add(newBit);
            GenerateNumber();
        }
        
    }
    public void RemoveBit() {
        if (bitButtons.Count > 0) {
            GameObject lastBit = bitButtons[bitButtons.Count - 1];

            bitButtons.RemoveAt(bitButtons.Count - 1);
            Destroy(lastBit); 
            bitButtonsCount--;
            GenerateNumber();
        }
        
    }

    public void CheckHandler()
    {
        if (CheckBinaryValue())
        {
            GenerateNumber();
        } 
    }

    public bool CheckBinaryValue()
    {
        bool result = false;
        string binaryString = "";

        for (int i = bitButtons.Count - 1; i >= 0; i--)
        {
            binaryString += bitButtons[i].gameObject.GetComponentInChildren<BitButton>().value ? "1" : "0";
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
    public void CloseDirections(){
        directions.DOAnchorPos(new Vector2(0, -1800), 0.5f, true);
    }
    public void ExitGame(){
        SceneManager.LoadScene("Home");
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    
    public RectTransform[] panelRect;
    int panelIndex = 0;
    public GameObject nextButton;
    public GameObject bitPrefab;
    public Transform bitParent; 
    public List<GameObject> bitButtons = new List<GameObject>(); 
    public int bitButtonsCount = 1;
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    public float minSwipeDistance = 20f;
    void Start()
    {
        for (int i = 1; i < panelRect.Length; i++){
            panelRect[i].anchoredPosition = new Vector2(1200, 0);
        }
    }

    void Update(){
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                if ((Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x) > minSwipeDistance) && (fingerDownPosition.x < fingerUpPosition.x))
                {
                    PreviousPanel();

                }
                fingerDownPosition = Vector2.zero;
                fingerUpPosition = Vector2.zero; ;
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
        }
    }
    public void RemoveBit() {
        if (bitButtons.Count > 0) {
            GameObject lastBit = bitButtons[bitButtons.Count - 1];

            bitButtons.RemoveAt(bitButtons.Count - 1);
            Destroy(lastBit); 
            bitButtonsCount--; 
        }
    }

    public void NextPanel(){
        if (panelIndex < panelRect.Length+1){
            panelRect[panelIndex].DOAnchorPos(new Vector2(-1200, 0), 0.5f, true);
            panelIndex++;
            panelRect[panelIndex].DOAnchorPos(new Vector2(0, 0), 0.5f, true);
            if (panelIndex >= panelRect.Length-1){
                nextButton.SetActive(false);
            }
        }
        switch (panelIndex) {
            case 1:
                AddBit();
                AddBit();
                AddBit();
                break;
            case 2:
                if (bitButtons[0].gameObject.GetComponentInChildren<BitButton>().value == false){
                    bitButtons[0].gameObject.GetComponentInChildren<BitButton>().ToggleState();
                }
                if (bitButtons[1].gameObject.GetComponentInChildren<BitButton>().value == true){
                    bitButtons[1].gameObject.GetComponentInChildren<BitButton>().ToggleState();
                }
                if (bitButtons[2].gameObject.GetComponentInChildren<BitButton>().value == false){
                    bitButtons[2].gameObject.GetComponentInChildren<BitButton>().ToggleState();
                }
                if (bitButtons[3].gameObject.GetComponentInChildren<BitButton>().value == true){
                    bitButtons[3].gameObject.GetComponentInChildren<BitButton>().ToggleState();
                }
                break;
            case 3:
                bitParent.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void PreviousPanel(){
        if (panelIndex > 0){
            panelRect[panelIndex].DOAnchorPos(new Vector2(1200, 0), 0.5f, true);
            panelIndex--;
            panelRect[panelIndex].DOAnchorPos(new Vector2(0, 0), 0.5f, true);
            nextButton.SetActive(true);
        }
        switch (panelIndex) {
            case 0:
                RemoveBit();
                RemoveBit();
                RemoveBit();
                break;
            case 1:
                if (bitButtons[0].gameObject.GetComponentInChildren<BitButton>().value == true){
                    bitButtons[0].gameObject.GetComponentInChildren<BitButton>().ToggleState();
                }
                if (bitButtons[1].gameObject.GetComponentInChildren<BitButton>().value == true){
                    bitButtons[1].gameObject.GetComponentInChildren<BitButton>().ToggleState();
                }
                if (bitButtons[2].gameObject.GetComponentInChildren<BitButton>().value == true){
                    bitButtons[2].gameObject.GetComponentInChildren<BitButton>().ToggleState();
                }
                if (bitButtons[3].gameObject.GetComponentInChildren<BitButton>().value == true){
                    bitButtons[3].gameObject.GetComponentInChildren<BitButton>().ToggleState();
                }
                break;
            case 2:
                bitParent.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void CloseTutorial(){
        SceneManager.LoadScene("Home");
    }

    public void OpenPractice(){
        SceneManager.LoadScene("Practice");
    }

    public void OpenPlay(){
        SceneManager.LoadScene("Game");
    }
}

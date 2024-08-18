using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BitManager : MonoBehaviour
{
    public GameObject bitPrefab;
    public Transform bitParent; 
    public GameManager gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        GameObject newButtonGO = Instantiate(bitPrefab, bitParent);
        newButtonGO.transform.SetAsFirstSibling(); 
        Button newButton = newButtonGO.GetComponent<Button>();
        gameManager.AddBitButton(newButton);
    }

    public void AddButtonToLeft()
    {
        if (gameManager.GetBitCount() < 8){
            GameObject newButtonGO = Instantiate(bitPrefab, bitParent);
            RectTransform rectTransform = newButtonGO.GetComponent<RectTransform>();

            HorizontalLayoutGroup layoutGroup = bitParent.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.enabled = false;

            Vector2 startPosition = new Vector2(-rectTransform.rect.width - 200, 100);
            rectTransform.anchoredPosition = startPosition;
            rectTransform.DOAnchorPos(new Vector2(100, rectTransform.anchoredPosition.y), 0.5f).SetEase(Ease.OutBack)
            .OnComplete(() => {
                layoutGroup.enabled = true;
                newButtonGO.transform.SetAsFirstSibling();
            });

            Button newButton = newButtonGO.GetComponent<Button>();
            gameManager.AddBitButton(newButton);
        }
    }
}
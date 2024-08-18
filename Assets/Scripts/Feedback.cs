using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback : MonoBehaviour
{
    public CanvasGroup feedbackDisplay;
    // Start is called before the first frame update
    void Start()
    {
        feedbackDisplay.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowFeedback()
    {
        StartCoroutine(FadeFeedback());
    }
    IEnumerator FadeFeedback()
    {
        feedbackDisplay.alpha = 1;

        yield return new WaitForSeconds(1);

        float fadeDuration = 0.5f;
        float startTime = Time.time;
        while (Time.time - startTime < fadeDuration)
        {
            feedbackDisplay.alpha = 1 - ((Time.time - startTime) / fadeDuration);
            yield return null;
        }

        feedbackDisplay.alpha = 0;
    }

}


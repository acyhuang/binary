using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    public float minSwipeDistance = 20f;

    public delegate void SwipeRightAction();
    public static event SwipeRightAction OnSwipeRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Touch touch in Input.touches)
        {
            // if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                DetectSwipe();
            }
            // }
        }
    }
    void DetectSwipe()
    {
        if ((Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x) > minSwipeDistance) && (fingerDownPosition.x < fingerUpPosition.x))
        {
            OnSwipeRight?.Invoke();

        }

        // reset positions
        fingerDownPosition = Vector2.zero;
        fingerUpPosition = Vector2.zero;
    }
}



using UnityEngine;
using System.Collections.Generic;
using Unity.Hierarchy;

public class TouchDetection : MonoBehaviour
{
    [Header("Timings")]
    public float holdTime = 0.5f;   // time needed for it to be a hold
    public float startTime;
    public bool isHold;
    public bool isTouching;

    // for holding notes that go across lanes
    public Vector2 startPos;
    GameObject startLane;
    public Vector2 endPos;
    GameObject endLane;

    void Update()
    {
        HandleTouch();
    }

    // should handle the touches and determine if they are holds/tap
    public void HandleTouch()
    {
        // loop through all touches made
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 pos = touch.position;

            // check for touch 
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(pos);

                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider == GetComponent<Collider>())
                {
                    isTouching = true;
                    isHold = false;
                    startTime = Time.time;
                    startPos = pos; // get the start pos of hold
                    startLane = gameObject;
                    Debug.Log("Tap");
                }
            }
            // check for hold
            else if (isTouching && (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            {
                // check the hold time to quantify hold
                if (!isHold && Time.time - startTime >= holdTime)
                {
                    isHold = true;
                }
            }
            // action for when touch ends
            else if (isTouching && touch.phase == TouchPhase.Ended)
            {
                endPos = pos;
                Ray endRay = Camera.main.ScreenPointToRay(endPos); 
                endLane = null;
                
                if (Physics.Raycast(endRay, out RaycastHit endHit))
                {
                    endLane = endHit.collider.gameObject;
                }
                // reset hold and touch
                isTouching = false;
                isHold = false;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour
{
    // assign in inspector for each lane
    [SerializeField] private RectTransform[] lanes; 

    void Update()
    {
        // check for touch input
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    CheckTouch(touch.position);
                }
            }
        }
    }
    void CheckTouch(Vector2 screenPos)
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(lanes[i], screenPos))
            {
                Debug.Log($"Touched Lane {i}");
                // TODO: Handle note hit in lane i
                break;
            }
        }
    }
}

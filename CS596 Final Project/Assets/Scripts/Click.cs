using UnityEngine;
using UnityEngine.UI;

public class Click : MonoBehaviour
{
    public float threshold = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
    }
}

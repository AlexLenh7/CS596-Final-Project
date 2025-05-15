using Unity.VisualScripting;
using UnityEngine;

public class NoteBody : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float distFromTail = 0;
    public GameObject tail;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (tail)
        {
            gameObject.transform.localPosition
            = new
            Vector3(tail.transform.localPosition.x,
            tail.transform.localPosition.y - distFromTail,
            tail.transform.localPosition.z);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }
}

using UnityEngine;

public class Note : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] float time = 0;
    [SerializeField] int lane = 1;
    [SerializeField] enum NoteType { Tap, Hold };
    [SerializeField] string noteType = "None";
    [SerializeField] float holdTime = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

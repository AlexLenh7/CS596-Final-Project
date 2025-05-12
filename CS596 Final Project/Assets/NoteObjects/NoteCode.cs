using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteCode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Another script will spawn this note object inside of the lane container and at a specifed lane's X coord
    //This script's purpose is to just traverse the note down the lane at a certain speed in the Y axis

    public float speed = 20f;
    void Start()
    {
        //speed = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        //print("Moving down");

        transform.Translate((new Vector3(0, -1f, 0)) * speed * Time.deltaTime);

    }
}

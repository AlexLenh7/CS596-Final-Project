using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SubsystemsImplementation;

public class NoteCode : MonoBehaviour
{
    public GameObject explosionVFX;
    public GameObject lightVFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Another script will spawn this note object inside of the lane container and at a specifed lane's X coord
    //This script's purpose is to just traverse the note down the lane at a certain speed in the Y axis

    public Note note;
    public float holdStartPos; //y-pos when player started holding

    public float startTime = 0;
    public float endTime = 0;
    public float elapsed = 0;

    public float relPosY; //Relative pos to the parent (aka the lane, which should be .6 y-axis)
    public float volume = .15f;
    public Rythm rythm; //IMPORTANT: Must be set by NoteSpawner!

    [SerializeField] AudioClip tapSound;
    void Start()
    {
        //speed = 50f;
        relPosY = .6f;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //print("Moving down");
        //elapsed = Time.time - startTime;
        elapsed += Time.deltaTime;
        //Old Transform Code
        //transform.Translate((new Vector3(0, -1f, 0)) * speed * Time.deltaTime);

        //The ratio uses the timer from when the note spawns divided by the time it takes to reach A to B
        if (endTime > 0)
        {
            //float interpolationRatio = (1 - (elapsed / endTime));
            float interpolationRatio = (elapsed / endTime);
            //print(interpolationRatio);
            transform.localPosition = Vector3.Lerp(new Vector3(0,relPosY,0), new Vector3(0, -.1799f, 0), interpolationRatio);
            
            if (interpolationRatio >= 1) {       
                
                //Interpolate further instead of destroying the note when running the regular gameplay

                SoundManager.instance.playSound(tapSound, transform, volume);
                //print("Played Tap");

                DestroySelf(true);
            }
        }
    }

    public void DestroySelf(bool missed)
    {
        //Dequeue first item in rythm's queue as that item should be us since we die by FIFO fashion
        rythm.spawnedNotes[note.lane].Dequeue();
        
        // add a explosion fx to the note
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }

        if (missed)
        {
            rythm.MissedNote();
        }

        Destroy(gameObject);
    }
}

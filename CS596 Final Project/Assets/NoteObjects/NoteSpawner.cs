using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject laneContainer;
    public GameObject lane1;
    public GameObject lane2;
    public GameObject lane3;
    public GameObject lane4;
    public GameObject lane5;

    public GameObject singleNote;

    public GameObject spawnedNote;

    public BeatmapParser beatmapParser;

    public List<Note> parsedNotes = new List<Note>();
    public Dictionary<string, GameObject> laneNums = new Dictionary<string, GameObject>();
    
    bool mapIsReady = false;
    bool isPlaying = false;
    float delayTime = 0;

    [SerializeField] AudioClip macaronMoon;
    [SerializeField] AudioClip freedomDive;
    [SerializeField] AudioClip masquerade;
    [SerializeField] AudioClip currentSong;
    float startTime = 0;

    private void Start()
    {
        //mapIsReady = false;
        currentSong = macaronMoon;
    }
    public void generateMap(List<Note> noteMap) //Add and argument for delay time
    {
        //beatmapParser.ParseBeatmap(songName);
        //print("parsed Notes Stored");
        parsedNotes = noteMap;

        //print("After parsed Notes Stored");
        //print(parsedNotes);
        laneNums["0"] = lane1;
        laneNums["1"] = lane2;
        laneNums["2"] = lane3;
        laneNums["3"] = lane4;
        laneNums["4"] = lane5;
        //print("lanes set");


        //Paste to update()
        //print(mapIsReady);

        mapIsReady = true;
        startTime = Time.time;
        //SoundManager.instance.playSound(testSound, transform, .3f);
        delayTime = .75f;
    }

    // Update is called once per frame
    void Update()
    {
        if (mapIsReady == true && parsedNotes.Count > 0)
        {
            float elapsed = Time.time - startTime;
            Debug.Log("Elapsed time: " + elapsed + " seconds");

            print(parsedNotes[0].time);
            print(elapsed >= parsedNotes[0].time);
            //for (int i = 0; i < parsedNotes.Count; i++)

            if (elapsed >= parsedNotes[0].time)
            {
                print("spawning note on lane " + parsedNotes[0].lane);
                spawnedNote = Instantiate(singleNote, new Vector3(0, .6f, 0), Quaternion.identity);
                spawnedNote.transform.SetParent(laneNums[parsedNotes[0].lane.ToString()].transform);
                spawnedNote.GetComponent<Transform>().localPosition = new Vector3(0, .6f, 0);
                spawnedNote.transform.localScale = new Vector3(1, .125f, 0);

                //Determine speed of note.
                spawnedNote.GetComponent<NoteCode>().speed = 150; //Calculate the speed using the BPM in a formula

                print(parsedNotes[0]);
                parsedNotes.RemoveAt(0);

            }

            if (elapsed >= delayTime)
            {
                //startTime = delayTime;
                if (isPlaying == false)
                {
                    SoundManager.instance.playSound(currentSong, transform, .25f);
                    isPlaying = true;

                }
                
            }
            

        }
    }
}

using System;
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
    public List<float> spawnTimes = new List<float>(); // Contains time for each note to spawn

    public Dictionary<string, GameObject> laneNums = new Dictionary<string, GameObject>();
    
    bool mapIsReady = false;
    bool isPlaying = false;
    float songDelayTime = 0;
    public float songOffset = 0;

    [SerializeField] AudioClip macaronMoon;
    [SerializeField] AudioClip freedomDive;
    [SerializeField] AudioClip masquerade;
    [SerializeField] AudioClip futureCandy;
    [SerializeField] AudioClip sidequest;
    [SerializeField] AudioClip currentSong;
    float startTime = 0;
    float elapsed = 0;


    float timeToHit = 0; //less is faster, more is slower 

    private Rythm rythm;

    private void Start()
    {
        //mapIsReady = false;
        currentSong = currentSong;

        rythm = GetComponent<Rythm>();
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
        //print(mapIsReady)
        songOffset = GetComponent<BeatmapParser>().initialOffset; //.010f; //.1351f //If early increase, if late decrease
        timeToHit = 2f;
        songDelayTime = timeToHit + 1f;

        for (int i = 0; i < parsedNotes.Count; i++)
        {
            spawnTimes.Add(parsedNotes[i].time - timeToHit + songDelayTime + songOffset);

            if (parsedNotes[i].type == NoteType.Hold)
            {
                //spawnTimes.Add(parsedNotes[i].time - timeToHit + songDelayTime + songOffset + parsedNotes[i].holdTime);
            }
            //print(spawnTimes[i]);
        }

        mapIsReady = true;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (mapIsReady == true && parsedNotes.Count > 0)
        {
            //elapsed = Time.time - startTime;
            elapsed += Time.deltaTime;
            //songElapsed = Time.time - songStartTime;

            //Debug.Log("Elapsed time: " + elapsed + " seconds");

            //print(parsedNotes[0].time);
            //print(songElapsed);

            if (elapsed >= songDelayTime)
            {
                //startTime = delayTime;
                if (isPlaying == false)
                {
                    //songStartTime = Time.time;
                    //songStartTime = delayTime;
                    SoundManager.instance.playSound(currentSong, transform, .25f);
                    isPlaying = true;
                }

            }
            //print(spawnTimes[0]);
            //if (elapsed >= parsedNotes[0].time)
            if (elapsed >= spawnTimes[0])
            {

                //print("spawning note on lane " + parsedNotes[0].lane);
                spawnedNote = Instantiate(singleNote, new Vector3(0, .6f, 0), Quaternion.identity);
                spawnedNote.transform.SetParent(laneNums[parsedNotes[0].lane.ToString()].transform);
                spawnedNote.GetComponent<Transform>().localPosition = new Vector3(0, .6f, 0);
                spawnedNote.transform.localScale = new Vector3(1, .125f, 0);

                //Determine speed of note.
                spawnedNote.GetComponent<NoteCode>().endTime = timeToHit;

                //Copy Note info for Rythm
                spawnedNote.GetComponent<NoteCode>().note = parsedNotes[0];

                //Prevent re-searching for Rythm
                spawnedNote.GetComponent<NoteCode>().rythm = rythm;

                if (parsedNotes[0].type == NoteType.Hold)
                {
                    spawnedNote.GetComponent<SpriteRenderer>().color = Color.cyan;
                }

                /*               
                if (parsedNotes[0].type == NoteType.Hold)
                {
                    //print("spawning note on lane " + parsedNotes[0].lane);
                    spawnedNote = Instantiate(singleNote, new Vector3(0, .6f, 0), Quaternion.identity);
                    spawnedNote.transform.SetParent(laneNums[parsedNotes[0].lane.ToString()].transform);
                    spawnedNote.GetComponent<Transform>().localPosition = new Vector3(0, .6f, 0);
                    spawnedNote.transform.localScale = new Vector3(1, .125f, 0);

                    //Determine speed of note
                    spawnedNote.GetComponent<NoteCode>().endTime = timeToHit;

                }
                */

                //Add spawned note to rythm's queue
                rythm.spawnedNotes.Enqueue(spawnedNote);

                //print(parsedNotes[0]);              
                parsedNotes.RemoveAt(0);
                spawnTimes.RemoveAt(0);
            }
        }
    }
}

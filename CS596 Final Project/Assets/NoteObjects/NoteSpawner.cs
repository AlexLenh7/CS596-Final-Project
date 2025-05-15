using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class NoteSpawner : MonoBehaviour
{
    public GameObject laneContainer;
    public GameObject lane1;
    public GameObject lane2;
    public GameObject lane3;
    public GameObject lane4;
    public GameObject lane5;
    public int laneCount = 5;

    public GameObject singleNote;
    public GameObject holdNote;
    public GameObject holdBody;
    public GameObject spawnedNote;
    public GameObject spawnedBody;

    public BeatmapParser beatmapParser;

    public List<Note> parsedNotes = new List<Note>();
    public List<float> spawnTimes = new List<float>(); //Contains time for each note to spawn

    public Dictionary<string, GameObject> laneNums = new Dictionary<string, GameObject>();
    
    bool mapIsReady = false;
    bool isPlaying = false;
    float songDelayTime = 0;
    public float songOffset = 0;

    string songName = SongSelection.selectedSongName;

    [SerializeField] AudioClip macaronMoon;
    [SerializeField] AudioClip freedomDive;
    [SerializeField] AudioClip masquerade;
    [SerializeField] AudioClip futureCandy;
    [SerializeField] AudioClip sidequest;
    [SerializeField] AudioClip renaiCirculation;
    AudioClip currentSong;

    private Dictionary<string, AudioClip> songLibrary;

    float elapsed = 0;
    float timeToHit = 0; //less is faster, more is slower 

    //Note start and end detection
    
    float longNotePart = 1;
    public List<GameObject> longPair = new List<GameObject>();
    private Rythm rythm;

    private void Start()
    {
        // find the song name
        string songName = SongSelection.selectedSongName;

        if (songName == macaronMoon.name)
            currentSong = macaronMoon;
        else if (songName == freedomDive.name)
            currentSong = freedomDive;
        else if (songName == masquerade.name)
            currentSong = masquerade;
        else if (songName == futureCandy.name)
            currentSong = futureCandy;
        else if (songName == sidequest.name)
            currentSong = sidequest;
        else if (songName == renaiCirculation.name)
            currentSong = renaiCirculation;
        else
            Debug.LogError("Song not recognized: " + songName);

        rythm = GetComponent<Rythm>();
    }

    public void generateMap(List<Note> noteMap) //Add and argument for delay time
    {
        parsedNotes = noteMap;

        laneNums["0"] = lane1;
        laneNums["1"] = lane2;
        laneNums["2"] = lane3;
        laneNums["3"] = lane4;
        laneNums["4"] = lane5;

        songOffset = GetComponent<BeatmapParser>().initialOffset; //.010f; //.1351f //If early increase, if late decrease
        timeToHit = 2f;
        songDelayTime = timeToHit + 1f;

        for (int i = 0; i < parsedNotes.Count; i++)
        {
            spawnTimes.Add(parsedNotes[i].time - timeToHit + songDelayTime + songOffset);

            if (parsedNotes[i].type == NoteType.Hold)
            {
                Note tailNote = parsedNotes[i];
                tailNote.time += tailNote.holdTime;
                tailNote.holdTime = 0.01f;
                
                parsedNotes.Insert(i + 1, tailNote);
                i++;
            }
        }   

        mapIsReady = true;
        print(parsedNotes.Count + " " + spawnTimes.Count);
        longNotePart = 1;
    }

    void Update()
    {
        if (mapIsReady == true && parsedNotes.Count > 0)
        {
            elapsed += Time.deltaTime;

            if (elapsed >= songDelayTime)
            {
                if (isPlaying == false)
                {
                    SoundManager.instance.playSound(currentSong, transform, .25f);
                    isPlaying = true;
                }
            }
            
            if (elapsed >= spawnTimes[0])
            {
                GameObject noteToUse;
                
                if (parsedNotes[0].type == NoteType.Hold)
                {
                    noteToUse = holdNote;
                }
                else
                {
                    noteToUse = singleNote;
                }

                //print("spawning note on lane " + parsedNotes[0].lane);
                spawnedNote = Instantiate(noteToUse, new Vector3(0, .6f, 0), Quaternion.identity);
                spawnedNote.transform.SetParent(laneNums[parsedNotes[0].lane.ToString()].transform);
                spawnedNote.GetComponent<Transform>().localPosition = new Vector3(0, .6f, 0);
                spawnedNote.transform.localScale = new Vector3(1, .125f, 0);

                //Determine speed of note.
                spawnedNote.GetComponent<NoteCode>().endTime = timeToHit;

                //Copy Note info for Rythm
                spawnedNote.GetComponent<NoteCode>().note = parsedNotes[0];

                //Prevent re-searching for Rythm
                spawnedNote.GetComponent<NoteCode>().rythm = rythm;

                spawnedNote.GetComponent<NoteCode>().volume = .15f;

                if (parsedNotes[0].type == NoteType.Hold)
                {
                    float midpoint;
                    GameObject noteBody;
                    float distance;

                    spawnedNote.GetComponent<SpriteRenderer>().color = Color.cyan;

                    if (longNotePart == 1)
                    {
                        longPair.Add(spawnedNote);
                        longNotePart++;
                    }
                    else if (longNotePart == 2)
                    {
                        longPair.Add(spawnedNote);
                        spawnedNote.GetComponent<NoteCode>().volume = .07f;
                        
                        longPair[0].GetComponent<SpriteRenderer>().color = Color.green;
                        longPair[1].GetComponent<SpriteRenderer>().color = Color.red;

                        longPair[1].GetComponent<NoteCode>().secondHoldNote = true;

                        float headPos = longPair[0].transform.localPosition.y;
                        float tailPos = .6f;
                        
                        //Generate note body
                        midpoint = (headPos + tailPos) / 2f;

                        //Create note body
                        distance = Mathf.Abs(headPos - tailPos);

                        noteBody = Instantiate(holdBody, new Vector3(0, midpoint, 0), Quaternion.identity);
                        noteBody.transform.SetParent(laneNums[parsedNotes[0].lane.ToString()].transform);
                        noteBody.transform.localScale = new Vector3(1, distance * (.3f / .10f), 1);
                        noteBody.transform.localPosition = new Vector3(0, midpoint, 0);

                        noteBody.GetComponent<NoteBody>().distFromTail = distance / 2;
                        noteBody.GetComponent<NoteBody>().tail = longPair[1];

                        longNotePart = 1;
                        longPair.Clear();
                    }
                }

                //Add spawned note to rythm's queue
                int lane = parsedNotes[0].lane;

                if (lane >= laneCount)
                {
                    Debug.LogError("Note contains invalid lane");
                }

                rythm.spawnedNotes[lane].Enqueue(spawnedNote);

                parsedNotes.RemoveAt(0);
                spawnTimes.RemoveAt(0);
            }
        }
    }
}

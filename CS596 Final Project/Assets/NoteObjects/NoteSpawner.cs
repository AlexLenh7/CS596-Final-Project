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

    private void Start()
    {
        mapIsReady = false;
    }
    void initialize()
    {
        mapIsReady = false;
        //laneNums["0"] = lane1;
        //laneNums["1"] = lane2;
        //laneNums["2"] = lane3;
        //laneNums["3"] = lane4;
        //laneNums["4"] = lane5; 

        if (mapIsReady == true)
        {
            for (int i = 0; i < parsedNotes.Count; i++)
            {
                spawnedNote = Instantiate(singleNote, new Vector3(0, .6f, 0), Quaternion.identity);
                spawnedNote.transform.SetParent(laneNums[parsedNotes[i].lane.ToString()].transform);
                spawnedNote.GetComponent<Transform>().localPosition = new Vector3(0, .6f, 0);


            }

        }

    }

    public void generateMap(string songName)
    {
        beatmapParser.ParseBeatmap(songName);
        parsedNotes = beatmapParser.parsedNotes;
        mapIsReady = true;
        laneNums["0"] = lane1;
        mapIsReady = false;

        laneNums["0"] = lane1;
        laneNums["1"] = lane2;
        laneNums["2"] = lane3;
        laneNums["3"] = lane4;
        laneNums["4"] = lane5;
        mapIsReady = true;
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

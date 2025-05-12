using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // testing beatmap parser
    
    public Rythm rythm;
    public Records record; //scriptable object that stores anything that needs to persist like high score and grades.
    //public ?? menuChoices; //Menu choice scriptable object

    //Is the game active
    bool gameActive = true;

    //read data from rythm
    float currScore = 0;
    float streak = 0;

    //Get highscore from scriptable object
    string songName = "name"; //menuChoices.songName;
    float highScore = 0;
    // public string artist = "artist";
    public string difficulty = "difficulty";
    string accuracy = "";
    float numMisses = 0;
    public List<Note> parsedNotes = new List<Note>();

    private void Start()
    {
        gameActive = true;
        songName = "YUC'e - macaron moon"; //menuChoices.songName;
        //songName = "FREEDOM DiVE - xi";
        //songName = "M2U - Masquerade";

        highScore = 0;//record.songList[songName].highScore;

        GetComponent<BeatmapParser>().ParseBeatmap(songName);
        GetComponent<NoteSpawner>().generateMap(GetComponent<BeatmapParser>().parsedNotes);

        currScore = 0;
        streak = 0;
    }
    void Update()
    {
        if (gameActive == true) // While the game is active, run detections and game element code.
        {
            currScore = rythm.score;
            streak = rythm.streak;
            //Do all detections here.
        }
        else //Otherwise, save necessary values into scriptable object records and change scene.
        {
            //If the current score is the best, save the full record to the current song's entry
            if (currScore > highScore)
            {
                record.songList[songName] = new SongData(songName, currScore, difficulty, accuracy, numMisses);
            }   

            //Change scene to something else
        }
    }

}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Rythm rythm;
    //public ?? menuChoices; //Menu choice scriptable object

    // Alternate screens to be shown
    public GameObject HP;
    public GameObject FailScreen;
    public GameObject ResultScreen;

    //Is the game active
    bool gameActive = true;

    //read data from rythm
    float currScore = 0;
    float streak = 0;
    int numMisses = 0;
    
    //Get highscore from scriptable object
    string songName = "name"; //menuChoices.songName;
    float highScore = 0f;
    float accuracy = 0f;

    public List<Note> parsedNotes = new List<Note>();
    int totalNotes = 0;

    public GameObject missLine;

    private void Start()
    {
        gameActive = true;

        // load the song from the static class selected in song selector
        string songName = SongSelection.selectedSongName;
        Debug.Log("Loaded song: " + songName);

        highScore = GetComponent<RecordSetup>().record.songList[songName].highScore;
        AudioListener.pause = false;

        //Set FPS
        Application.targetFrameRate = 60;

        GetComponent<BeatmapParser>().ParseBeatmap(songName);
        GetComponent<NoteSpawner>().generateMap(GetComponent<BeatmapParser>().parsedNotes);

        currScore = 0;
        streak = 0;
        totalNotes = parsedNotes.Count;

        Vector3 adjustedMissLinePos = missLine.transform.localPosition;
        adjustedMissLinePos.y -= rythm.maxAllowedNoteDelta;
        missLine.transform.localPosition = adjustedMissLinePos;
    }

    void CalculateAccuracy()
    {
        accuracy = currScore / (rythm.scoreEffects[(int)Rythm.effectIdxs.PERFECT] * totalNotes);
    }

    void Update()
    {
        if (gameActive == true) // While the game is active, run detections and game element code.
        {
            currScore = rythm.score;
            streak = rythm.streak;
            numMisses = rythm.numMisses;
            CalculateAccuracy();

            // Game over when hp falls below 0
            if (rythm.currHP <= 0)
            {
                HP.SetActive(false);
                FailScreen.SetActive(true);
                gameActive = false;
                AudioListener.pause = true;
                GetComponent<NoteSpawner>().parsedNotes.Clear(); 
            }
            
            // Result screen when no more notes and hp isn't 0
            if (rythm.currHP > 0 && GetComponent<NoteSpawner>().parsedNotes.Count == 0)
            {
                StartCoroutine(ShowResults());
            }
        }
        else //Otherwise, save necessary values into scriptable object records and change scene.
        {
            //If the current score is the best, save the full record to the current song's entry
            if (currScore > highScore)
            {
                string difficulty = GetComponent<RecordSetup>().record.songList[songName].highDiff;
                GetComponent<RecordSetup>().record.songList[songName] = new SongData(songName, currScore, difficulty, accuracy, numMisses);
            }
        }
    }

    IEnumerator<WaitForSeconds> ShowResults()
    {
        yield return new WaitForSeconds(2);
        HP.SetActive(false);
        ResultScreen.SetActive(true);
        ResultScreen.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = numMisses.ToString();
        gameActive = false;
    }
}

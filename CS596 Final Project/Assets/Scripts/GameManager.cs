using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Rythm rythm;
    public Records record; //scriptable object that stores anything that needs to persist like high score and grades.
    //public ?? menuChoices; //Menu choice scriptable object

    // Alternate screens to be shown
    public GameObject HP;
    public GameObject FailScreen;
    public GameObject ResultScreen;
    [SerializeField] AudioClip failSound;

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

        highScore = record.songList[songName].highScore;

        //Set FPS
        Application.targetFrameRate = 60;

        GetComponent<BeatmapParser>().ParseBeatmap(songName);
        GetComponent<NoteSpawner>().generateMap(GetComponent<BeatmapParser>().parsedNotes);

        currScore = 0;
        streak = 0;
        totalNotes = parsedNotes.Count;
        Debug.Log(totalNotes);

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
                SoundManager.instance.playSound(failSound, transform, .25f);
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
                string difficulty = record.songList[songName].highDiff;
                record.songList[songName] = new SongData(songName, currScore, difficulty, accuracy, numMisses);
            }
        }
    }

    IEnumerator<WaitForSeconds> ShowResults()
    {
        yield return new WaitForSeconds(2);
        HP.SetActive(false);
        ResultScreen.SetActive(true);
        gameActive = false;
    }
}

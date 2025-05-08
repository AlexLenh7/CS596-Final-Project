using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    string accuracy = "";
    float numMisses = 0;

    private void Start()
    {
        gameActive = true;
        //songName = menuChoices.songName;
        highScore = record.songList[songName].highScore;
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
            //save any relevant values to the record's list
            if (currScore > highScore)
            {
                record.songList[songName] = new SongData(songName, currScore, accuracy, numMisses);
            }

            
            

            //Change scene to something else
        }

        
    }

}

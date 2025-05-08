using UnityEngine;

public class SongData : MonoBehaviour
{
    //Data of a specific song
    public string songName = "name";
    public float highScore = 0;
    public string accuracy = "F";
    public float numMisses = 0;

    public SongData(string songName, float highScore, string accuracy, float numMisses)
    {
        this.songName = songName;
        this.highScore = highScore;
        this.accuracy = accuracy;
        this.numMisses = numMisses;
        
    }
}

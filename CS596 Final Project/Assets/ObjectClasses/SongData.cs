using System;
using UnityEngine;

public class SongData
{
    //Data of a specific song
    public string songName = "name";
    // public string artist = "artist";
    public string highDiff = "difficulty";
    public float highScore = 0;
    public string accuracy = "F";
    public float numMisses = 0;

    public SongData(string songName, float highScore, string highDiff, string accuracy, float numMisses)
    {
        this.songName = songName;
        this.highScore = highScore;
        this.highDiff = highDiff;
        this.accuracy = accuracy;
        this.numMisses = numMisses;
    }

    public void printSong()
    {
        Debug.Log(
                $"-------------------------------------------- \n" +
                $"Song Name: {this.songName}\n" +
                $"High Score: {this.highScore}\n" +
                $"Accuracy: {this.accuracy}\n" +
                $"Number of Misses: {this.numMisses}\n" +
                $"High Difficulty: {this.highDiff}\n"
            );
    }
}

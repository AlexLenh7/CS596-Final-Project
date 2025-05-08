using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Records", menuName = "Scriptable Objects/Records")]
public class Records : ScriptableObject
{
    //public string songName = "name";
    //public float highScore = 0;
    //public string accuracy = "F";
    //public float numMisses = 0;

    //Dictionary for all Songs to save scores to
    public Dictionary<string, SongData> songList = new Dictionary<string, SongData>();
    //songList[lastSong] = accuracy;

}

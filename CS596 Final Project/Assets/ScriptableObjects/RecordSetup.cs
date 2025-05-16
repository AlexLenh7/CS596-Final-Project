using System;
using UnityEngine;

public class RecordSetup : MonoBehaviour
{
    public Records record;
    // Setup empty record with default values
    void Awake()
    {
        //SongData(string songName, float highScore, string highDiff, string accuracy, float numMisses)
        record = new Records();

        //Easy
        record.songList["Hanazawa Kana - Renai Circulation"] 
            = new SongData("Hanazawa Kana - Renai Circulation", 0, "Easy", 0, 0);

        //Medium
        record.songList["M2U - Masquerade"]
            = new SongData("M2U - Masquerade", 0, "Medium", 0, 0);

        record.songList["YUC'e - macaron moon"]
            = new SongData("YUC'e - macaron moon", 0, "Medium", 0, 0);

        //Hard
        record.songList["YUC'e - Future Candy"]
            = new SongData("YUC'e Future Candy", 0, "Hard", 0, 0);

        record.songList["Redside - Sidequest"]
            = new SongData("Redside - Sidequest", 0, "Hard", 0, 0);

        //Expert
        record.songList["xi - FREEDOM DiVE"]
            = new SongData("xi - FREEDOM DiVE", 0, "Expert", 0, 0);

        int index = 0;
        foreach (var entry in record.songList)
        {
            SongData songRecord = entry.Value;

            Debug.Log($"[{index}]\n");
            songRecord.printSong();

            index++;
        }

    }
}

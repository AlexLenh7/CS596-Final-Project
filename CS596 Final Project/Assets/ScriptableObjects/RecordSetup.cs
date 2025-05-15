using System;
using UnityEngine;

public class RecordSetup : MonoBehaviour
{
    public Records record;
    // Setup empty record with default values
    void Start()
    {
        //SongData(string songName, float highScore, string highDiff, string accuracy, float numMisses)

        //Easy
        record.songList["Hanazawa Kana - Renai Circulation"] 
            = new SongData("Hanazawa Kana - Renai Circulation", 0, "None", "None", 0);

        //Medium
        record.songList["M2U - Masquerade"]
            = new SongData("M2U - Masquerade", 0, "None", "None", 0);

        record.songList["YUC'e - macaron moon"]
            = new SongData("YUC'e - macaron moon", 0, "None", "None", 0);

        //Hard
        record.songList["Future Candy - YUC'e"]
            = new SongData("Future Candy - YUC'e", 0, "None", "None", 0);

        record.songList["Redside - Sidequest"]
            = new SongData("Redside - Sidequest", 0, "None", "None", 0);

        //Expert
        record.songList["FREEDOM DiVE - xi"]
            = new SongData("FREEDOM DiVE - xi", 0, "None", "None", 0);

        int index = 0;
        foreach (var entry in record.songList)
        {
            SongData songRecord = entry.Value;

            Debug.Log($"[{index}]\n");
            songRecord.printSong();

            index++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

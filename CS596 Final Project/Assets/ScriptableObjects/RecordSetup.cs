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
        record.songList["Bad Apple!! - Masayoshi Minoshima ft.nomico"] 
            = new SongData("Bad Apple!! - Masayoshi Minoshima ft.nomico", 0, "None", "None", 0);

        record.songList["Last Goodbye - toby fox"]
            = new SongData("Last Goodbye - toby fox", 0, "None", "None", 0);

        //Medium
        record.songList["Sidequest - Redside "]
            = new SongData("Sidequest - Redside ", 0, "None", "None", 0);

        record.songList["Yoru ni Kakeru - YOASOBI"]
            = new SongData("Yoru ni Kakeru - YOASOBI", 0, "None", "None", 0);

        //Hard
        record.songList["Future Candy - YUC'e"]
            = new SongData("Future Candy - YUC'e", 0, "None", "None", 0);

        record.songList["Sangatsu No Phantasia - Pastel Rain"]
            = new SongData("Sangatsu No Phantasia - Pastel Rain", 0, "None", "None", 0);

        //Expert
        record.songList["FREEDOM DiVE - xi"]
            = new SongData("FREEDOM DiVE - xi", 0, "None", "None", 0);

        record.songList["YUC’e - macaron moon"]
            = new SongData("YUC’e - macaron moon", 0, "None", "None", 0);


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

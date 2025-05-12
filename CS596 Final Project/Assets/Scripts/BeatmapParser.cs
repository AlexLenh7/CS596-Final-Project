using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.Interactions;

public class BeatmapParser : MonoBehaviour 
{
    private int totalKeys = 5;

    // store the paresed notes for each song
    public List<Note> BadApple = new List<Note>();
    public List<Note> LastGoodbye = new List<Note>();
    public List<Note> Sidequest = new List<Note>();
    public List<Note> YoruNiKakeru = new List<Note>();
    public List<Note> FutureCandy = new List<Note>();
    public List<Note> Masquerade = new List<Note>();
    public List<Note> FreedomDive = new List<Note>();
    public List<Note> MacaronMoon = new List<Note>();

    // call at the beginning of song to begin parse 
    private void Awake()
    {
        // get the folderpath and parse every file within path
        string folderPath = Path.Combine(Application.streamingAssetsPath, "Beatmaps");

        string[] osuFiles = Directory.GetFiles(folderPath);

        // goes through and parses every file within folder
        foreach (var file in osuFiles)
        {
            List<Note> songNotes = ParseBeatmap(file);
            string fileName = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();

            if (fileName.Contains("badapple"))
                BadApple = songNotes;
            else if (fileName.Contains("lastgoodbye"))
                LastGoodbye = songNotes;
            else if (fileName.Contains("sidequest"))
                Sidequest = songNotes;
            else if (fileName.Contains("yorunikakeru"))
                YoruNiKakeru = songNotes;
            else if (fileName.Contains("futurecandy"))
                FutureCandy = songNotes;
            else if (fileName.Contains("masquerade"))
                Masquerade = songNotes;
            else if (fileName.Contains("freedomdive"))
                FreedomDive = songNotes;
            else if (fileName.Contains("macaronmoon"))
                MacaronMoon = songNotes;
            else
                Debug.LogWarning($"Unrecognized beatmap name: {fileName}");
            ParseBeatmap(file);
        }
    }

    private List<Note> ParseBeatmap(string filePath)
    {
        List<Note> parsedNotes = new List<Note>();

        // read all lines within the file
        string[] lines = File.ReadAllLines(filePath);
        bool inHitObjects = false;

        // loop through the each line
        foreach (var line in lines)
        {
            // remove leading white space from lines
            string rawLine = line.Trim();

            if (rawLine.StartsWith("[HitObjects]"))
            {
                inHitObjects = true;
                continue;
            }

            if (!inHitObjects || string.IsNullOrWhiteSpace(rawLine) || rawLine.StartsWith("//"))
                continue;

            // split each line into respective parts (x),(y),(time),(type),(hitsound)
            string[] parts = rawLine.Split(',');
            
            if (parts.Length < 5) 
                continue;

            // get the necessary lines
            int lanePos = int.Parse(parts[0]); // translates to lane pos
            int timing = int.Parse(parts[2]); // time when the object is to be hit in ms from the beginning of the song
            int type = int.Parse(parts[3]); // type of note (single or hold)

            Note note = new Note(); // create the note object
            note.time = timing / 1000f; // convert time to seconds
            note.lane = MapToLane(lanePos, totalKeys); // set lane based off x pos

            // identify taps and holds
            if ((type & 128) != 0)
            {
                // ignore extras
                var extras = parts[5].Split(':');
                int endTime = int.Parse(extras[0]); // get the hold time
                note.type = NoteType.Hold;
                note.holdTime = (endTime - timing) / 1000f; // duration in seconds
            }
            else
            {
                note.type = NoteType.Tap;
                note.holdTime = 0;
            }

            parsedNotes.Add(note);
        }
        return parsedNotes;
    }
    
    // function to set lane from x pos
    private int MapToLane(int x, int keyCount)
    {
        float columnWidth = 512f / keyCount;
        return Mathf.Clamp(Mathf.FloorToInt(x / columnWidth), 0, keyCount - 1);
    }
}

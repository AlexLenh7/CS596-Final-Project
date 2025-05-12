using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.Interactions;

public class BeatmapParser : MonoBehaviour 
{
    private int totalKeys = 5;

    // store the paresed notes for each song
    public List<Note> parsedNotes = new List<Note>();

    // call within game manager or song selection script
    public void ParseBeatmap(string songName)
    {
        parsedNotes.Clear();

        string folderPath = Path.Combine(Application.streamingAssetsPath, "Beatmaps");
        string targetPath = Path.Combine(folderPath, songName + ".txt");

        // read all lines within the file
        string[] lines = File.ReadAllLines(targetPath);
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
    }
    
    // function to set lane from x pos
    private int MapToLane(int x, int keyCount)
    {
        float columnWidth = 512f / keyCount;
        return Mathf.Clamp(Mathf.FloorToInt(x / columnWidth), 0, keyCount - 1);
    }
}

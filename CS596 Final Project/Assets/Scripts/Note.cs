using System;
using UnityEngine;

public enum NoteType {Tap, Hold}

[Serializable]
public class Note
{
    public float time; // in seconds
    public int lane;
    public NoteType type;
    public float holdTime; // for hold notes
}

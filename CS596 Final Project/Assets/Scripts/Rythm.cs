using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rythm : MonoBehaviour
{
    [SerializeField] private AudioClip TapSound;
    [Range(0f, 1f)]
    public float volume = 1f;

    //Terry: I am adding variables to maintain score, streaks, etc.
    //Terry: Using the data collected from inputs, determine scores and conditions within this entire script
    
    //Game manager will read these variables to determine win conditions.
    public float currHP = 100; //Heal this value depending on accuracy
    public float maxHP = 100; //MaxHP
    public float score = 0; //add to this score based on input accuracy data
    public float streak = 0; //add to this value as long as the player does not land a miss

    //Amount of HP being subtracted 
    float dmgValue = .1f; //Should be taken from difficulty choice in the main menu's scriptable object

    //Frequency in seconds that you lose take the dmgValue
    float drainRate = .2f; //Should be taken from difficulty choice in the main menu's scriptable object

    public Transform timingLine;
    public Queue<GameObject> spawnedNotes = new Queue<GameObject>();

    [SerializeField]
    private float maxAllowedNoteDelta = 0.15f;

    void Start()
    {
        //Bleed damage over time
        StartCoroutine("dmgOverTime");
    }

    IEnumerator dmgOverTime()
    {
        while (currHP > 0)
        {
            currHP -= dmgValue;
            yield return new WaitForSeconds(drainRate);  //wait 1 second
        }
    }

    private bool WasNoteHit(float inputPos, float expectedPos)
    {
        return Mathf.Abs(inputPos - expectedPos) <= maxAllowedNoteDelta;
    }

    private bool WasProperHit(GameObject note, int lane, bool isHold)
    {
        NoteCode noteCode = note.GetComponent<NoteCode>();

        //Wrong lane
        if (lane != noteCode.note.lane)
        {
            return false;
        }

        if (isHold && noteCode.note.type == NoteType.Hold)
        {
            //The below implementation won't work since it isn't based on any hold implementation yet
            //For example, if the holds are to be long in height and moved from its center, these values would need to be added/subtracted by the length to the midpoint
            Debug.LogError("Hold detection doesn't properly work yet, it must be fixed once holds are implemented!");

            //Ensure note was started and ended at proper times, within the allowed delta
            if (!WasNoteHit(noteCode.holdStartPos, timingLine.localPosition.y) || !WasNoteHit(note.transform.localPosition.y, timingLine.localPosition.y))
            {
                return false;
            }
        }
        else if (!isHold && noteCode.note.type == NoteType.Tap)
        {
            //Ensure note is hit at proper time, within the allowed delta
            if (!WasNoteHit(note.transform.localPosition.y, timingLine.localPosition.y))
            {
                return false;
            }
        }
        else 
        {
            return false;
        }

        return true;
    }

    public void BeginTouch(int lane)
    {
        //Debug.Log("Touch began at lane " + lane);
        SoundManager.instance.playSound(TapSound, transform, volume);

        float lowestY = float.MaxValue;
        GameObject lowestNote = null;

        foreach (GameObject note in spawnedNotes)
        {
            NoteCode noteCode = note.GetComponent<NoteCode>();

            if (noteCode.note.lane != lane)
            {
                continue;
            }

            if (note.transform.localPosition.y < lowestY)
            {
                lowestY = note.transform.localPosition.y;
                lowestNote = note;
            }
        }

        if (lowestNote)
        {
            lowestNote.GetComponent<NoteCode>().holdStartPos = lowestNote.transform.localPosition.y;
        }
    }

    //TODO: Calculate score, streaks, and delete properly hit note
    public void EndTouch(int lane, bool isHold)
    {
        //Debug.Log("Touch ended at lane " + lane);
        bool properHit = false;

        //Check if there was a proper hit on any of the lowest notes
        foreach (GameObject note in spawnedNotes)
        {
            if (WasProperHit(note, lane, isHold))
            {
                properHit = true;
                break;
            }
        }

        if (!properHit)
        {
            Debug.Log("DAMAGE");
            currHP -= dmgValue;
            return;
        }

        Debug.Log("HIT");
    }
}

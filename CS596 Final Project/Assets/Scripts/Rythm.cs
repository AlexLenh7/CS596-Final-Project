using UnityEngine;
using System.Collections;

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

    [SerializeField]
    private float maxAllowedNoteDelta = 0.15f;

    private int noteStartLane;
    private float noteStartPos; //Y position

    //IMPORTANT: These variables need to be populated somehow, the specific way to do so can be figured out once note spawning is complete
    private Note lowestNote;
    private float noteEndTime;
    private float noteCurrPos;

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

    private void GetCurrNote()
    {
        Debug.LogError("Need to implement finding current Note!");
        UnityEditor.EditorApplication.isPlaying = false;
        return;
    }

    public void BeginTouch(int lane)
    {
        Debug.Log("Touch began at lane " + lane);
        SoundManager.instance.playSound(TapSound, transform, volume);

        GetCurrNote();
        noteStartLane = lane;
        noteStartPos = noteCurrPos;
    }

    private bool WasNoteHit(float inputPos, float expectedPos)
    {
        return Mathf.Abs(inputPos - expectedPos) <= maxAllowedNoteDelta;
    }

    //TODO: Calculate score, streaks, and delete properly hit note
    public void EndTouch(int lane, bool isHold)
    {
        Debug.Log("Touch ended at lane " + lane);

        //Wrong lane or didn't stay in lane
        if (lane != lowestNote.lane || lane != noteStartLane)
        {
            currHP -= dmgValue;
            return;
        }

        if (isHold && lowestNote.type == NoteType.Hold)
        {
            //Ensure note was started and ended at proper times, within the allowed delta
            if (!WasNoteHit(noteStartPos, timingLine.position.y) || !WasNoteHit(noteCurrPos, timingLine.position.y))
            {
                currHP -= dmgValue;
                return;
            }
        }
        else if (!isHold && lowestNote.type == NoteType.Tap)
        {
            //Ensure note is hit at proper time, within the allowed delta
            if (!WasNoteHit(noteCurrPos, timingLine.position.y))
            {
                currHP -= dmgValue;
                return;
            }
        }
        else 
        {
            currHP -= dmgValue;
            return;
        }
    }
}

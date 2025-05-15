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

    //Health/Score effects for perfect hit, great hit, good hit, miss
    public float[] healthEffects = new float[4] { 2f, 1f, -1f, -5f };
    public float[] scoreEffects = new float[4] { 300f, 150f, 50f, 0f };
    private enum effectIdxs { PERFECT, GREAT, OK, MISS, EFFECT_LENGTH };
    private int effectsSize = (int)effectIdxs.EFFECT_LENGTH;

    //Amount of HP being subtracted 
    float dmgValue = .1f; //Should be taken from difficulty choice in the main menu's scriptable object

    //Frequency in seconds that you lose take the dmgValue
    float drainRate = .2f; //Should be taken from difficulty choice in the main menu's scriptable object

    public Transform timingLine;

    //Queued notes indexed by lane
    public List<Queue<GameObject>> spawnedNotes = new List<Queue<GameObject>>();

    [SerializeField]
    private float maxAllowedNoteDelta = 0.05f;

    void Awake()
    {
        if (effectsSize != scoreEffects.Length || effectsSize != healthEffects.Length)
        {
            Debug.LogError("healthEffects or/and scoreEffects doesn't match effectsSize, this is not supported!");
            
            Debug.LogWarning("Limiting both effects to first 2 elements.");
            effectsSize = 1;
        }

        for (int i = 0; i < GetComponent<NoteSpawner>().laneCount; ++i)
        {
            spawnedNotes.Add(new Queue<GameObject>());
        }
    }

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

    //Returns an index in the range [0-3] determining which Health/Score effects to use
    private int CalculateAccuracyIdx(GameObject note)
    {
        Debug.LogError("Accuracy isn't calculated correctly for holds yet!");

        float delta = Mathf.Abs(note.transform.localPosition.y - timingLine.localPosition.y);

        if (delta > maxAllowedNoteDelta)
        {
            return effectsSize - 1;
        }

        //Round down the delta percentage multiplied by max index
        return (int)((effectsSize - 1) * (Mathf.Abs(note.transform.localPosition.y - timingLine.localPosition.y) / maxAllowedNoteDelta));
    }

    public void MissedNote()
    {
        streak = 0;
        currHP += healthEffects[(int)effectIdxs.MISS];
    }

    public void BeginTouch(int lane)
    {
        //Debug.Log("Touch began at lane " + lane);

        if (lane > spawnedNotes.Count)
        {
            Debug.LogError("Invalid lane provided in TouchDetection, lanes should be 0-indexed.");
        }

        SoundManager.instance.playSound(TapSound, transform, volume);

        float lowestY = float.MaxValue;
        GameObject lowestNote = null;

        foreach (GameObject note in spawnedNotes[lane])
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

    public void EndTouch(int lane, bool isHold)
    {
        //Debug.Log("Touch ended at lane " + lane);

        if (lane > spawnedNotes.Count)
        {
            Debug.LogError("Invalid lane provided in TouchDetection, lanes should be 0-indexed.");
        }

        GameObject hitNote = null;

        //Check if there was a proper hit on any of the lowest notes
        foreach (GameObject note in spawnedNotes[lane])
        {
            if (WasProperHit(note, lane, isHold))
            {
                hitNote = note;
                break;
            }
        }

        //Miss
        if (!hitNote)
        {
            MissedNote();
            return;
        }
        
        //Calculate how accurate the hit was and assign values accordingly
        int effectIdx = CalculateAccuracyIdx(hitNote);
        
        currHP += healthEffects[effectIdx];
        score += scoreEffects[effectIdx];

        hitNote.GetComponent<NoteCode>().DestroySelf(false);
        streak += 1;
    }
}

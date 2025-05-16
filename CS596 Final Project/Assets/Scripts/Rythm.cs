using CartoonFX;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rythm : MonoBehaviour
{
    public CFXR_ParticleText comboTextEffectPrefab;
    public RectTransform uiCanvas;
    CFXR_ParticleText comboTextInstance;

    [SerializeField] private AudioClip TapSound;
    [Range(0f, 1f)]
    public float volume = 1f;

    //Terry: I am adding variables to maintain score, streaks, etc.
    //Terry: Using the data collected from inputs, determine scores and conditions within this entire script

    //Game manager will read these variables to determine win conditions.
    public float currHP = 100;
    public float maxHP = 100;
    public float score = 0;
    public int streak = 0;
    public int numMisses = 0;

    //Health/Score effects for perfect hit, great hit, good hit, miss
    public float[] healthEffects = new float[4] { 2f, 1f, 0f, -5f };
    public float[] scoreEffects = new float[4] { 300f, 150f, 50f, 0f };
    public enum effectIdxs { PERFECT, GREAT, OK, MISS, EFFECT_LENGTH };
    private int effectsSize = (int)effectIdxs.EFFECT_LENGTH;

    //Amount of HP being subtracted 
    float dmgValue = .1f; //Should be taken from difficulty choice in the main menu's scriptable object

    //Frequency in seconds that you lose take the dmgValue
    float drainRate = .2f; //Should be taken from difficulty choice in the main menu's scriptable object

    public Transform timingLine;

    //Queued notes indexed by lane
    public List<Queue<GameObject>> spawnedNotes = new List<Queue<GameObject>>();

    public float maxAllowedNoteDelta = 0.05f;

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
        string difficulty = GameObject.Find("Game Manager").GetComponent<RecordSetup>().record.songList[SongSelection.selectedSongName].highDiff;

        switch (difficulty)
        {
            case "Easy":
                dmgValue = 0.1f;
                break;
            case "Medium":
                dmgValue = 0.2f;
                break;
            case "Hard":
                dmgValue = 0.4f;
                break;
            case "Expert":
                dmgValue = 0.8f;
                break;
        }

        Debug.Log("Damage value every " + drainRate + " seconds: " + dmgValue);

        //Bleed damage over time
        StartCoroutine("dmgOverTime");

        comboTextInstance = Instantiate(
            comboTextEffectPrefab,
            uiCanvas   // parent
        );
        comboTextInstance.transform.localScale = Vector3.one;
        comboTextInstance.isDynamic = true;
    }

    public void AddToStreak(int delta)
    {
        streak += delta;
        UpdateStreakDisplay();
    }
    void UpdateStreakDisplay()
    {
        // convert int to string
        string text = streak.ToString();

        // push it into the effect
        comboTextInstance.UpdateText(text);

        // (re)play the particle effect so you see it every time
        var ps = comboTextInstance.GetComponent<ParticleSystem>();
        ps.Clear(true);
        ps.Play(true);
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

        //Ensure note is hit at proper time, within the allowed delta
        if (!WasNoteHit(note.transform.localPosition.y, timingLine.localPosition.y))
        {
            return false;
        }

        //Wrong touch input
        if ((isHold && noteCode.note.type == NoteType.Tap) || (!isHold && noteCode.note.type == NoteType.Hold))
        {
            return false;
        }

        return true;
    }

    //Returns an index in the range [0-3] determining which Health/Score effects to use
    private int CalculateAccuracyIdx(GameObject note)
    {
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
        numMisses++;
    }

    public void BeginTouch(int lane)
    {
        //Debug.Log("Touch began at lane " + lane);
        SoundManager.instance.playSound(TapSound, transform, volume);

        if (lane > spawnedNotes.Count)
        {
            Debug.LogError("Invalid lane provided in TouchDetection, lanes should be 0-indexed.");
        }

        if (spawnedNotes[lane].Count == 0)
        {
            MissedNote();
            return;
        }

        GameObject note = spawnedNotes[lane].Peek();

        //Premature end touch for current note, as real end touch will come later for second hold note
        if (note.GetComponent<NoteCode>().note.type == NoteType.Hold && !note.GetComponent<NoteCode>().secondHoldNote)
        {
            EndTouch(lane, true);
        }
    }

    public void EndTouch(int lane, bool isHold)
    {
        //Debug.Log("Touch ended at lane " + lane);

        if (lane > spawnedNotes.Count)
        {
            Debug.LogError("Invalid lane provided in TouchDetection, lanes should be 0-indexed.");
        }

        if (spawnedNotes[lane].Count == 0)
        {
            MissedNote();
            return;
        }

        GameObject note = spawnedNotes[lane].Peek();

        //Check if there was a proper hit
        if (!WasProperHit(note, lane, isHold))
        {
            if (note.GetComponent<NoteCode>().secondHoldNote) //as long as hold try that
            {
                note.GetComponent<NoteCode>().DestroySelf(true);
                return;
            }

            MissedNote();
            return;
        }
        
        //Calculate how accurate the hit was and assign values accordingly
        int effectIdx = CalculateAccuracyIdx(note);
        
        currHP += healthEffects[effectIdx];

        if (currHP > maxHP)
        {
            currHP = maxHP;
        }

        score += scoreEffects[effectIdx];

        note.GetComponent<NoteCode>().DestroySelf(false);
        streak += 1;
    }
}
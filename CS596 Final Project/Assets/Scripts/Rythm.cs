using UnityEngine;
using System.Collections;

public class Rythm : MonoBehaviour
{
    //These paramaters can be changed in the future, this is only an example for now

    //Terry: I am adding variables to maintain score, streaks, etc.
    //Game manager will read these variables to determine win conditions.

    public float currHP = 100; //Heal this value depending on accuracy
    public float maxHP = 100; //MaxHP
    public float score = 0; //add to this score based on input accuracy data
    public float streak = 0; //add to this value as long as the player does not land a miss
    
    //Amount of HP being subtracted 
    float dmgValue = .1f; //Should be taken from difficulty choice in the main menu's scriptable object

    //Frequency in seconds that you lose take the dmgValue
    float drainRate = .2f; //Should be taken from difficulty choice in the main menu's scriptable object

    void Start()
    {
        //Bleed damage over time
        StartCoroutine("dmgOverTime");
    }

    //Terry: Using the data collected from inputs, determine scores and conditions within this entire script
    public void BeginTouch(GameObject startLane)
    {
        //Collect information at beginning touch such as how far off player was from note and whatnot
        Debug.Log("Touch began at lane " + startLane);

        
    }

    public void EndTouch(GameObject endLane)
    {
        //Now deal with earlier information, and check to see if touch was a hold, if so deal with it accordingly
        Debug.Log("Touch ended at lane " + endLane);
    }

    //HP Drain
    IEnumerator dmgOverTime()
    {
        while (currHP > 0)
        {
            currHP -= dmgValue;
            yield return new WaitForSeconds(drainRate);  // wait 1 second

        }

    }
}

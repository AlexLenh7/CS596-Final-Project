using UnityEngine;

public class Rythm : MonoBehaviour
{
    //These paramaters can be changed in the future, this is only an example for now

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
}

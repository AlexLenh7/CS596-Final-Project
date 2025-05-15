using UnityEngine;

public class SoundPreview : MonoBehaviour
{
    [SerializeField] AudioClip macaronMoon;
    [SerializeField] AudioClip freedomDive;
    [SerializeField] AudioClip masquerade;
    [SerializeField] AudioClip futureCandy;
    [SerializeField] AudioClip sidequest;
    [SerializeField] AudioClip renaiCirculation;

    public void MacaronMoon()
    {
        SoundManager.instance.songPreview(macaronMoon, 50f, 30f, .3f);
        //macaronMoonCover.SetActive(true);
        //macaronMoonCover.transform.SetAsLastSibling(); // bring to front
    }

    public void FreedomDive()
    {
        SoundManager.instance.songPreview(freedomDive, 28f, 30f, .3f);
        //freedomDiveCover.SetActive(true);
        //freedomDiveCover.transform.SetAsLastSibling();
    }

    public void Masquerade()
    {
        SoundManager.instance.songPreview(masquerade, 50f, 30f, .3f);
        //masqueradeCover.SetActive(true);
        //masqueradeCover.transform.SetAsLastSibling();
    }

    public void FutureCandy()
    {
        SoundManager.instance.songPreview(futureCandy, 10f, 30f, .3f);
        //futureCandyCover.SetActive(true);
        //futureCandyCover.transform.SetAsLastSibling();
    }

    public void Sidequest()
    {
        SoundManager.instance.songPreview(sidequest, 30f, 30f, .3f);
        //sidequestCover.SetActive(true);
        //sidequestCover.transform.SetAsLastSibling();
    }

    public void RenaiCirculation()
    {
        SoundManager.instance.songPreview(renaiCirculation, 57f, 30f, .5f);
        //renaiCirculationCover.SetActive(true);  
        //renaiCirculationCover.transform.SetAsLastSibling();
    }
}

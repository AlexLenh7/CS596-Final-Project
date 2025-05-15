using UnityEngine;

public class SoundPreview : MonoBehaviour
{
    [SerializeField] AudioClip macaronMoon;
    [SerializeField] AudioClip freedomDive;
    [SerializeField] AudioClip masquerade;
    [SerializeField] AudioClip futureCandy;
    [SerializeField] AudioClip sidequest;
    [SerializeField] AudioClip renaiCirculation;

    public void MacaronMoon() { SoundManager.instance.songPreview(macaronMoon, 50f, 30f, .3f); }
    public void FreedomDive() { SoundManager.instance.songPreview(freedomDive, 28f, 30f, .3f); }
    public void Masquerade() { SoundManager.instance.songPreview(masquerade, 50f, 30f, .3f); }
    public void FutureCandy() { SoundManager.instance.songPreview(futureCandy, 10f, 30f, .3f); }
    public void Sidequest() { SoundManager.instance.songPreview(sidequest, 30f, 30f, .3f); }
    public void RenaiCirculation() { SoundManager.instance.songPreview(renaiCirculation, 57f, 30f, .5f); }

    public void LoadSongMacaronMoon()
    {

    }
    public void LoadSongFreedomDive()
    {

    }
    public void LoadSongMasquerade()
    {

    }

    public void LoadSongFutureCandy()
    {

    }

    public void LoadSongSidequest()
    {

    }

    public void LoadSongRenaiCirculation()
    { 
    
    }
}

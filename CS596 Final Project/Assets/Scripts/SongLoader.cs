using UnityEngine;
using UnityEngine.SceneManagement;

public class SongLoader : MonoBehaviour
{
    public string Song;
    public void SelectAndLoadSong(string songName)
    {
        SongSelection.selectedSongName = songName;
        SceneManager.LoadScene(3); 
    }
}

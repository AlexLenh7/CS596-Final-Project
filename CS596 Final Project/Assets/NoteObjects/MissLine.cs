using UnityEngine;

public class MissLine : MonoBehaviour
{
    [SerializeField] AudioClip missSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedWith = collision.gameObject;

        if (collidedWith.CompareTag("Note"))
        {
            //Destroy Coin
            collidedWith.GetComponent<NoteCode>().DestroySelf(true);

            SoundManager.instance.playSound(missSound, transform, .15f);
        }
    }
}
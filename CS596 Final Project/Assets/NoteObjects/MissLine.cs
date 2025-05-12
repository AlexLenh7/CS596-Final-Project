using UnityEngine;

public class MissLine : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] AudioClip tapSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedWith = collision.gameObject;

        if (collidedWith.CompareTag("Note"))
        {
            //Destroy Coin
            Destroy(collidedWith);
            SoundManager.instance.playSound(tapSound, transform, .15f);
            print("Played Tap");

        }

    }

}

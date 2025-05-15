using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Rythm rythm; // reference to the Rythm component

    private void Update()
    {
        if (rythm != null)
        {
            setHealth(rythm.currHP);
        }
    }

    public void setHealth(float hp)
    {
        healthBar.value = hp;
    }
}

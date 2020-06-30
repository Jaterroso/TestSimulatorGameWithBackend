using UnityEngine;
using UnityEngine.UI;

public class BossSlider : MonoBehaviour
{
    public Slider slider;
    public int health;
    public int maxHealth;

    public void Init()
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance;

    public Slider healthSlider;
    public Image fillImage;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthSlider == null || fillImage == null) return;

        healthSlider.maxValue = max;
        healthSlider.value = current;

        // Ändra färg
        if (current >= 3) fillImage.color = Color.green;
        else if (current == 2) fillImage.color = Color.yellow;
        else fillImage.color = Color.red;
    }
}
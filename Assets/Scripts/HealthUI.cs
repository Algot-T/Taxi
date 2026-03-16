using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider slider;
    public Color fullHealthColor = Color.green;
    public Color midHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;
    private Image fillImage;

    void Awake()
    {
        if (slider != null)
        {
            fillImage = slider.fillRect.GetComponent<Image>();
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
            UpdateColor();
        }
    }

    public void SetHealth(int currentHealth)
    {
        if (slider != null)
        {
            slider.value = currentHealth;
            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        if (fillImage == null) return;

        float percent = slider.value / slider.maxValue;

        if (percent >= 0.8f)
            fillImage.color = fullHealthColor;
        else if (percent >= 0.4f)
            fillImage.color = midHealthColor;
        else
            fillImage.color = lowHealthColor;
    }
}
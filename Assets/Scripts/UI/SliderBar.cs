using UnityEngine;
using UnityEngine.UI;
public class SliderBar : MonoBehaviour {
    [SerializeField] private Slider healthSlider;
    private float currentHealth;
    private float maxValue;

    public void Set(float maxValue, float startingHealth) {
        healthSlider.value = 1.0f;
        
        currentHealth = startingHealth;
        this.maxValue = maxValue;
    }

    public void UpdateSlider(float currentHealth) {
        this.currentHealth = currentHealth;
        healthSlider.value = ConvertToDecimal();
    }

    private float ConvertToDecimal() => currentHealth / maxValue;
}
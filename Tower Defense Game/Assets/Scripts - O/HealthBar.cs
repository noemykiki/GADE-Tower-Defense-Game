using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] public SpriteRenderer fillRenderer;

    public float maxHealth;
    private float currentHealth;

    void Start()
    {
        // Initialize the health bar
        SetHealth(maxHealth);
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Calculate fill percentage
        float healthPercentage = currentHealth / maxHealth;

        // Adjust the scale of the fill sprite based on health percentage
        fillRenderer.transform.localScale = new Vector3(healthPercentage, 1, 1);

        // Optionally, adjust the position or color based on health
        // e.g., fillRenderer.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }
}

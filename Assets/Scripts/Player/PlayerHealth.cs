using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] float maxHealth;
    [SerializeField] GameEvent PlayerDied;
    [SerializeField] GameEvent OnPlayerHit;
    [SerializeField] Material healthBarMaterial;
    float _currentHealth;

    void OnEnable()
    {
        healthBarMaterial.SetFloat("_XFill", 1);
        _currentHealth = maxHealth;
    }

    public void Damage(float amount)
    {
        _currentHealth -= amount;
        OnPlayerHit?.Invoke(gameObject);
        UpdateHealthBar();
        if (_currentHealth <= 0)
        {
            PlayerDied?.Invoke(gameObject);
            gameObject.SetActive(false);
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarMaterial == null) return;
        float clampVal = Utils.RemapClamped(_currentHealth, 0, maxHealth, 0, 1);
        healthBarMaterial.SetFloat("_XFill", clampVal);
    }
}

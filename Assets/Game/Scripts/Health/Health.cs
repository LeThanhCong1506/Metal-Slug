using System;
using UnityEngine;

public class Health : MonoBehaviour, ICanTakeDamege
{
    private int _maxHealth = 100;
    private int _currentHealth;

    public Action OnHealthChanged;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }
            else if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }

            OnHealthChanged?.Invoke();
        }
    }

    private void Die()
    {

    }

    public void TakeDamage(int v)
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(int damage, GameObject source)
    {
        throw new NotImplementedException();
    }
}

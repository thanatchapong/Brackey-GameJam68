using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdate : MonoBehaviour
{
    public Slider slider;
    private Coroutine healthCoroutine;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        // Stop any ongoing transition
        if (healthCoroutine != null)
        {
            StopCoroutine(healthCoroutine);
        }

        // Start a new transition
        healthCoroutine = StartCoroutine(SmoothHealthChange(health));
    }

    private IEnumerator SmoothHealthChange(int targetHealth)
    {
        float duration = 0.3f; // Rapid but smooth
        float elapsed = 0f;
        float startValue = slider.value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, targetHealth, elapsed / duration);
            yield return null;
        }

        slider.value = targetHealth; // Ensure final value is exact
    }
}


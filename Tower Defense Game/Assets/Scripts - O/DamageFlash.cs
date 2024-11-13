using UnityEngine;
using System.Collections;

public class DamageEffectController : MonoBehaviour
{
    private Material material;
    private Coroutine flashRoutine;
    private Coroutine shakeRoutine;

    [Header("Flash Settings")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Shake Settings")]
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.2f;
    public int shakeFrequency = 20;

    private void Start()
    {
        // Get a unique instance of the material
        material = GetComponent<SpriteRenderer>().material;

        // Initialize the shader properties
        material.SetFloat("_FlashAmount", 0f);
        material.SetFloat("_ShakeAmount", 0f);
        material.SetColor("_FlashColor", flashColor);
    }

    public void TriggerDamageEffects()
    {
        // Start both the flash and shake effects
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());

        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }
        shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Set the flash amount to 1 (full flash color)
        material.SetFloat("_FlashAmount", 1f);

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        // Return flash amount to zero
        material.SetFloat("_FlashAmount", 0f);
        flashRoutine = null;
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsedTime = 0f;
        float shakeInterval = 1f / shakeFrequency;

        while (elapsedTime < shakeDuration)
        {
            // Set shake amount
            material.SetFloat("_ShakeAmount", shakeAmount);

            yield return new WaitForSeconds(shakeInterval / 2f);

            // Reset shake amount
            material.SetFloat("_ShakeAmount", 0f);

            yield return new WaitForSeconds(shakeInterval / 2f);

            elapsedTime += shakeInterval;
        }

        // Ensure shake amount is reset
        material.SetFloat("_ShakeAmount", 0f);
        shakeRoutine = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemySpawn : MonoBehaviour
{
    // Enemy prefabs
    public GameObject regularEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;

    // Wave settings
    public float timeBeforeStart;
    public float timeBetweenWaves = 10f;
    public int totalNumberOfWaves = 10;
    public int baseEnemyCount = 5;
    public int enemyCountIncrement = 2;


    // Other variables
    private int currentWaveNumber = 1;
    private bool isSpawning = false;
    public Castle castle;

    public static bool isBonusFrenzyActive = false;
    public Volume globalVolume; // Assign your global volume in the Inspector
    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;

    // Reference to coroutine
    private Coroutine spawnCoroutine;
    private Coroutine startSpawningCoroutine;
    // List of enemies to spawn in the current wave
    private List<EnemyType> enemiesToSpawn;

    void Start()
    {
        startSpawningCoroutine = StartCoroutine(StartSpawning());
        if (globalVolume != null)
        {
            VolumeProfile profile = globalVolume.profile;
            if (!profile.TryGet(out chromaticAberration))
            {
                UnityEngine.Debug.LogError("Chromatic Aberration not found in the Volume Profile");
            }
            else
            {
                chromaticAberration.active = false; // Ensure it's disabled at the start
            }
            if (!profile.TryGet(out colorAdjustments))
            {
                UnityEngine.Debug.LogError("Color Adjustments not found in the Volume Profile");
            }
            else
            {
                colorAdjustments.active = false; // Ensure it's disabled at the start
                colorAdjustments.hueShift.Override(0f); // Initialize hue shift
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Global Volume is not assigned in the EnemySpawn script.");
        }
    }

    private float GetPlayerPerformance()
    {
        if (castle != null)
        {
            return castle.GetHealthPercentage(); // Value between 0 and 1
        }
        else
        {
            UnityEngine.Debug.LogWarning("Player reference is not set in EnemySpawn.");
            return 1f; // Assume full health if player reference is missing
        }
    }

    public IEnumerator StartSpawning()
    {
        enemiesToSpawn = GenerateEnemiesForWave(currentWaveNumber);
        // Wait before starting the first wave
        yield return new WaitForSeconds(timeBeforeStart);

        while (currentWaveNumber <= totalNumberOfWaves)
        {
            UnityEngine.Debug.Log("Starting Wave " + currentWaveNumber);
            isSpawning = true;

            // Generate enemies for the current wave
            enemiesToSpawn = GenerateEnemiesForWave(currentWaveNumber);

            // Start spawning enemies
            spawnCoroutine = StartCoroutine(SpawnEnemies());

            float spawnInterval = 1f; // From SpawnEnemies() coroutine
            int enemyCount = enemiesToSpawn.Count;
            float totalSpawnDuration = enemyCount * spawnInterval;

            // Determine a random time during the spawning phase to start the bonus frenzy
            float bonusFrenzyDuration = 12f;

            // Ensure that the bonus frenzy can fit within the spawning duration
            float bonusFrenzyStartTime = 0f;
            if (totalSpawnDuration > bonusFrenzyDuration)
            {
                bonusFrenzyStartTime = UnityEngine.Random.Range(0f, totalSpawnDuration - bonusFrenzyDuration);
            }

            // Start the bonus frenzy coroutine
            StartCoroutine(BonusFrenzyCoroutine(bonusFrenzyStartTime, bonusFrenzyDuration));
            // Wait until all enemies are spawned
            yield return new WaitUntil(() => enemiesToSpawn.Count == 0);

            // Wait until all enemies are destroyed before starting the next wave
            yield return new WaitUntil(() => Enemies.enemies.Count == 0);

            isSpawning = false;
            currentWaveNumber++;

            if (currentWaveNumber <= totalNumberOfWaves)
            {
                UnityEngine.Debug.Log("Wave completed. Next wave in " + timeBetweenWaves + " seconds.");
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            else
            {
                UnityEngine.Debug.Log("All waves completed!");
            }
        }
    }

    List<EnemyType> GenerateEnemiesForWave(int waveNumber)
    {
        List<EnemyType> enemyList = new List<EnemyType>();
        int enemyCount = baseEnemyCount + (waveNumber - 1) * enemyCountIncrement;

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyType enemyType = DecideEnemyType(waveNumber);
            enemyList.Add(enemyType);
        }

        return enemyList;
    }

    EnemyType DecideEnemyType(int waveNumber)
    {
        float playerPerformance = GetPlayerPerformance();
        float rand = UnityEngine.Random.value;

        // Wave 1
        if (waveNumber < 2)
        {
            return EnemyType.Regular;
        }
        // Waves 2-3
        else if (waveNumber < 4)
        {
            if (playerPerformance >= 0.8f)
            {
                // Player is doing well; introduce challenging enemies
                if (rand < 0.7f)
                    return EnemyType.Fast;
                else
                    return EnemyType.Regular;
            }
            else if (playerPerformance >= 0.3f)
            {
                // Player is average
                if (rand < 0.7f)
                    return EnemyType.Regular;
                else
                    return EnemyType.Fast;
            }
            else
            {
                if (rand < 0.85f)
                    return EnemyType.Regular;
                else
                    return EnemyType.Fast;
            }
        }
        // Waves 4 and above
        else
        {
            if (playerPerformance >= 0.8f)
            {
                // Player is doing well; higher chance of tougher enemies
                if (rand < 0.5f)
                    return EnemyType.Fast;
                else if (rand < 0.8f)
                    return EnemyType.Tank;
                else
                    return EnemyType.Regular;
            }
            else if (playerPerformance >= 0.3f)
            {
                // Player is average; balanced enemy types
                if (rand < 0.4f)
                    return EnemyType.Fast;
                else if (rand < 0.6f)
                    return EnemyType.Tank;
                else
                    return EnemyType.Regular;
            }
            else
            {
                // Player is struggling; spawn easier enemies
                if (rand < 0.80f)
                    return EnemyType.Regular;
                else
                    return EnemyType.Fast;
            }
        }
    }


    public IEnumerator SpawnEnemies()
    {
        while (enemiesToSpawn.Count > 0)
        {

            if (enemiesToSpawn == null)
            {
                UnityEngine.Debug.LogError("enemiesToSpawn is null in SpawnEnemies!");
                yield break;
            }
            EnemyType enemyType = enemiesToSpawn[0];
            enemiesToSpawn.RemoveAt(0);

            SpawnEnemy(enemyType);

            // Adjust the spawn interval as needed
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnEnemy(EnemyType enemyType)
    {
        GameObject enemyPrefab = GetEnemyPrefab(enemyType);

        int randomIndex = UnityEngine.Random.Range(0, MapGenerator.startTile.Length);
        GameObject spawnTile = MapGenerator.startTile[randomIndex];

        if (spawnTile != null)
        {
            Instantiate(enemyPrefab, spawnTile.transform.position, Quaternion.identity);
        }
    }

    GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Regular:
                return regularEnemyPrefab;
            case EnemyType.Fast:
                return fastEnemyPrefab;
            case EnemyType.Tank:
                return tankEnemyPrefab;
            default:
                return regularEnemyPrefab;
        }
    }
    public void StopSpawning()
    {
        // Stop the main spawning coroutine
        if (startSpawningCoroutine != null)
        {
            StopCoroutine(startSpawningCoroutine);
            startSpawningCoroutine = null;
        }

        // Stop the enemy spawning coroutine if it's running
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        isSpawning = false;
    }
    void Update()
    {
        Enemy.CleanUpDestroyedEnemies();
    }
    private IEnumerator BonusFrenzyCoroutine(float startDelay, float duration)
    {
        // Wait for the start delay
        yield return new WaitForSeconds(startDelay);

        // Activate bonus frenzy
        isBonusFrenzyActive = true;
        UnityEngine.Debug.Log("Bonus Frenzy Activated!");

        // Activate chromatic aberration effect
        if (chromaticAberration != null)
        {
            chromaticAberration.active = true;
        }

        // Activate color adjustments effect
        if (colorAdjustments != null)
        {
            colorAdjustments.active = true;
        }

        // Variables for pulsing effects
        float elapsedTime = 0f;
        float pulseSpeedC = 0.2f;
        float pulseSpeedA = 2f; // Adjust this value for faster or slower pulsing
        float maxCAIntensity = 0.6f;  // Maximum chromatic aberration intensity
        float minCAIntensity = 0.2f;  // Minimum chromatic aberration intensity

        // Pulse the effects over the duration
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Chromatic Aberration Intensity (Pulsing)
            float caIntensity = Mathf.Lerp(minCAIntensity, maxCAIntensity,
                (Mathf.Sin(elapsedTime * pulseSpeedA * Mathf.PI * 2f) + 1f) / 2f);
            chromaticAberration.intensity.Override(caIntensity);

            // Hue Shift (Pulsing between -180 and 180)
            float hueShift = Mathf.Lerp(-180f, 180f,
                (Mathf.Sin(elapsedTime * pulseSpeedC * Mathf.PI * 2f) + 1f) / 2f);
            colorAdjustments.hueShift.Override(hueShift);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Deactivate bonus frenzy
        isBonusFrenzyActive = false;
        UnityEngine.Debug.Log("Bonus Frenzy Deactivated!");

        // Deactivate chromatic aberration effect
        if (chromaticAberration != null)
        {
            chromaticAberration.active = false;
            chromaticAberration.intensity.Override(0f); // Reset intensity
        }

        // Deactivate color adjustments effect
        if (colorAdjustments != null)
        {
            colorAdjustments.active = false;
            colorAdjustments.hueShift.Override(0f); // Reset hue shift
        }
    }

}
    // EnemyType enum should be in a separate script file EnemyType.cs
    public enum EnemyType
{
    Regular,
    Fast,
    Tank
}

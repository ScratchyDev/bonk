using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Survival / wave mode for ANY existing level, no new art needed.
///
/// Setup: add this to an empty object in a level scene, drag an Enemy into "Enemy Template"
/// (a prefab, or the Enemy that is already in the scene - it gets disabled and used as the template),
/// add a few spawn points, then drag this object into "Wave Spawner" on the GameManager.
/// Note: the enemy's energy slider must be a child of the enemy so every clone gets its own.
public class WaveSpawner : MonoBehaviour
{
    [Header("What to spawn")]
    public GameObject enemyTemplate;
    public Transform[] spawnPoints;
    public Transform player;

    [Header("Waves")]
    [Tooltip("How many waves to survive to win. 0 = endless (the best wave is saved).")]
    public int wavesToWin = 5;
    public int enemiesInFirstWave = 1;
    public int enemiesAddedPerWave = 1;
    [Tooltip("Never more than this many enemies at the same time.")]
    public int maxAlive = 4;
    public float timeBetweenWaves = 2.5f;
    public float spawnInterval = 0.6f;

    [Header("Difficulty growth per wave")]
    public float speedGrowth = 1.04f;
    public int extraEnergyPerWave = 1;
    public float maxSpeedMultiplier = 2f;

    public int CurrentWave { get; private set; }
    public int WavesCleared { get; private set; }
    public bool Finished { get; private set; }

    readonly List<Enemy> alive = new List<Enemy>();

    void Awake(){
        // a template that sits in the scene must not fight on its own
        if(enemyTemplate != null && enemyTemplate.scene.IsValid()){
            enemyTemplate.SetActive(false);
        }
    }

    void Start(){
        StartCoroutine(Run());
    }

    IEnumerator Run(){
        if(enemyTemplate == null){
            Debug.LogWarning("WaveSpawner: no Enemy Template assigned.");
            yield break;
        }

        Enemy template = enemyTemplate.GetComponent<Enemy>();
        if(template == null){
            Debug.LogWarning("WaveSpawner: the template needs an Enemy component.");
            yield break;
        }

        if(player == null){
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if(found != null) player = found.transform;
        }

        // let every Start() run, then wait out the "tap to start" pause (StartPaused)
        yield return null;
        while(Time.timeScale <= 0f) yield return null;

        int wave = 0;
        while(wavesToWin <= 0 || wave < wavesToWin){
            wave++;
            CurrentWave = wave;

            if(player != null){
                GameFeel.PopText(player.position + Vector3.up * 2.5f, "WAVE " + wave, Color.white, 8f);
            }
            yield return new WaitForSeconds(timeBetweenWaves);

            int toSpawn = Mathf.Max(1, enemiesInFirstWave + enemiesAddedPerWave * (wave - 1));
            int spawned = 0;
            while(spawned < toSpawn){
                Cull();
                if(alive.Count < Mathf.Max(1, maxAlive)){
                    Spawn(template, wave);
                    spawned++;
                    yield return new WaitForSeconds(spawnInterval);
                }
                else{
                    yield return null;
                }
            }

            while(true){
                Cull();
                if(alive.Count == 0) break;
                yield return null;
            }

            WavesCleared = wave;
        }

        Finished = true;
    }

    // forget (and remove from the scene) enemies that were knocked off the island
    void Cull(){
        for(int i = alive.Count - 1; i >= 0; i--){
            Enemy e = alive[i];
            if(e == null){
                alive.RemoveAt(i);
            }
            else if(e.dead){
                Destroy(e.gameObject);
                alive.RemoveAt(i);
            }
        }
    }

    void Spawn(Enemy template, int wave){
        Transform point = transform;
        if(spawnPoints != null && spawnPoints.Length > 0){
            point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        }

        GameObject clone = Instantiate(enemyTemplate, point.position, Quaternion.identity);
        Enemy enemy = clone.GetComponent<Enemy>();

        if(player != null){
            enemy.player = player;

            // the slice attack wants the player too, and a prefab cannot hold scene references
            foreach(SliceAttack slice in clone.GetComponentsInChildren<SliceAttack>(true)){
                if(slice.player == null) slice.player = player.gameObject;
            }
        }

        enemy.speed = template.speed * Mathf.Min(maxSpeedMultiplier, Mathf.Pow(speedGrowth, wave - 1));
        enemy.maxEnergy = template.maxEnergy + extraEnergyPerWave * (wave - 1);

        clone.SetActive(true);
        alive.Add(enemy);
    }
}

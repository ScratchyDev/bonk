using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Enemy enemy;
    public Boss boss;
    public GameObject player;
    public GameObject countdown;
    public SaveManager saveManager;
    public SceneLoader sceneLoader;
    public Transform spawnPoint;
    public bool isBoss;
    public int currentLevel;
    [HideInInspector] public bool counting;

    [Header("Extras (all optional)")]
    [Tooltip("More enemies that must also be knocked off before the level counts as won.")]
    public Enemy[] extraEnemies;
    [Tooltip("Survival / wave mode: the level is won when this spawner has finished all of its waves. The 'Enemy' above is then only used as a template.")]
    public WaveSpawner waveSpawner;
    [Tooltip("Losing restarts the level straight away instead of sending you back to the main menu.")]
    public bool retryOnDefeat = true;
    public float defeatDelay = 0.6f;
    [Tooltip("Scene to load after winning. -1 = back to the main menu (scene 0) like before.")]
    public int nextSceneIndex = -1;
    public KeyCode restartKey = KeyCode.R;
    public bool showHud = true;

    public float LevelTime { get; private set; }
    public float BestTime { get { return saveManager != null ? saveManager.GetBestTime(currentLevel) : 0f; } }

    bool defeated;

    void Start(){
        if(showHud && BonkUtil.FindFirst<LevelHud>() == null){
            GameObject hud = new GameObject("LevelHud");
            hud.AddComponent<LevelHud>().manager = this;
        }
    }

    void Update(){
        // timer runs in real time, but not while paused (or hit-stopped enough to read as paused)
        if(Time.timeScale > 0f && !counting && !defeated){
            LevelTime += Time.unscaledDeltaTime;
        }

        if(Input.GetKeyDown(restartKey) && Time.timeScale > 0f && !counting && !defeated){
            StartCoroutine(Defeat(true));
        }

        if(!counting && !defeated && EnemiesDefeated()){
            StartCoroutine(MainMenu());
        }
        
        if(!defeated && player.transform.position.y < -50){
            if(EnemiesDefeated()){
                LeaveLevel();
            }
            else if(isBoss && boss.health < boss.maxHealth){
                // the boss recovers one health every time the player falls, then the player tries again
                Respawn();
                boss.health++;
            }
            else{
                StartCoroutine(Defeat(false));
            }
        }
    }

    bool EnemiesDefeated(){
        if(waveSpawner != null){
            return waveSpawner.Finished;
        }

        if(isBoss){
            return boss != null && boss.dead;
        }

        bool any = false;

        if(enemy != null){
            any = true;
            if(!enemy.dead) return false;
        }

        if(extraEnemies != null){
            foreach(Enemy extra in extraEnemies){
                if(extra == null) continue;
                any = true;
                if(!extra.dead) return false;
            }
        }

        return any;
    }

    void Respawn(){
        player.transform.position = spawnPoint.position;

        // without this the player kept the speed of the 50 unit fall and tore straight through the floor
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if(playerRb != null){
            playerRb.velocity = Vector2.zero;
        }
    }

    void LeaveLevel(){
        if(nextSceneIndex >= 0){
            sceneLoader.LoadScene(nextSceneIndex);
        }
        else{
            sceneLoader.LoadScene(0);
        }
    }

    IEnumerator Defeat(bool instant){
        defeated = true;

        if(waveSpawner != null){
            saveManager.RecordWave(waveSpawner.WavesCleared);
        }

        if(!instant){
            GameFeel.PopText(spawnPoint.position + Vector3.up * 2f, "BONKED!", new Color(1f, 0.4f, 0.3f), 8f);
            yield return new WaitForSecondsRealtime(defeatDelay);
        }

        if(retryOnDefeat || instant){
            sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else{
            sceneLoader.LoadScene(0);
        }
    }

    IEnumerator MainMenu(){
        if(saveManager.completionLevel < currentLevel){ 
            saveManager.completionLevel = currentLevel;
        }
        counting = true;

        float previousBest = saveManager.GetBestTime(currentLevel);
        if(saveManager.RecordLevelTime(currentLevel, LevelTime) && previousBest > 0f){
            GameFeel.PopText(player.transform.position + Vector3.up * 2.5f, "NEW BEST! " + LevelTime.ToString("0.0") + "s", new Color(1f, 0.85f, 0.2f), 6f);
        }

        if(waveSpawner != null){
            saveManager.RecordWave(waveSpawner.WavesCleared);
        }

        // winning while scoped (slow-motion) would otherwise make the 3 second countdown last 12
        if(Time.timeScale > 0f && Time.timeScale < 1f){
            Time.timeScale = 1f;
        }

        countdown.SetActive(true);
        countdown.GetComponent<CountDownTimer>().StartTimer(3f);
        yield return new WaitForSeconds(3f);
        LeaveLevel();
    }
}

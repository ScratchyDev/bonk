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

    void Update(){
        if(!counting){
            if(!isBoss && enemy.dead){
                    StartCoroutine(MainMenu());
            }
            else if(isBoss && boss.dead){
                    StartCoroutine(MainMenu());
            }
        }
        
        if(player.transform.position.y < -50){
            if(!isBoss && enemy.dead){
                sceneLoader.LoadScene(0);
            }
            else if(isBoss && boss.dead){
                sceneLoader.LoadScene(0);
            }
            else{
                player.transform.position = spawnPoint.position;
                
                if(isBoss && boss.health < boss.maxHealth){
                    boss.health++;
                }
                else{
                    sceneLoader.LoadScene(0);
                }
            }
        }
    }

    IEnumerator MainMenu(){
        if(saveManager.completionLevel < currentLevel){ 
            saveManager.completionLevel = currentLevel;
        }
        counting = true;
        countdown.SetActive(true);
        countdown.GetComponent<CountDownTimer>().StartTimer(5f);
        yield return new WaitForSeconds(5f);
        sceneLoader.LoadScene(0);
    }
}

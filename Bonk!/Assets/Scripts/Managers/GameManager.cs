using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Enemy enemy;
    public Boss boss;
    public GameObject player;
    public GameObject countdown;
    public Transform spawnPoint;
    public bool isBoss;
    [HideInInspector]
    public bool counting;

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
                SceneManager.LoadScene(0);
            }
            else if(isBoss && boss.dead){
                SceneManager.LoadScene(0);
            }
            else{
                player.transform.position = spawnPoint.position;
                
                if(isBoss && boss.health < boss.maxHealth){
                    boss.health++;
                }
                else{
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

    IEnumerator MainMenu(){
        counting = true;
        countdown.SetActive(true);
        countdown.GetComponent<CountDownTimer>().StartTimer(5f);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketBall : MonoBehaviour
{
    [Header("Variables")]
    public float decayTime;
    [HideInInspector] public int damage;

    void Start()
    {
        StartCoroutine(Decay());
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        Enemy enemy = collider2D.gameObject.GetComponent<Enemy>();

        if(enemy != null){
            enemy.Stun(damage);
        } else {
            Boss boss = collider2D.gameObject.GetComponent<Boss>();
            if(boss != null){
                boss.Stun();
            }
        }
    }

    IEnumerator Decay(){
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }
}

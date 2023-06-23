using System.Collections;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Attributes")]
    public int damage;
    public float reach;

    [Header("References")]
    public EnergyManager energyManager;
    Animator animator;

    bool attacking;

    void Start(){
        animator = GetComponent<Animator>();
        attacking = false;
    }

    public void Attack(){
        if(attacking == false){
        animator.SetBool("attacking", true);
        attacking = true;
        StartCoroutine(StopAttack());
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        Enemy enemy = collider2D.gameObject.GetComponent<Enemy>();
        if(enemy != null){
            enemy.Stun();
        }
        else{
            Boss boss = collider2D.gameObject.GetComponent<Boss>();
            if(boss != null){
                boss.Stun();
            }
        }
    }

    IEnumerator StopAttack(){
        yield return new WaitForSeconds(0.75f);
        animator.SetBool("attacking", false);
        attacking = false;
    }
}
using System.Collections;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Attributes")]
    public int damage;
    public float reach;
    public float attackCooldown;

    public bool isRanged;

    [Header("References")]
    public EnergyManager energyManager;
    Animator animator;

    bool attacking;
    float cooldown;

    void Start(){
        animator = GetComponent<Animator>();
        attacking = false;
    }

    void Update(){
        cooldown -= Time.deltaTime;
    }

    public void Attack(){
        if(attacking == false){
            animator.SetBool("attacking", true);
            attacking = true;
            if(isRanged){
                WeaponFunction(); 
            }
            StartCoroutine(StopAttack());
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        if(!isRanged){
            Enemy enemy = collider2D.gameObject.GetComponent<Enemy>();
            if(enemy != null && cooldown <= 0){
                cooldown = attackCooldown;
                enemy.Stun(damage);
                WeaponFunction();
            } else {
                Boss boss = collider2D.gameObject.GetComponent<Boss>();
                if(boss != null){
                    boss.Stun();
                    WeaponFunction();
                }
            }
        }
    }

    IEnumerator StopAttack(){
        yield return new WaitForSeconds(0.75f);
        animator.SetBool("attacking", false);
        attacking = false;
    }

    public virtual void WeaponFunction(){
        Debug.Log("No weapon function");
    }
}
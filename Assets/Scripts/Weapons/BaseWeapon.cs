using System.Collections;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Variables")]
    public int damage;
    public float reach;
    public float attackCooldown;

    public bool isRanged;

    [Header("Weapon Tuning")]
    [Tooltip("How long one attack locks the weapon (this used to be hard-coded to 0.75).")]
    public float attackDuration = 0.75f;
    [HideInInspector] public float damageMultiplier = 1f; // changed by power-ups

    [Header("Weapon References")]
    public EnergyManager energyManager;
    Animator animator;

    bool attacking;
    float cooldown;

    /// Damage after power-ups. Weapons should use this instead of the raw damage field.
    public int EffectiveDamage {
        get {
            int scaled = Mathf.RoundToInt(damage * damageMultiplier);
            return damage > 0 ? Mathf.Max(1, scaled) : damage;
        }
    }

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
                enemy.Stun(EffectiveDamage);
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
        yield return new WaitForSeconds(attackDuration);
        animator.SetBool("attacking", false);
        attacking = false;
    }

    public virtual void WeaponFunction(){
    }
}
